using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "weapon",menuName = "Weapon/data")]
public class WeaponSO : ItemInfoDataSO
{
    public int coldTime;
    public WeaponType type;
    public List<Vector2Int> hurtArea;
}
