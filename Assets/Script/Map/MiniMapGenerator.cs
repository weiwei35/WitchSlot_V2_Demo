using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MiniMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public string wallTag = "MapWall"; // 筛选条件

    public PlayerMiniMapIndicator playerMini;

    public Image mapImage;
    
    private int[,] mapData;
    public BoundsInt cellBounds;
    private Vector3Int minPosition;
    
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

                if (tile != null)
                {
                    mapData[x, y] = IsWallTile(tile) ? 1 : 0;
                }
                else
                {
                    mapData[x, y] = 0;
                }
            }
        }
    }

    // 判断Tile是否为Wall（可根据需要扩展）
    bool IsWallTile(TileBase tile)
    {
        return tile.name.Contains(wallTag); // 简单通过名称识别
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
                    mapData[arrayX, arrayY] == 1)
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
                Color pixelColor = tileVal == 1 ? Color.black : Color.white;

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
}
