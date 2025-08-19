using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapLightGenerator : MonoBehaviour
{
    [Header("角色配置")]
    [SerializeField] private Image indicatorImage; // 小地图标记图像

    [Header("地图数据")]
    [SerializeField] public MiniMapGenerator mapGenerator;
    
    #region 内部状态
    private BoundsInt effectiveArea;
    private BoundsInt maskArea;
    private Vector3Int playerTilePosition;
    private Vector3 previousPosition;
    #endregion

    private RectTransform rectTransform;
    
    
    public void UpdateIndicatorPosition(Vector3 tilePos)
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero; // 锚点左上角
        rectTransform.anchorMax = Vector2.zero; // 锚点左上角
        rectTransform.pivot = new Vector2(0.5f, 0.5f); // 中心点为图标中心
        effectiveArea = mapGenerator.effectiveRect;
        // 获取瓦片在 effectiveRect 的相对坐标
        int localX = (int)(tilePos.x - effectiveArea.xMin);
        int localY = (int)(tilePos.y - effectiveArea.yMin);
        // 每个瓦片显示为 4x4 像素，图标居中
        float pixelX = localX * 4 + 2;
        float pixelY = localY * 4 + 2;
        // 获取小地图图像的大小（纹理尺寸）
        Sprite sprite = mapGenerator.mapImage.sprite;
        float textureWidth = sprite.rect.width;  // 例如：48
        float textureHeight = sprite.rect.height; // 例如：32
        // 不使用 mapImage.rect.width（因为那是 Canvas UI 的显示尺寸，受缩放影响）
        // 这里我们使用实际纹理尺寸进行映射
        float normalizedX = pixelX / textureWidth;
        float normalizedY = pixelY / textureHeight;
        // 获取 parent RectTransform（mapImage 的 rectTransform）
        RectTransform mapRectTransform = mapGenerator.mapImage.rectTransform;
        // 计算锚定位置（UI 坐标系，原点在左下角）
        float anchoredPosX = normalizedX * mapRectTransform.rect.width;
        float anchoredPosY = normalizedY * mapRectTransform.rect.height;
        rectTransform.anchoredPosition = new Vector2(anchoredPosX, anchoredPosY);
        // 设置图标尺寸（原尺寸 3x3）
        float iconSize = 3.0f;
        rectTransform.sizeDelta = new Vector2(iconSize, iconSize);
    }
}
