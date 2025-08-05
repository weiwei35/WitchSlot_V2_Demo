using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo_UI : MonoBehaviour
{
	public Player player;
	public Image playerIcon;
	public TMP_Text playerName;
	public IntVariable playerHP;

	private void OnEnable()
	{
		RectTransform rectTransform = GetComponent<RectTransform>();
		rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
	}

	public void Init()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		playerIcon.sprite = player.playerIcon;
		playerName.text = player.playerName;
		SetHpBar(playerHP.currentValue);
	}

	public MMProgressBar hpBar;
	public void SetHpBar(int amount)
	{
		hpBar.UpdateBar(amount, 0f, playerHP.maxValue);
	}

	public GameObject defenceState;
	public TMP_Text defenceAmount;
	public void SetDefense(int amount)
	{
		if (amount > 0)
		{
			defenceState.SetActive(true);
			defenceAmount.text = amount.ToString();
		}
		else
		{
			defenceState.SetActive(false);
		}
	}
}
