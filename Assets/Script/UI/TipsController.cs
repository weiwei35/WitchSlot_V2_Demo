using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class TipsController : MonoBehaviour
{
	public TipsItem tipsObj;
	public TMP_Text tipsName;
	public TMP_Text tipsDesc;
	public void ShowTips(ItemInfoDataSO value,GameObject obj)
	{
		tipsObj.transform.position = obj.transform.position;
		tipsName.text = value.itemName;
		tipsDesc.text = value.itemDesc;
		
		tipsObj.gameObject.SetActive(true);

		SetPosition();
	}
	public void HideTips(ItemInfoDataSO value,GameObject obj)
	{
		tipsObj.gameObject.SetActive(false);
	}

	private void SetPosition()
	{
		float offset = 20;
		float height = tipsObj.GetComponent<RectTransform>().sizeDelta.y;
        if (tipsObj.transform.position.y + height > Screen.height)
        {
        	tipsObj.transform.position = tipsObj.transform.position - new Vector3(0,tipsObj.transform.position.y +
        		height - Screen.height,0)-new Vector3(0,offset,0);
        }
        float width = tipsObj.GetComponent<RectTransform>().sizeDelta.x;
        if (tipsObj.transform.position.x + width > Screen.width)
        {
	        tipsObj.transform.position = tipsObj.transform.position - new Vector3(tipsObj.transform.position.x +
		        width - Screen.width,0,0)-new Vector3(offset,0,0);
        }
	}
}
