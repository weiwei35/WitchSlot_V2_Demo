using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public List<Vector2Int> defaultGrid = new List<Vector2Int>();//默认攻击范围
    
    // public GridView_UI gridView_UI;
    public GridView_Map gridView_Map;
    public static Vector2Int playerFaceGridPosCurrent = new Vector2Int(1,0);//角色朝向位置
    public CharacterBase player;
    
    Dictionary<Vector2Int,SymbolSO> symbolDic = new Dictionary<Vector2Int,SymbolSO>();
    
    public bool canAttack = false;
    [Header("广播事件")]
    public DictionaryEventSO SetRandomSymbolEvent;
    public ObjectEventSO EndGridAttackEvent;
    public ObjectEventSO PlayerCallSymbolEvent;

    private void Start()
    {
    }

    public void SetCanAttack()
    {
        canAttack = true;
    }
    public void SetDefaultGrid(object obj)
    {
        defaultGrid.Clear();
        List<Vector2Int> grid = (List<Vector2Int>)obj;
        foreach (var pos in grid)
        {
            defaultGrid.Add(pos);
        }
    }
    
    //计算伤害
    public EnemyGroup enemyGroup;
    List<Vector3> hurtGridPos = new List<Vector3>();
    List<Vector3> allGridPos = new List<Vector3>();
    bool attcked = false;
    public void GridAttack()
    {
        if(!attcked)
        {
            attcked = true;
            StartCoroutine(SetHurtGrid());
        }
    }
    IEnumerator SetHurtGrid()
    {
        //TODO:符文按稀有度排序
        
        hurtGridPos.Clear();
        allGridPos.Clear();
        
        int areaHurt = 0;
        int areaDefence = 0;
        int areaHeal = 0;
        foreach (var grid in gridView_Map.gridObjs)
        {
            foreach (var effect in grid.symbol.effects)
            {
                switch (effect.effectType)
                {
                    case EffectType.伤害:
                        areaHurt+=effect.value;
                        break;
                    case EffectType.护甲:
                        areaDefence+=effect.value;
                        break;
                    case EffectType.治愈:
                        areaHeal+=effect.value;
                        break;
                }
            }
        }

        if (areaDefence > 0)
        {
            player.UpdateDefense(areaDefence);
            yield return new WaitForSeconds(0.5f);
        }

        if (areaHeal > 0)
        {
            player.UpdateHp(areaHeal);
            yield return new WaitForSeconds(0.5f);
        }
        foreach (var grid in gridView_Map.gridObjs.ToList())
        {
            if (grid != null)
            {
                allGridPos.Add(grid.transform.position);
                foreach (Transform enemy in enemyGroup.gameObject.transform)
                {
                    EnemyCommon enemyBase = enemy.GetComponent<EnemyCommon>();
                    Vector3 gridPos = new Vector3(grid.transform.position.x, grid.transform.position.y);
                    if (enemy != null && Vector3.Distance(gridPos, enemy.position) < 0.1f) //判断怪物位置是否有符文
                    {
                        hurtGridPos.Add(gridPos);
                        enemyBase.TakeDamage(areaHurt);
                        if (enemyGroup.enemies.Count == 0) break;
                        yield return new WaitForSeconds(0.5f);
                        if (enemyGroup.enemies.Count == 0) break;
                    }
                }
            }
            if (enemyGroup.enemies.Count == 0) break;
        }
        CountSkillPoint();
        PlayerCallSymbolEvent.RaiseEvent(null, this);
        StartCoroutine(AttackAnimEnd());
        canAttack = false;
    }

    IEnumerator AttackAnimEnd()
    {
        yield return new WaitForSeconds(0.2f);
        
        EndGridAttackEvent.RaiseEvent(null,this);
    }
    //计算法力点:没打出伤害的符文格每个+2点
    public ObjectEventSO AddSkillPointEvent;
    public void CountSkillPoint()
    {
        List<Vector3> pointGridPos = new List<Vector3>();
        foreach (var grid in allGridPos)
        {
            if (!hurtGridPos.Contains(grid))
            {
                pointGridPos.Add(grid);
            }
        }
        float point = pointGridPos.Count * 2;
        AddSkillPointEvent.RaiseEvent(point,this);
        // Debug.Log(pointGridPos.Count);
    }
    //每次攻击区域变换时先清空之前的攻击区域
    public void ClearGrids()
    {
        angleBefore = 0;
        gridView_Map.ClearGrid();
        symbolDic.Clear();
        attcked = false;
    }
    //旋转符文
    private int angleBefore = 0;
    public ObjectEventSO RotateGridUIEvent;
    public void RotateGrid(int angle)
    {
        int angleDiff = ToolFunctions.CalculateClockwiseRotation(angleBefore,angle);
        angleBefore = angle;
        Dictionary<Vector2Int,SymbolSO> newSymbolDic = new Dictionary<Vector2Int,SymbolSO>();
        foreach (var symbol in symbolDic)
        {
            Vector2Int pos = ToolFunctions.RotateGridInt(symbol.Key,angleDiff);
            newSymbolDic.Add(pos, symbol.Value);
        }
        symbolDic = newSymbolDic;
        RotateGridUIEvent.RaiseEvent(angleDiff,this);
        switch (angle)
        {
            case 0:
                playerFaceGridPosCurrent = new Vector2Int(1, 0);
                break;
            case 90:
                playerFaceGridPosCurrent = new Vector2Int(0, -1);
                break;
            case 180:
                playerFaceGridPosCurrent = new Vector2Int(-1, 0);
                break;
            case 270:
                playerFaceGridPosCurrent = new Vector2Int(0, 1);
                break;
        }
        gridView_Map.SetGrid(symbolDic);
        gridView_Map.ShowGrid();
    }
    public void SetSymbolInGrid(object o)
    {
        Dictionary<Vector2Int,SymbolSO> gridPos = (Dictionary<Vector2Int,SymbolSO>)o;
        foreach (var grid in gridPos)
        {
            if (!symbolDic.ContainsKey(grid.Key))
            {
                symbolDic.Add(grid.Key, grid.Value);
            }
        }
        gridView_Map.SetGrid(symbolDic);
        gridView_Map.ShowGrid();
        SetRandomSymbolEvent.RaiseEvent(symbolDic,this);
    }
}
