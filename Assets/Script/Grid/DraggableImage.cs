using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DraggableImage : MonoBehaviour, 
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("状态控制")]
    [Range(0.1f, 1f)]
    [SerializeField] private float smoothTime = 0.2f; //返回动画时间
    
    private RectTransform rectTransform;
    private Image image;
    private Canvas canvas;
    private Vector2 initialPos;
    private bool isSnapped = false;

    public Vector2Int symbolPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        // 初始位置记录
        initialPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(SlotSystem.Instance.transform.parent);
        image.raycastTarget = false;
        // 👇 通知 SlotSystem 释放当前占用槽位
        var currentPos = rectTransform.anchoredPosition;
        if (SlotSystem.Instance.GetSlotFor(currentPos) is Vector2Int slotPos)
        {
            SlotSystem.Instance.ReleaseSlot(slotPos);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isSnapped) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            eventData.position,
            null,
            out Vector2 localPoint);
        rectTransform.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 尝试吸附
        if (SlotSystem.Instance.TrySnapToValidSlot(this, eventData.position, out Vector2 targetPos))
        {
            StartCoroutine(SmoothMove(rectTransform, rectTransform.anchoredPosition, targetPos, smoothTime));
            SlotSystem.Instance.CleanupUnusedSlots(); // 吸附后清空无用槽
            symbolPos = new Vector2Int((int)targetPos.x/120, (int)targetPos.y/120);
        }
        else
        {
            StartCoroutine(SmoothReturn());
            Debug.LogWarning($"❌ 吸附失败: 离最近槽位过远 @ {rectTransform.anchoredPosition}");
        }
    }


    /// <summary>
    /// 平滑返回原位动画
    /// </summary>
    private IEnumerator SmoothReturn()
    {
        float t = 0;
        Vector2 startPos = rectTransform.anchoredPosition;
        
        while (t < 1)
        {
            t += Time.deltaTime / smoothTime;
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, initialPos, t);
            yield return null;
        }
        
        rectTransform.anchoredPosition = initialPos;
        image.raycastTarget = true;
    }

    /// <summary>
    /// 平滑移动动画
    /// </summary>
    private IEnumerator SmoothMove(RectTransform target, Vector2 from, Vector2 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.anchoredPosition = Vector2.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        target.anchoredPosition = to;
    }
}
