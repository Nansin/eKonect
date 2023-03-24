using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigController : SingletonComponent<ConfigController>
{
    [SerializeField] private PlayerDatabase playerDatabase;
    [SerializeField] private GunDatabase gunDatabase;
    public PlayerDatabase PlayerDatabase { get => playerDatabase; private set => playerDatabase = value; }
    public GunDatabase GunDatabase { get => gunDatabase; private set => gunDatabase = value; }
}
