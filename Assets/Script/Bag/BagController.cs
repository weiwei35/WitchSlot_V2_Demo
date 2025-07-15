using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;
/// <summary>
/// 槽位1：药水位|槽位2：备用装备
/// 各有2个格子，获得更多物品时需要进行替换
/// </summary>
public class BagController : MonoBehaviour
{
    public WeaponGroup weaponGroup;
    public WeaponInBag_UI weaponItem;
    public List<GameObject> weaponGrids;
    public List<WeaponInBag_UI> weaponInBag;
    public void AddWeapon(WeaponSO weapon)
    {
        if (weaponInBag.Count < 2)
        {
            //添加物品
            var weaponObj = 
                Instantiate(weaponItem, weaponGrids[weaponInBag.Count].transform);
            weaponObj.weapon = weapon;
            weaponObj.group = weaponGroup;
            weaponObj.Init();
            weaponInBag.Add(weaponObj);
        }
        else
        {
            //需要替换
        }
    }

    public void RemoveWeapon(WeaponSO weapon)
    {
        foreach (var obj in weaponInBag.ToList())
        {
            if (obj.weapon == weapon)
            {
                weaponInBag.Remove(obj);
                Destroy(obj.gameObject);
            }
        }
    }
}
