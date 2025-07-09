using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using NaughtyAttributes;
using Random = UnityEngine.Random;

public class GridController : MonoBehaviour
{
    public List<Vector2Int> defaultGrid = new List<Vector2Int>();//默认攻击范围
    public List<Vector2Int> changedGrid = new List<Vector2Int>();//变换攻击范围
    
    // public GridView_UI gridView_UI;
    private GridView_Map gridView_Map;
    private GridMove gridMove;
    public static Vector2Int playerFaceGridPosCurrent = new Vector2Int(1,0);//角色朝向位置
	private bool attacked = false;
    
    public CharacterBase player;
    
    Dictionary<Vector2Int,SymbolSO> symbolDic = new Dictionary<Vector2Int,SymbolSO>();
    [Header("广播事件")]
    public ObjectEventSO EndStepEvent;
    public DictionaryEventSO RotateGridEvent;
    public DictionaryEventSO SetRandomSymbolEvent;

    private void Start()
    {
        gridView_Map = GetComponent<GridView_Map>();
        gridMove = GetComponent<GridMove>();
        
        FightController.instance.endStep += ClearGrid;
        FightController.instance.endRound += ClearGrid;
        symbolDeck = GameObject.FindWithTag("SymbolDeck").GetComponent<SymbolDeck>();
    }

    public void SetDefaultGrid(object obj)
    {
        defaultGrid.Clear();
        changedGrid.Clear();
        List<Vector2Int> grid = (List<Vector2Int>)obj;
        foreach (var pos in grid)
        {
            defaultGrid.Add(pos);
            changedGrid.Add(pos);
        }
    }

    public void SetChangedGrid(object obj)
    {
        changedGrid.Clear();
        List<Vector2Int> grid = (List<Vector2Int>)obj;
        foreach (var pos in grid)
        {
            changedGrid.Add(pos);
        }
    }
    
    //计算伤害
    public EnemyGroup enemyGroup;
    List<Vector3> hurtGridPos = new List<Vector3>();
    List<Vector3> allGridPos = new List<Vector3>();
    public void GridAttack()
    {
        StartCoroutine(SetHurtGrid());
    }
    IEnumerator SetHurtGrid()
    {
        //TODO:符文按稀有度排序
        
        hurtGridPos.Clear();
        allGridPos.Clear();
        foreach (var grid in gridView_Map.gridObjs)
        {
            allGridPos.Add(grid.transform.position);
            foreach (Transform enemy in enemyGroup.gameObject.transform)
            {
                CharacterBase enemyBase = enemy.GetComponent<CharacterBase>();
                if (grid.symbol.symbolAttacks.Count > 0)
                {
                    foreach (var gridExtra in grid.symbol.symbolAttacks)
                    {
                        Vector3 gridExtraPos = new Vector3(grid.transform.position.x+gridExtra.position.x*0.8f, grid.transform.position.y+gridExtra.position.y*0.8f);
                        if (enemy != null && Vector3.Distance(gridExtraPos, enemy.position) < 0.1f) //判断怪物位置是否有符文
                        {
                            hurtGridPos.Add(gridExtraPos);
                            foreach (var effect in grid.symbol.effects)
                            {
                                effect.ApplyEffect(player,enemyBase);
                                if (enemyGroup.enemiesInFight.Count == 0) yield break;
                                yield return new WaitForSeconds(0.5f);
                            }
                        }
                        if (enemyGroup.enemiesInFight.Count == 0) yield break;
                    }
                }
                else
                {
                    Vector3 gridPos = new Vector3(grid.transform.position.x, grid.transform.position.y);
                    if (enemy != null && Vector3.Distance(gridPos, enemy.position) < 0.1f) //判断怪物位置是否有符文
                    {
                        hurtGridPos.Add(gridPos);
                        foreach (var effect in grid.symbol.effects)
                        {
                            effect.ApplyEffect(player,enemyBase);
                            if (enemyGroup.enemiesInFight.Count == 0) yield break;
                            yield return new WaitForSeconds(0.5f);
                        }
                        if (enemyGroup.enemiesInFight.Count == 0) yield break;
                    }
                }
            }
            symbolDeck.DiscardSymbol(grid.symbol);
            if (enemyGroup.enemiesInFight.Count == 0) yield break;
        }

        CountSkillPoint();
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
    public void ClearGrid()
    {
        attacked = false;
        // gridView_UI.ClearGrid();
        EndStepEvent.RaiseEvent(null,this);
        symbolDic.Clear();
        foreach (var grid in gridView_Map.gridObjs)
        {
            Destroy(grid.gameObject);
        }
        gridView_Map.gridObjs.Clear();
        gridView_Map.SetHurt();
        gridMove.savedDirection = Direction.Right;
        SetChangedGrid(defaultGrid);
    }

    Dictionary<Vector2Int, SymbolSO> oldSymbolDic = new Dictionary<Vector2Int,SymbolSO>();

    public void SaveGridRotate()
    {
        oldSymbolDic.Clear();
        foreach (var value in symbolDic)
        {
            oldSymbolDic.Add(value.Key,value.Value);
        }
    }
    public void RotateGrid(int angle)
    {
        Dictionary<Vector2Int, SymbolSO> newSymbolDic = new Dictionary<Vector2Int,SymbolSO>();
        List<Vector2Int> newPos = new List<Vector2Int>();
        foreach (var grid in oldSymbolDic)
        {
            Vector2Int pos = ToolFunctions.RotateGridInt(grid.Key,angle);
            newPos.Add(pos);
            newSymbolDic.Add(pos, grid.Value);
        }
        symbolDic.Clear();
        symbolDic = newSymbolDic;
        SetChangedGrid(newPos);
        // gridView_UI.SetGridSymbol(symbolDic);
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
        RotateGridEvent.RaiseEvent(symbolDic,this);

        gridView_Map.SetGrid(symbolDic);
        gridView_Map.ShowGrid();
    }
    
    //随机符文
    private List<SymbolSO> symbolListCurrent = new List<SymbolSO>();
    private List<float> symbolRandomRank = new List<float>();
    private SymbolDeck symbolDeck;

    List<SymbolSO> resultSymbols = new List<SymbolSO>();
    public void SetSymbol(int count)//外部方法
    {
        if (!attacked)
        {
            var symbolList = symbolDeck.GetSymbolList(count);
            symbolListCurrent.Clear();
            symbolRandomRank.Clear();
            foreach (var symbol in symbolList)
            {
                symbolListCurrent.Add(symbol);
                symbolRandomRank.Add(symbol.randomRank);
            }
            SetRandomSymbol(count);
            attacked = true;
        }
    }
    private void SetRandomSymbol(int count)
    {
        resultSymbols.Clear();
        resultSymbols = WeightedRandomWithoutRepeats<SymbolSO>(
            symbolListCurrent,
            symbolRandomRank,
            (int)MathF.Min(count, symbolListCurrent.Count));
        List<Vector2Int> points = new List<Vector2Int>();
        foreach (var grid in changedGrid)
        {
            // if(grid != playerPos)
                points.Add(grid);
        }
        SetSymbolPos(points, resultSymbols);
        // gridView_UI.SetGridSymbol(symbolDic);
        SetRandomSymbolEvent.RaiseEvent(symbolDic,this);
    }
    //随机位置
    private void SetSymbolPos(List<Vector2Int> points,List<SymbolSO> symbols)
    {
        symbolDic.Clear();
        List<Vector2Int> newPoints = points.ToList();
        foreach (var symbol in symbols)
        {
            int index = Random.Range(0, newPoints.Count);
            symbolDic.Add(newPoints[index], symbol);
            newPoints.RemoveAt(index);
        }

        gridView_Map.SetGrid(symbolDic);
        gridView_Map.ShowGrid();
        SaveGridRotate();
        // gridView_Map.SetHurt(symbolDic);
    }
    /// <summary>
    /// 优化的权重随机不重复选择算法（适合游戏抽卡、战利品掉落等场景）
    /// </summary>
    /// <param name="elements">候选元素集合</param>
    /// <param name="weights">权重数组（建议使用相对权重）</param>
    /// <param name="n">选取数量</param>
    /// <param name="zeroWeightRemoval">是否自动移除零权重元素</param>
    /// <param name="seed">随机种子（用于可重复测试）</param>
    private static List<SymbolSO> WeightedRandomWithoutRepeats<T>(
        List<SymbolSO> elements,
        List<float> weights,
        int n,
        bool zeroWeightRemoval = true,
        int? seed = null)
    {
        // 参数校验
        if (elements == null || weights == null || elements.Count != weights.Count)
        {
            Debug.LogError("元素与权重数量不匹配");
            return new List<SymbolSO>();
        }

        // 初始化随机种子
        Random.State originalState = Random.state;
        if (seed.HasValue) Random.InitState(seed.Value);

        // 数值过滤（处理游戏设计中常见的零权重情况）
        List<(SymbolSO Element, float Weight)> candidates = elements
            .Select((item, index) => (Element: item, Weight: weights[index]))
            .Where(x => !zeroWeightRemoval || x.Weight > 0) // 现在可以正确访问Weight字段
            .ToList();

        List<SymbolSO> results = new List<SymbolSO>();
        int maxAttempts = Mathf.Min(n, candidates.Count);

        // 预计算总权重优化
        float totalWeight = candidates.Sum(x => x.Weight);
        bool needRescale = totalWeight > float.MaxValue * 0.9f; // 处理超大权重值

        for (int i = 0; i < maxAttempts; i++)
        {
            // 权重重新缩放（解决超大数值导致的浮点精度问题）
            if (needRescale)
            {
                float maxWeight = candidates.Max(x => x.Weight);
                float scaleFactor = float.MaxValue / maxWeight * 0.9f;
                candidates = candidates.Select(x => (x.Element, x.Weight * scaleFactor)).ToList();
                totalWeight *= scaleFactor;
                needRescale = false;
            }

            // 权重耗尽时终止
            if (totalWeight <= float.Epsilon) break;

            // 生成随机落点（使用左闭右开区间）
            float randomValue = Random.Range(0f, totalWeight);
            float cumulative = 0f;
            int selectedIndex = -1;

            // 选择循环（后续可优化为二分查找）
            for (int j = 0; j < candidates.Count; j++)
            {
                if (candidates[j].Weight <= 0) continue;

                cumulative += candidates[j].Weight;
                if (cumulative >= randomValue)
                {
                    selectedIndex = j;
                    break;
                }
            }

            // 默认保底（选择第一个有效元素）
            if (selectedIndex == -1)
            {
                selectedIndex = candidates.FindIndex(x => x.Weight > 0);
                if (selectedIndex == -1) break;
            }

            // 记录结果并更新权重
            results.Add(candidates[selectedIndex].Element);
            totalWeight -= candidates[selectedIndex].Weight;

            // 从候选中移除已选元素（保持数组顺序）
            candidates.RemoveAt(selectedIndex);
        }

        // 恢复随机状态（不影响全局随机性）
        Random.state = originalState;
        return results;
    }
}
