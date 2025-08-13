using UnityEngine;

public class GridObj : MonoBehaviour 
{
	public SymbolSO symbol;
	public Vector2 gridPos;

	public GameObject effect;

	public void SetAnim()
	{
		Animation anim = GetComponent<Animation>();
		anim.Play("格子生效特效");
	}

	public void SetEffect()
	{
		var o = Instantiate(effect,transform);
	}
}
