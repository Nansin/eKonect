using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/LevelData", order = 4)]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> levelDatas;
}

[Serializable]
public class LevelData
{
    public int numberWave;
    public float timeNextWave;
    public int numberEnemyMin;
    public int numberEnemyMax;
}
