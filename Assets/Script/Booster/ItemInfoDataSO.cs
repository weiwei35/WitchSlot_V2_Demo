using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfoData", menuName = "ItemInfo/Data")]
public class ItemInfoDataSO : ScriptableObject 
{
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;
}