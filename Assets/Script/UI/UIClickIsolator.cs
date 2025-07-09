using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIClickIsolator : MonoBehaviour, ICanvasRaycastFilter
{
    [Header("2D射线设置")]
    public LayerMask raycastLayers = -1;   // 要检测的2D物体层级
    public bool requireCollider = true;    // 需要精确碰撞器
    private Vector2 worldPoint;
    private Camera screenCamera;
    
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        // 获取屏幕坐标对应的世界坐标
        screenCamera = eventCamera != null ? eventCamera : Camera.main;
        worldPoint = screenCamera.ScreenToWorldPoint(screenPoint);
        
        // 2D光线投射检测碰撞体
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, 0, raycastLayers);
        
        // 如果检测到碰撞体，则穿透点击（返回false）
        return requireCollider ? !(hit.collider != null) : !(hit.transform != null);
    }

}