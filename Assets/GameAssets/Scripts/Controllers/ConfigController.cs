using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigController : SingletonComponent<ConfigController>
{
    [SerializeField] private PlayerDatabase playerDatabase;
    [SerializeField] private GunDatabase gunDatabase;
    [SerializeField] private EnemyDatabase enemyDatabase;
    [SerializeField] private LevelDatabase levelDatabase;
    public PlayerDatabase PlayerDatabase { get => playerDatabase; private set => playerDatabase = value; }
    public GunDatabase GunDatabase { get => gunDatabase; private set => gunDatabase = value; }
    public EnemyDatabase EnemyDatabase { get => enemyDatabase; set => enemyDatabase = value; }
    public LevelDatabase LevelDatabase { get => levelDatabase; set => levelDatabase = value; }
}
