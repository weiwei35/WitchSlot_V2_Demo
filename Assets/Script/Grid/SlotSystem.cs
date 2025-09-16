using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SlotSystem : MonoBehaviour
{
    public static SlotSystem Instance { get; private set; }
    
    [Header("网格配置")]
    [SerializeField] private GameObject slotPrefab;     // 槽位预制体
    [SerializeField] private Vector2 gridSize = new(150f, 150f); // 网格尺寸（与UI元素尺寸匹配）
    [SerializeField] private float snapThreshold = 40f;  // 吸附阈值
    [SerializeField] private bool autoExpand = true;     // 自动扩展开关

    // 内部状态管理
    private HashSet<Vector2Int> occupiedSlots = new(); // 已占用槽位
    private HashSet<Vector2Int> availableSlots = new(); // 可扩展槽位

    private RectTransform canvasRect; // 画布尺寸引用
    private Vector2 centerPos;      // 画布中心锚点

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        canvasRect = GetComponent<RectTransform>();
        centerPos = canvasRect.rect.center;
    }

    private void OnEnable()
    {
        InitializeFirstSlot();
    }

    /// <summary>
    /// 初始化第一个槽位
    /// </summary>
    private Vector2Int? originalAnchor = null;
    public void InitializeFirstSlot()
    {
        var centerGrid = Vector2Int.zero;
        originalAnchor = centerGrid;

        // 初始化槽位状态
        CreateSlot(centerGrid, isOccupied: false); // 初始为可用状态
    }


    /// <summary>
    /// 创建一个新槽位
    /// </summary>
    private void CreateSlot(Vector2Int gridPos, bool isOccupied = false)
    {
        if (occupiedSlots.Contains(gridPos) && isOccupied) return;
        if (availableSlots.Contains(gridPos) && !isOccupied) return;

        RectTransform slot = Instantiate(slotPrefab, transform).GetComponent<RectTransform>();
        slot.anchoredPosition = GridToLocal(gridPos);

        if (slot.TryGetComponent(out Slot s))
        {
            s.isOccupied = isOccupied;
        }

        if (isOccupied)
        {
            occupiedSlots.Add(gridPos);
            availableSlots.Remove(gridPos);
        }
        else
        {
            availableSlots.Add(gridPos);
        }
    }
    /// <summary>
    /// 尝试吸附到有效槽位
    /// </summary>
    public bool TrySnapToValidSlot(DraggableImage draggable, Vector2 screenPos, out Vector2 targetPos)
    {
        targetPos = Vector2.zero;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, screenPos, null, out Vector2 panelPos))
            return false;
        Vector2Int gridPos = LocalToGrid(panelPos);
        Vector2 candidatePos = GridToLocal(gridPos);
        // ✅ 1. 吸附到已占用槽位
        if (occupiedSlots.Contains(gridPos))
        {
            float dist = Vector2.Distance(panelPos, candidatePos);
            if (dist <= snapThreshold)
            {
                targetPos = candidatePos;

                // ✅ 首次触发吸附到初始槽位，才生成扩展槽位
                if (gridPos == (Vector2Int)originalAnchor && autoExpand)
                {
                    GenerateExpansionSlots(gridPos);
                }

                return true;
            }
            return false;
        }

        // 2. 吸附到可扩展槽位
        if (autoExpand && availableSlots.Contains(gridPos))
        {
            targetPos = candidatePos;
            occupiedSlots.Add(gridPos);
            availableSlots.Remove(gridPos);
            // ✅ 标记为已占用时同步更新UI状态
            foreach (var slot in transform.GetComponentsInChildren<RectTransform>())
            {
                if (slot.anchoredPosition == targetPos)
                {
                    if (slot.TryGetComponent<Slot>(out var sl))
                    {
                        sl.isOccupied = true;
                    }
                }
            }
            GenerateExpansionSlots(gridPos); // ✅ 拖放后自动生成新槽位
            return true;
        }
        return false;
    }

    /// <summary>
    /// 生成扩展区域的槽位
    /// </summary>
    public void GenerateExpansionSlots(Vector2Int origin)
    {
        // 仅生成最大 3 层扩展（避免无限扩展）
        if (GetExpansionDepth(origin) >= 3) return;

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (var dir in directions)
        {
            Vector2Int newPos = origin + dir;
            if (!occupiedSlots.Contains(newPos) && !availableSlots.Contains(newPos))
            {
                CreateSlot(newPos, isOccupied: false);
                availableSlots.Add(newPos);
            }
        }

        GetComponent<SetGridArea>()?.EndSymbolSet();
    }


    private int GetExpansionDepth(Vector2Int pos)
    {
        // 可用 BFS 算法实现深度检测
        return 0; // 示例
    }



    /// <summary>
    /// 本地坐标 → 网格坐标
    /// </summary>
    public Vector2Int LocalToGrid(Vector2 localPos)
    {
        int x = Mathf.RoundToInt(localPos.x / gridSize.x);
        int y = Mathf.RoundToInt(localPos.y / gridSize.y);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// 网格坐标 → 本地坐标
    /// </summary>
    public Vector2 GridToLocal(Vector2Int gridPos)
    {
        return new Vector2(gridPos.x * gridSize.x, gridPos.y * gridSize.y);
    }
    
    /// <summary>
    /// 释放占用的槽位并转换为可扩展状态
    /// </summary>
    public void ReleaseSlot(Vector2Int gridPos)
    {
        if (occupiedSlots.Contains(gridPos))
        {
            occupiedSlots.Remove(gridPos);
            availableSlots.Add(gridPos);

            // 更新 Slot 状态
            foreach (var slot in transform.GetComponentsInChildren<RectTransform>())
            {
                if (slot.anchoredPosition == GridToLocal(gridPos))
                {
                    if (slot.TryGetComponent<Slot>(out var s))
                    {
                        s.isOccupied = false;
                    }
                }
            }

            Debug.Log($"🪫 已释放槽位 @ {gridPos}");
        }
    }

    /// <summary>
    /// 获取当前占用槽位
    /// </summary>
    public Vector2Int? GetSlotFor(Vector2 position)
    {
        // 实现查找当前占用
        foreach (var pos in occupiedSlots)
        {
            if (Vector2.Distance(GridToLocal(pos), position) < 1)
            {
                return pos;
            }
        }
        return null;
    }

    /// <summary>
    /// 清理无用的 available 槽位（非相邻）
    /// </summary>
    public void CleanupUnusedSlots()
    {
        if (availableSlots.Count == 1 &&
            occupiedSlots.Count == 0 &&
            originalAnchor != null &&
            availableSlots.Contains((Vector2Int)originalAnchor))
        {
            return; // 防止清空初始槽位
        }

        List<Vector2Int> toRemove = new();

        foreach (var slot in availableSlots)
        {
            bool hasNeighbor = false;
            foreach (var dir in new[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                if (occupiedSlots.Contains(slot + dir))
                {
                    hasNeighbor = true;
                    break;
                }
            }
            if (!hasNeighbor) toRemove.Add(slot);
        }

        foreach (var slot in toRemove)
        {
            availableSlots.Remove(slot);
            DestroyAssociatedGO(slot);
        }
    }

    private void DestroyAssociatedGO(Vector2Int slot)
    {
        foreach (Transform child in transform)
        {
            if (child is RectTransform rt && rt.anchoredPosition == GridToLocal(slot))
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }


    public void RemoveAllSlots()
    {
        foreach (var slot in availableSlots)
        {
            // 查找并销毁对应的 GameObject
            foreach (Transform t in transform)
            {
                if (t is RectTransform rt &&
                    rt.anchoredPosition == GridToLocal(slot))
                {
                    Destroy(t.gameObject);
                    break;
                }
            }
        }
    }
}
