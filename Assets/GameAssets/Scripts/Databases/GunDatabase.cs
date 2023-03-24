using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/GunData", order = 2)]
public class GunDatabase : ScriptableObject
{
    public List<GunData> gunDatas;
}

[Serializable]
public class GunData
{
    public float range;
    public int damage;
    public float attackSpeed;
    public float bulletSpeed;
}
