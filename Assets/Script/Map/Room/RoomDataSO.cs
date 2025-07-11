using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "Room/Data")]
public class RoomDataSO : ScriptableObject 
{
    public RoomType roomType;
    public SymbolType symbolType;
    public float randomCount;
    public int roomLevel;

    public Sprite roomIcon;
    public GameObject roomPrefab;

    public List<EnemyData> enemies;
}

[System.Serializable]
public struct EnemyData
{
    public EnemyCommon enemy;
    public float enemyHP;
}
