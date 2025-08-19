using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MiniMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public string wallTag = "MapWall"; // 筛选条件
    public string lightTag = "Light"; // 暴露在Inspector中

    private bool IsWallTile(TileBase tile) => tile.name.Contains(wallTag);
    private bool IsLightTile(TileBase tile) => tile.name.Contains(lightTag);


    public PlayerMiniMapIndicator playerMini;

    public Image mapImage;
    
    private int[,] mapData; // 0(空) / 1(墙) / 2(灯)

    public BoundsInt cellBounds;
    private Vector3Int minPosition;
    
    private List<Vector3Int> lightPositions = new List<Vector3Int>();
    public List<Vector3Int> lightInArea = new List<Vector3Int>();
    
    public event Action OnMapGenerated;

    public void SetMap()
    {
        StartCoroutine(SetupMiniMap());
    }

    IEnumerator SetupMiniMap()
    {
        yield return new WaitForSeconds(0.5f);
        tilemap = GameObject.FindWithTag("MapWall").GetComponent<Tilemap>();
                
        AnalyzeTilemap();
        GenerateMiniMap();
        playerMini.Initialize();
    }

    // 分析Tilemap数据
    void AnalyzeTilemap()
    {
        cellBounds = tilemap.cellBounds;
        minPosition = cellBounds.min;

        int width = cellBounds.size.x;
        int height = cellBounds.size.y;
        mapData = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int position = new Vector3Int(minPosition.x + x, minPosition.y + y, 0);
                TileBase tile = tilemap.GetTile(position);

                if (tile != null) {
                    if (IsWallTile(tile)) mapData[x,y] = 1;
                    else if (IsLightTile(tile))
                    {
                        mapData[x, y] = 2;
                        lightPositions.Add(position);
                    }
                    else mapData[x,y] = 0;
                } else {
                    mapData[x,y] = 0;
                }
            }
        }
    }

    void GenerateMiniMap()
    {
        CalculateEffectiveArea();
        int pixelPerTile = 4; // 保持像素放大系数一致
        Texture2D texture = GenerateMiniMapSection(pixelPerTile);
        if (texture.width == 0 || texture.height == 0)
        {
            Debug.Log("无有效区域");
            return;
        }
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f); // 居中显示
        Sprite sprite = Sprite.Create(texture, rect, pivot);
    
        mapImage.sprite = sprite;
        mapImage.SetNativeSize();
        mapImage.transform.localScale = new Vector3(5, 5, 5);
        OnMapGenerated?.Invoke();
    }
    public BoundsInt effectiveRect;

    private void CalculateEffectiveArea()
    {
        BoundsInt bounds = cellBounds;

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        // 获取有效区域的起始坐标（用于映射到 mapData）
        Vector3Int effectiveOrigin = bounds.position;
        int arrayWidth = mapData.GetLength(0);
        int arrayHeight = mapData.GetLength(1);

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                // 将世界坐标转换为 mapData 数组索引
                int arrayX = x - effectiveOrigin.x;
                int arrayY = y - effectiveOrigin.y;

                // 仅在范围内访问数组
                if (arrayX >= 0 && arrayY >= 0 && 
                    arrayX < arrayWidth && arrayY < arrayHeight &&
                    mapData[arrayX, arrayY] != 0)
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }

        // 空地图逻辑优化
        if (maxX <= minX || maxY <= minY)
        {
            effectiveRect = new BoundsInt(Vector3Int.zero, Vector3Int.zero);
        }
        else
        {
            effectiveRect = new BoundsInt(
                new Vector3Int(minX, minY, 0),
                new Vector3Int(maxX - minX + 1, maxY - minY + 1, 0)
            );
        }
    }

    Color GetTileColor(int tileType) {
        switch(tileType) {
            case 1: return Color.black; // 墙壁
            case 2: return Color.yellow; // 灯光
            default: return Color.white; // 背景
        }
    }
    public Texture2D GenerateMiniMapSection(int pixelPerTile = 4)
    {
        int effWidth = effectiveRect.size.x;
        int effHeight = effectiveRect.size.y;

        Texture2D texture = new Texture2D(effWidth * pixelPerTile, effHeight * pixelPerTile);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < effWidth; x++)
        {
            for (int y = 0; y < effHeight; y++)
            {
                int worldX = effectiveRect.xMin + x;
                int worldY = effectiveRect.yMin + y;

                // 转换为 mapData 的相对索引
                int arrayX = worldX - cellBounds.xMin;
                int arrayY = worldY - cellBounds.yMin;

                // 确保数组索引合法
                bool inBounds = arrayX >= 0 && arrayY >= 0 &&
                                arrayX < mapData.GetLength(0) &&
                                arrayY < mapData.GetLength(1);

                int tileVal = inBounds ? mapData[arrayX, arrayY] : 0;
                Color pixelColor = GetTileColor(tileVal);;

                // 填充每个格子为 4x4 像素
                for (int px = 0; px < pixelPerTile; px++)
                {
                    for (int py = 0; py < pixelPerTile; py++)
                    {
                        texture.SetPixel(x * pixelPerTile + px, y * pixelPerTile + py, pixelColor);
                    }
                }
            }
        }

        texture.Apply();
        return texture;
    }

    private Vector3Int maskCenter;
    private BoundsInt maskArea;
    public void CheckLightInArea(BoundsInt area, Vector3Int center)
    {
        maskCenter = center;
        maskArea = area;
        foreach (var lightPos in lightPositions)
        {
            if (lightPos.x > area.xMin && lightPos.x < area.xMax && lightPos.y > area.yMin && lightPos.y < area.yMax)
            {
                if (!lightInArea.Contains(lightPos))
                {
                    lightInArea.Add(lightPos);
                }
            }
            else
            {
                if (lightInArea.Contains(lightPos))
                {
                    lightInArea.Remove(lightPos);
                }
            }
        }

        SetLightOutArea();
    }

    public MapLightGenerator lightPrefab;
    private List<GameObject> lightObjects = new List<GameObject>();
    private void SetLightOutArea()
    {
        foreach (var light in lightObjects)
        {
            Destroy(light);
        }
        foreach (var light in lightPositions)
        {
            if (!lightInArea.Contains(light))
            {
                List<Vector2> intersections = FindEdgeIntersections(maskArea, light, maskCenter);
                foreach (var pt in intersections) {
                    // Debug.Log($"交点坐标: ({pt.x}, {pt.y})");
                    var lightObj = Instantiate(lightPrefab, transform);
                    lightObjects.Add(lightObj.gameObject);
                    lightObj.mapGenerator = this;
                    lightObj.UpdateIndicatorPosition(pt);
                }
            }
        }
    }
    private struct LineSegment {
        public Vector2 start, end;
    }
    /// <summary>
    /// 计算线段与矩形区域的边界交点
    /// </summary>
    public static List<Vector2> FindEdgeIntersections(BoundsInt area, Vector3Int pointA, Vector3Int pointB) 
    {
        List<Vector2> intersections = new List<Vector2>();
    
        // 转换为二维坐标
        Vector2 a = new Vector2(pointA.x, pointA.y);
        Vector2 b = new Vector2(pointB.x, pointB.y);
    
        // 定义矩形边界线段
        List<LineSegment> rectEdges = GetRectEdges(area);

        // 检测交点
        foreach (var edge in rectEdges) 
        {
            if (LineLineIntersection(a, b, edge.start, edge.end, out Vector2 intersection))
            {
                // 保证交点在线段内
                if (IsWithinSegment(intersection, edge.start, edge.end))
                    intersections.Add(intersection);
            }
        }

        // 去重算法
        return RemoveDuplicatePoints(intersections);
    }

    private static List<LineSegment> GetRectEdges(BoundsInt area)
    {
        float l = area.xMin;
        float r = area.xMax;
        float d = area.yMin;
        float t = area.yMax;

        return new List<LineSegment> {
            new LineSegment { start = new Vector2(l, d), end = new Vector2(r, d) }, // 底边
            new LineSegment { start = new Vector2(r, d), end = new Vector2(r, t) }, // 右边
            new LineSegment { start = new Vector2(r, t), end = new Vector2(l, t) }, // 顶边
            new LineSegment { start = new Vector2(l, t), end = new Vector2(l, d) }  // 左边
        };
    }
// 检测两条线段是否相交
    public static bool LineLineIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2, out Vector2 intersection)
    {
        // 计算方向向量
        Vector2 da = a2 - a1;
        Vector2 db = b2 - b1;
    
        // 计算分母
        float denominator = da.x * db.y - da.y * db.x;
    
        if (Mathf.Approximately(denominator, 0)) {
            intersection = Vector2.zero;
            return false; // 平行线
        }

        // 参数方程系数
        float t = (db.x * (a1.y - b1.y) - db.y * (a1.x - b1.x)) / denominator;
        float u = (da.x * (a1.y - b1.y) - da.y * (a1.x - b1.x)) / denominator;
    
        // 保证交点在线段之间
        if (t >= 0 && t <= 1 && u >= 0 && u <= 1) {
            intersection = a1 + t * da;
            return true;
        }
    
        intersection = Vector2.zero;
        return false;
    }

    private static bool IsWithinSegment(Vector2 point, Vector2 segA, Vector2 segB)
    {
        float crossProduct = (point.y - segA.y) * (segB.x - segA.x) - 
                             (point.x - segA.x) * (segB.y - segA.y);
    
        if (!Mathf.Approximately(crossProduct, 0)) return false;
    
        float dotProduct = (point.x - segA.x) * (segB.x - segA.x) + 
                           (point.y - segA.y) * (segB.y - segA.y);
    
        if (dotProduct < 0) return false;
    
        float squaredLength = (segB.x - segA.x) * (segB.x - segA.x) + 
                              (segB.y - segA.y) * (segB.y - segA.y);
                         
        return dotProduct <= squaredLength;
    }
    private static List<Vector2> RemoveDuplicatePoints(List<Vector2> points, float threshold = 0.1f)
    {
        List<Vector2> uniquePoints = new List<Vector2>();
    
        foreach (var pt in points)
        {
            if (!uniquePoints.Any(p => Vector2.Distance(p, pt) < threshold))
                uniquePoints.Add(pt);
        }
    
        return uniquePoints;
    }

}
