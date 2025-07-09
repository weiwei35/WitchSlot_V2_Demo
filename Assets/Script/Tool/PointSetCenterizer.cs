using System.Collections.Generic;
using UnityEngine;

public static class PointSetCenterizer
{
    /// <summary>
    /// 重新为中心点集并更新所有点的坐标
    /// </summary>
    /// <param name="points">原始点集</param>
    /// <returns>包含新原点和转换后点集的结构体</returns>
    public static RecenteredPointSet RecenterPointSet(List<Vector2Int> points)
    {
        // 处理空输入
        if (points == null || points.Count == 0)
        {
            Debug.LogWarning("点集为空，无法重新确定中心");
            return new RecenteredPointSet
            {
                NewOrigin = Vector2Int.zero,
                RecenteredPoints = new List<Vector2Int>(),
                IsValid = false
            };
        }

        // 如果点集只有一个点，直接返回处理
        if (points.Count == 1)
        {
            return new RecenteredPointSet
            {
                NewOrigin = points[0],
                RecenteredPoints = new List<Vector2Int> { Vector2Int.zero },
                IsValid = true
            };
        }

        // 1. 计算几何重心点
        Vector2 centroid = CalculateCentroid(points);
        
        // 2. 寻找距离重心最近的点作为新原点
        Vector2Int newOrigin = FindClosestToCentroid(points, centroid);
        
        // 3. 创建转换矩阵
        Matrix4x4 transformationMatrix = CreateTranslationMatrix(newOrigin);
        
        // 4. 转换所有点坐标
        List<Vector2Int> transformedPoints = TransformPoints(points, newOrigin);
        
        return new RecenteredPointSet
        {
            NewOrigin = newOrigin,
            OriginalPoints = new List<Vector2Int>(points),
            RecenteredPoints = transformedPoints,
            Centroid = centroid,
            IsValid = true
        };
    }

    // 计算点集几何重心
    private static Vector2 CalculateCentroid(List<Vector2Int> points)
    {
        Vector2 centroid = Vector2.zero;
        
        foreach (var point in points)
        {
            centroid.x += point.x;
            centroid.y += point.y;
        }
        
        centroid.x /= points.Count;
        centroid.y /= points.Count;
        
        return centroid;
    }

    // 寻找距离重心最近的点
    private static Vector2Int FindClosestToCentroid(List<Vector2Int> points, Vector2 centroid)
    {
        float minDistanceSqr = float.MaxValue;
        Vector2Int closestPoint = points[0];
        
        // 尝试寻找距离重心最近的点
        foreach (var point in points)
        {
            float distSqr = (point.x - centroid.x) * (point.x - centroid.x) + 
                            (point.y - centroid.y) * (point.y - centroid.y);
            
            if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                closestPoint = point;
            }
        }
        
        // 检查是否有多个点距离相同
        List<Vector2Int> candidates = new List<Vector2Int>();
        List<Vector2Int> contenders = new List<Vector2Int>();
        
        foreach (var point in points)
        {
            float distSqr = (point.x - centroid.x) * (point.x - centroid.x) + 
                            (point.y - centroid.y) * (point.y - centroid.y);
            
            if (Mathf.Approximately(distSqr, minDistanceSqr))
            {
                contenders.Add(point);
            }
        }
        
        // 如果有多个点距离相同，选择最接近整体中心的点
        if (contenders.Count > 1)
        {
            // 计算所有距离重心的点中哪个最接近所有点的中心位置
            Vector2Int middleOfContenders = CalculateCenter(contenders);
            
            minDistanceSqr = float.MaxValue;
            foreach (var contender in contenders)
            {
                float distSqr = (contender.x - middleOfContenders.x) * (contender.x - middleOfContenders.x) + 
                                (contender.y - middleOfContenders.y) * (contender.y - middleOfContenders.y);
                
                if (distSqr < minDistanceSqr)
                {
                    minDistanceSqr = distSqr;
                    closestPoint = contender;
                }
            }
        }
        
        return closestPoint;
    }

    // 计算点集的整数中心位置
    private static Vector2Int CalculateCenter(List<Vector2Int> points)
    {
        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;
        
        foreach (var point in points)
        {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.y > maxY) maxY = point.y;
        }
        
        return new Vector2Int((minX + maxX) / 2, (minY + maxY) / 2);
    }

    // 创建用于坐标转换的矩阵
    private static Matrix4x4 CreateTranslationMatrix(Vector2Int origin)
    {
        // 创建平移矩阵
        return Matrix4x4.Translate(new Vector3(-origin.x, -origin.y, 0));
    }

    // 应用坐标转换
    private static List<Vector2Int> TransformPoints(List<Vector2Int> points, Vector2Int newOrigin)
    {
        List<Vector2Int> transformedPoints = new List<Vector2Int>();
        
        foreach (var point in points)
        {
            Vector2Int newPoint = new Vector2Int(
                point.x - newOrigin.x,
                point.y - newOrigin.y
            );
            transformedPoints.Add(newPoint);
        }
        
        return transformedPoints;
    }

    /// <summary>
    /// 中心化点集的可视化数据结构
    /// </summary>
    public struct RecenteredPointSet
    {
        /// <summary>
        /// 是否有效转换
        /// </summary>
        public bool IsValid;
        
        /// <summary>
        /// 新的原点(0,0)在原坐标系中的位置
        /// </summary>
        public Vector2Int NewOrigin;
        
        /// <summary>
        /// 点集的几何重心
        /// </summary>
        public Vector2 Centroid;
        
        /// <summary>
        /// 原始点集数据
        /// </summary>
        public List<Vector2Int> OriginalPoints;
        
        /// <summary>
        /// 以新原点为基准的坐标集合
        /// </summary>
        public List<Vector2Int> RecenteredPoints;
        
        /// <summary>
        /// 可视化点集数据
        /// </summary>
        public void Visualize(string prefix = "点集")
        {
            // if (!IsValid) return;
            
            Debug.Log($"{prefix}中心化结果:");
            Debug.Log($"新原点: {NewOrigin} (原始坐标)");
            Debug.Log($"几何重心: ({Centroid.x:F2}, {Centroid.y:F2})");
            Debug.Log($"包含{RecenteredPoints.Count}个点:");
            
            for (int i = 0; i < OriginalPoints.Count; i++)
            {
                Debug.Log($"原始点[{i}]: {OriginalPoints[i]} → 新位置: {RecenteredPoints[i]}");
            }
        }
    }
}
