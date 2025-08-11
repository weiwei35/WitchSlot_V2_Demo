using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MiniMapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public string wallTag = "MapWall"; // 筛选条件
    public SpriteRenderer mapDisplay; // 显示小地图的精灵渲染器

    public Image mapImage;
    
    private int[,] mapData;
    private BoundsInt cellBounds;
    private Vector3Int minPosition;

    void OnEnable()
    {
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
        }

        mapImage = GameObject.FindWithTag("MiniMap").GetComponent<Image>();
        AnalyzeTilemap();
        GenerateMiniMap();
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
            if (mapDisplay != null) mapDisplay.sprite = null;
            return;
        }
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f); // 居中显示
        Sprite sprite = Sprite.Create(texture, rect, pivot);
    
        if (mapDisplay != null)
        {
            mapImage.sprite = sprite;
            mapImage.SetNativeSize();
            mapImage.transform.localScale = new Vector3(5, 5, 5);
        }
    }
    private BoundsInt effectiveRect;

    void CalculateEffectiveArea()
    {
        int width = cellBounds.size.x;
        int height = cellBounds.size.y;

        int minX = width, minY = height;
        int maxX = -1, maxY = -1;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (mapData[x, y] == 1)
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }

        // 裁剪后尺寸
        if (maxX < 0 || maxY < 0 || minX >= width || minY >= height)
        {
            // 没有有效瓦片时输出空白小地图
            effectiveRect = new BoundsInt();
            effectiveRect.size = Vector3Int.zero;
        }
        else
        {
            effectiveRect = new BoundsInt(minX, minY, 0, (maxX - minX) + 1, (maxY - minY) + 1,0);
        }
    }
    Texture2D GenerateMiniMapSection(int pixelPerTile = 4)
    {
        int effWidth = effectiveRect.size.x;
        int effHeight = effectiveRect.size.y;

        Texture2D texture = new Texture2D(effWidth * pixelPerTile, effHeight * pixelPerTile);
        texture.filterMode = FilterMode.Point;

        for (int x = 0; x < effWidth; x++)
        {
            for (int y = 0; y < effHeight; y++)
            {
                int originX = effectiveRect.xMin + x;
                int originY = effectiveRect.yMin + y;

                Color pixelColor = mapData[originX, originY] == 1 ? Color.black : Color.white;

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
