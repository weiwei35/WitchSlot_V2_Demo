using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class ShowUIMask_Tutorial : MonoBehaviour 
{
    public static ShowUIMask_Tutorial instance;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Awake()
    {
        instance = this;
    }
    public List<Button> passThroughButtons = new List<Button>();
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            
            foreach (Button btn in passThroughButtons)
            {
                if (btn.gameObject.activeInHierarchy && 
                    RectTransformUtility.RectangleContainsScreenPoint(
                        btn.GetComponent<RectTransform>(), 
                        mousePos, 
                        null))
                {
                    btn.onClick.Invoke();
                }
            }
        }
    }
    
    public Canvas canvas;
    public Image image;
	public Material material; // 材质
    private Camera _camera;

    // 引导
    public void Guide(Vector3[] areaPos, Vector3 centerPosition)
    {
        image.enabled = true;
        // 计算中心点
        Vector2 center = WorldToScreenPoint(canvas, centerPosition);
        // Vector2 center =_camera.WorldToScreenPoint(centerPosition);
        // Debug.Log(center);
        // 计算宽 和 高 
        Vector2 point0 = _camera.WorldToScreenPoint(areaPos[0]);
        Vector2 point1 = _camera.WorldToScreenPoint(areaPos[1]);
        float width = (point1.x - point0.x)/2;
        float height = (point1.y - point0.y)/2;
        // 设置中心点
        material.SetVector("_Center", center);
        material.SetFloat("_SliderX", width);
        material.SetFloat("_SliderY", height);
    }
    public void GuideUI(Vector3[] areaPos, Vector3 centerPosition)
    {
        image.enabled = true;
        // 计算中心点
        Vector2 center = centerPosition;
        // Vector2 center =_camera.WorldToScreenPoint(centerPosition);
        // Debug.Log(center);
        // 计算宽 和 高 
        Vector2 point0 = areaPos[0];
        Vector2 point1 = areaPos[1];
        float width = (point1.x - point0.x)/2;
        float height = (point1.y - point0.y)/2;
        // 设置中心点
        material.SetVector("_Center", center);
        material.SetFloat("_SliderX", width);
        material.SetFloat("_SliderY", height);
    }

    public void HideGuid()
    {
        image.enabled = false;
    }

    public Vector2 WorldToScreenPoint(Canvas canvas, Vector3 world)
    {
        // 把世界坐标转成 屏幕坐标
        Vector2 screenPoint = _camera.WorldToScreenPoint(world);
        Vector2 localPoint;
        // 把屏幕坐标 转成 局部坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out localPoint);
        return localPoint;
    }
}
