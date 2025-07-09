using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfo_UI : MonoBehaviour
{
	public Player player;
	public Image playerIcon;
	public TMP_Text playerName;
	public IntVariable playerHP;
	public TMP_Text HP;

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
	}
	
	private void Update()
	{
		if (player != null)
		{
			HP.text = playerHP.currentValue.ToString();
			SetHP();
		}
	}

	public Slider HPSlider;
	private void SetHP()
	{
		if (playerHP != null) HPSlider.value = (float)playerHP.currentValue / playerHP.maxValue;
	}
	public EnemySkillList SkillList;
	private GameObject skill;
	public Button openButton;
	bool isOpenDetail = false;
	GameObject skillDetail;
	public void OpenPlayerDetail()
	{
		if (!isOpenDetail)
		{
			isOpenDetail = true;
			var skillList = Instantiate(SkillList,transform.parent);
            skillList.transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
            skillList.InitSkillList(player.skill);
            skillDetail = skillList.gameObject;
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
            skill = skillList.gameObject;
            openButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "A";
            
            OpenBoosterDetail();
		}
		else
		{
			isOpenDetail = false;
			Destroy(skillDetail);
			LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
			openButton.transform.GetChild(0).GetComponent<TMP_Text>().text = "V";
		}
	}

	public BoosterList boosterList;
	private void OpenBoosterDetail()
	{
		var booster = Instantiate(boosterList,skillDetail.transform);
		booster.Init();
		LayoutRebuilder.ForceRebuildLayoutImmediate(booster.transform.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(skillDetail.transform.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
		LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
	}
}
