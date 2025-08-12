using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PlayerMiniMapIndicator : MonoBehaviour
{
    #region 单例模式
    public static PlayerMiniMapIndicator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region 核心配置
    [Header("角色配置")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Image indicatorImage; // 小地图标记图像

    [Header("地图数据")]
    [SerializeField] private MiniMapGenerator mapGenerator;
    
    [Header("调试设置")]
    [SerializeField] private float updateThreshold = 0.1f;
    #endregion

    #region 内部状态
    private BoundsInt effectiveArea;
    private Vector3 previousPosition;
    #endregion

    #region 生命周期
    private RectTransform rectTransform;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero; // 锚点左上角
        rectTransform.anchorMax = Vector2.zero; // 锚点左上角
        rectTransform.pivot = new Vector2(0.5f, 0.5f); // 中心点为图标中心
    }
    private void OnEnable()
    {
        if (mapGenerator != null)
        {
            mapGenerator.OnMapGenerated += RefreshIndicator;
        }
    }

    private void OnDisable()
    {
        if (mapGenerator != null)
        {
            mapGenerator.OnMapGenerated -= RefreshIndicator;
        }
    }
    #endregion

    #region 初始化方法
    public void Initialize()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        if (indicatorImage == null)
        {
            indicatorImage = GetComponent<Image>();
            if (indicatorImage == null)
            {
                Debug.LogError("未找到或未设置玩家标记图像（Image）组件");
                return;
            }
        }

        effectiveArea = mapGenerator.effectiveRect;
        // 强制刷新一次位置
        UpdateIndicatorPosition();
    }
    #endregion

    #region 更新逻辑
    private void Update()
    {
        if (!mapGenerator || !playerTransform) return;

        // 只在位置变化超过阈值时执行
        if (Vector3.Distance(previousPosition, playerTransform.position) > updateThreshold)
        {
            UpdateIndicatorPosition();
            previousPosition = playerTransform.position;
        }
    }
    #endregion

    #region 位置计算
    private void UpdateIndicatorPosition()
    {
        // 获取玩家世界坐标
        Vector3 worldPosition = playerTransform.position;
        // 转换为 Tilemap 坐标
        Vector3Int tilePos = mapGenerator.tilemap.WorldToCell(worldPosition);
        // 限制在有效区域内
        if (IsPointInEffectiveArea(tilePos))
        {
            // 获取瓦片在 effectiveRect 的相对坐标
            int localX = tilePos.x - effectiveArea.xMin;
            int localY = tilePos.y - effectiveArea.yMin;
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
    #endregion

    #region 工具方法
    private bool IsPointInEffectiveArea(Vector3Int position)
    {
        return position.x >= effectiveArea.xMin && 
               position.x < effectiveArea.xMax && 
               position.y >= effectiveArea.yMin && 
               position.y < effectiveArea.yMax;
    }

    private void RefreshIndicator()
    {
        effectiveArea = mapGenerator.effectiveRect;
    }
    #endregion
}
