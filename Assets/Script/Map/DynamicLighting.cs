using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[RequireComponent(typeof(Tilemap))]
public class DynamicLighting : MonoBehaviour
{
    [Header("光照设置")] 
    public GameObject player;
    public Vector3Int lightSourcePosition; // 光源位置(网格坐标)
    public float maxLightIntensity = 1.5f;  // 光源中心亮度
    public float minLightIntensity = 0.3f;  // 最小光照强度
    public float decayRate = 0.85f;        // 光照衰减率
    public float updateFrequency = 0.1f;   // 光照更新频率(秒)

    [Header("可视化调试")]
    public bool showLightRadius = true;
    public Color highlightColor = new Color(1f, 1f, 0.7f, 1f);

    private Tilemap targetTilemap;
    private Dictionary<Vector3Int, float> tileLightLevels = new Dictionary<Vector3Int, float>();
    private Dictionary<Vector3Int, Color> originalColors = new Dictionary<Vector3Int, Color>();
    
    private float timer = 0f;
    private bool isInitialized = false;

    void Start()
    {
        targetTilemap = GetComponent<Tilemap>();
        InitializeLightSystem();
    }

    private void OnEnable()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateFrequency)
        {
            UpdateLighting();
            timer = 0f;
        }
        
        if (showLightRadius && Application.isPlaying)
        {
            DrawLightRadiusDebug();
        }
    }
    
    // 初始化光照系统并缓存原始颜色
    void InitializeLightSystem()
    {
        foreach (var position in targetTilemap.cellBounds.allPositionsWithin)
        {
            if (targetTilemap.HasTile(position))
            {
                originalColors[position] = targetTilemap.GetColor(position);
            }
        }
        isInitialized = true;
        UpdateLighting();
    }
    
    // 更新所有瓦片的光照状态
    void UpdateLighting()
    {
        if (!isInitialized) return;
        lightSourcePosition = targetTilemap.WorldToCell(player.transform.position);
        tileLightLevels.Clear();
        
        BoundsInt bounds = targetTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                
                if (targetTilemap.HasTile(tilePos))
                {
                    float lightValue = CalculateLightValue(tilePos, lightSourcePosition);
                    tileLightLevels[tilePos] = lightValue;
                    ApplyLightToTile(tilePos, lightValue);
                }
            }
        }
    }
    
    // 计算单个瓦片的光照强度（使用衰减公式）
    float CalculateLightValue(Vector3Int tilePosition, Vector3Int lightPosition)
    {
        // 计算欧几里得距离（对角线也计算在内）
        float distance = Vector3Int.Distance(tilePosition, lightPosition);
        
        // 指数衰减公式：亮度 = (衰减率)^距离 * 最大亮度
        float lightValue = Mathf.Pow(decayRate, distance) * maxLightIntensity;
        
        return Mathf.Clamp(lightValue, minLightIntensity, maxLightIntensity);
    }
    
    // 应用光照效果到单个瓦片
    void ApplyLightToTile(Vector3Int position, float intensity)
    {
        if (!originalColors.ContainsKey(position)) return;
        
        Color baseColor = originalColors[position];
        Color modifiedColor = baseColor * intensity;
        modifiedColor.a = baseColor.a; // 保留原始alpha值
        
        targetTilemap.SetTileFlags(position, TileFlags.None);
        targetTilemap.SetColor(position, modifiedColor);
    }
    
    // 调试可视化：显示光照半径
    void DrawLightRadiusDebug()
    {
        float maxDistance = Mathf.Log(minLightIntensity / maxLightIntensity) / Mathf.Log(decayRate);
        
        // 计算有效辐射半径（向上取整）
        int radius = Mathf.CeilToInt(maxDistance);
        
        // 绘制光照范围方块
        for (int offsetX = -radius; offsetX <= radius; offsetX++)
        {
            for (int offsetY = -radius; offsetY <= radius; offsetY++)
            {
                Vector3Int gridPosition = lightSourcePosition + new Vector3Int(offsetX, offsetY, 0);
                
                // 只绘制有效的瓦片位置
                if (targetTilemap.HasTile(gridPosition) && 
                    Vector3Int.Distance(lightSourcePosition, gridPosition) <= maxDistance)
                {
                    float opacity = Mathf.Clamp01((maxDistance - Vector3Int.Distance(lightSourcePosition, gridPosition)) / maxDistance);
                    Color debugColor = highlightColor * opacity;
                    debugColor.a = opacity * 0.3f; // 半透明效果
                    
                    // 创建调试矩形
                    Vector3 center = targetTilemap.CellToWorld(gridPosition) + new Vector3(0.5f, 0.5f, 0);
                    Vector3 size = new Vector3(1, 1, 0) * targetTilemap.cellSize.x;
                    Debug.DrawLine(center - size * 0.5f, center + new Vector3(size.x * 0.5f, -size.y * 0.5f, 0), debugColor);
                    Debug.DrawLine(center - size * 0.5f, center + new Vector3(-size.x * 0.5f, size.y * 0.5f, 0), debugColor);
                    Debug.DrawLine(center + size * 0.5f, center + new Vector3(-size.x * 0.5f, size.y * 0.5f, 0), debugColor);
                    Debug.DrawLine(center + size * 0.5f, center + new Vector3(size.x * 0.5f, -size.y * 0.5f, 0), debugColor);
                }
            }
        }
    }
    
    // 辅助功能：动态更改光源位置
    public void SetLightSourcePosition(object o)
    {
        Vector3 newPosition = (Vector3)o;
        lightSourcePosition = targetTilemap.WorldToCell(newPosition);
        UpdateLighting();
    }
    
    // 辅助功能：获取瓦片的光照强度
    public float GetTileLightIntensity(Vector3Int position)
    {
        if (tileLightLevels.ContainsKey(position))
            return tileLightLevels[position];
        return minLightIntensity;
    }
    
    // 辅助功能：重置瓦片原始颜色
    void OnDisable()
    {
        if (!isInitialized) return;
        
        foreach (var position in originalColors.Keys)
        {
            if (targetTilemap.HasTile(position))
            {
                targetTilemap.SetColor(position, originalColors[position]);
            }
        }
    }
}
