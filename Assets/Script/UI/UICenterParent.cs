using UnityEngine;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 根据子节点长宽推断位置中心
/// </summary>
public class UICenterParent : MonoBehaviour
{
    float width,height;
    private float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
    private void GetChildArea()
    {
        minX = float.MaxValue;
        minY = float.MaxValue;
        maxX = float.MinValue;
        maxY = float.MinValue;
        foreach (Transform grid in transform)
        {
            Vector3 pos = grid.localPosition;
            if(pos.x < minX) minX = pos.x;
            if(pos.y < minY) minY = pos.y;
            if(pos.x > maxX) maxX = pos.x;
            if(pos.y > maxY) maxY = pos.y;
        }
        
        width = maxX - minX;
        height = maxY - minY;
    }

    public void SetCenter()
    {
        GetChildArea();
        float centerX = minX + width / 2;
        float centerY = minY + height / 2;
        // Debug.LogError(centerX +" "+ centerY);
        GetComponent<RectTransform>().sizeDelta = new Vector2(width+100, height+100);
        transform.localPosition = -new Vector3(centerX,centerY,0);
    }
}
