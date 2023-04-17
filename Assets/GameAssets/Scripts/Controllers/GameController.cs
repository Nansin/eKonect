using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : SingletonComponent<GameController>
{
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private GameObject bulletParent;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private LevelController levelController;

    private List<GameObject> listEnemies = new List<GameObject>();
    private LevelData levelData;
    private List<int> listCountSpawn = new List<int>();
    private int wave;

    public List<GameObject> ListEnemies { get => listEnemies; set => listEnemies = value; }
    public GameObject BulletParent { get => bulletParent; set => bulletParent = value; }
    public GameObject EnemyParent { get => enemyParent; set => enemyParent = value; }

    private void Start()
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        levelData = levelController.LevelData;
        wave = 0;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        int numberEnemy = Random.Range(levelData.numberEnemyMin + wave, levelData.numberEnemyMax + wave);
        for (int i = 0; i < numberEnemy; i++)
        {
            int idSpawn = Random.Range(0, levelController.ListsSpawn.Count - 1);
            while (listCountSpawn.Equals(idSpawn))
            {
                idSpawn = Random.Range(0, levelController.ListsSpawn.Count - 1);
            }
            Vector3 pos = levelController.ListsSpawn[idSpawn].position;
            listCountSpawn.Add(idSpawn);
            GameObject enemy = SimplePool.Instance.Spawn(prefabEnemy, pos, Quaternion.identity);
            enemy.transform.SetParent(enemyParent.transform);
            enemy.GetComponent<Enemy>().InitEnemy(wave);
            listEnemies.Add(enemy);
            StaticBatchingUtility.Combine(enemy);
        }

    }

    public void RemoveEnemy(GameObject enemy)
    {
        listEnemies.Remove(enemy);
        if (listEnemies.Count == 0)
        {
            wave++;
            if (wave > levelData.numberWave)
            {
                wave = 0; //continue level
            }
            UIController.Instance.PanelNextWave.SetActive(true);
            DOVirtual.DelayedCall(levelData.timeNextWave, () =>
            {
                SpawnEnemy();
                UIController.Instance.PanelNextWave.SetActive(false);
            });
        }    
    }
}
