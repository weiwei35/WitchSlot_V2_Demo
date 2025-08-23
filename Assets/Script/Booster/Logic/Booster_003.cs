using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Booster_003 : Booster_Move
{
    private Vector3 playerPos;
    public Booster_003_Grid hurtGrid;
    public override void PlayerMove(object o)
    {
        playerPos = (Vector3)o;
        BoosterEffect();
    }

    //角色移动后，留下可以对路过敌人造成1基础伤害的地表，持续两回合
    public override void BoosterEffect()
    {
        var grid = Instantiate(hurtGrid, playerPos, Quaternion.identity);
        grid.count = 2;
        grid.hurt = hurtAmount;
        grid.effect = effect;
    }
}
