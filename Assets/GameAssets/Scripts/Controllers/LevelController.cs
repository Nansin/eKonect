using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoints;
    [SerializeField] private int level;

    private LevelData levelData;
    private List<Transform> listsSpawn = new List<Transform>();

    public List<Transform> ListsSpawn { get => listsSpawn; private set => listsSpawn = value; }
    public LevelData LevelData { get => levelData; set => levelData = value; }

    private void Awake()
    {
        for (int i = 0; i < spawnPoints.transform.childCount; i++)
        {
            listsSpawn.Add(spawnPoints.transform.GetChild(i));
        }

        levelData = ConfigController.Instance.LevelDatabase.levelDatas[level];
    }
}
