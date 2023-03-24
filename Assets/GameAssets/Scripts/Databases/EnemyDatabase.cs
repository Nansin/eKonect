using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/EnemyData", order = 3)]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemyDatas;
}

[Serializable]
public class EnemyData
{
    public int health;
}
