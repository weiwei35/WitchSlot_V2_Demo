using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class Booster_002 : Booster_Move
{
    private Vector3 playerPos;
    List<Vector3> hurtPos = new List<Vector3>();
    public override void PlayerMove(object o)
    {
        playerPos = (Vector3)o;
        hurtPos.Clear();
        hurtPos.Add(playerPos+new Vector3(0.8f,0,0));
        hurtPos.Add(playerPos+new Vector3(-0.8f,0,0));
        hurtPos.Add(playerPos+new Vector3(0.8f,0.8f,0));
        hurtPos.Add(playerPos+new Vector3(-0.8f,0.8f,0));
        hurtPos.Add(playerPos+new Vector3(0.8f,-0.8f,0));
        hurtPos.Add(playerPos+new Vector3(-0.8f,-0.8f,0));
        hurtPos.Add(playerPos+new Vector3(0,0.8f,0));
        hurtPos.Add(playerPos+new Vector3(0,-0.8f,0));
        BoosterEffect();
    }

    //移动时，对周围所有格子造成1基础伤害
    public override void BoosterEffect()
    {
        foreach (var pos in hurtPos)
        {
            StartCoroutine(SetEnemyHurt(pos, hurtAmount));
            var effectObj = Instantiate(effect, pos, Quaternion.identity);
        }
    }
}
