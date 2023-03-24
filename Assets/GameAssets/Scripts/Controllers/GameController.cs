using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : SingletonComponent<GameController>
{
    [SerializeField] private GameObject bulletParent;
    [SerializeField] private GameObject enemyParent;
    private List<Enemy> listEnemies = new List<Enemy>();

    public List<Enemy> ListEnemies { get => listEnemies; set => listEnemies = value; }
    public GameObject BulletParent { get => bulletParent; set => bulletParent = value; }
    public GameObject EnemyParent { get => enemyParent; set => enemyParent = value; }

}
