using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class GridBtnGroup : MonoBehaviour 
{
	private void Start()
	{
		FightController.instance.endRound += DisableBtn;
		FightController.instance.startRound += ResetBtn;
	}
	public Button attackBtn;
	public Button moveBtn;
	public Button callBtn;
	public Button endBtn;
	public void ResetBtn()
	{
		attackBtn.interactable = true;
		callBtn.interactable = true;
		endBtn.interactable = true;
		moveBtn.interactable = true;
		
		callBtn.gameObject.SetActive(true);
		attackBtn.gameObject.SetActive(false);
		moveBtn.gameObject.SetActive(false);
	}
	private void DisableBtn()
	{
		attackBtn.interactable = false;
		callBtn.interactable = false;
		endBtn.interactable = false;
		moveBtn.interactable = false;
	}

	public ObjectEventSO GridAttackEvent;
	public ObjectEventSO GridMoveEvent;
	public ObjectEventSO GridCallEvent;

	public void SetGridAttack()
	{
		// GridAttackEvent.RaiseEvent(null,this);
	}
	public void SetGridMove()
	{
		// GridMoveEvent.RaiseEvent(null,this);
	}
	public void SetGridCall()
	{
		// GridCallEvent.RaiseEvent(null,this);
		// attackBtn.interactable = false;
	}

	public void ShowGuidMask()
	{
		// StartCoroutine(ShowGuidMaskWait());
	}

	IEnumerator ShowGuidMaskWait()
	{
		yield return new WaitForSeconds(0.2f);
		Vector3 maskCenter = transform.localPosition+callBtn.transform.localPosition;
		RectTransform rect = callBtn.GetComponent<RectTransform>();
		Vector3[] corners = new Vector3[2];
		corners[0] = maskCenter-new Vector3(rect.sizeDelta.x/2,rect.sizeDelta.y/2,0);
		corners[1] = maskCenter+new Vector3(rect.sizeDelta.x/2,rect.sizeDelta.y/2,0);
		ShowUIMask_Tutorial.instance.GuideUI(corners,maskCenter);
	}
}
