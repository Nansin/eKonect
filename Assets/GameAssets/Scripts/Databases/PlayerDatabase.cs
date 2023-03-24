using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Database/PlayerData", order = 1)]
public class PlayerDatabase : ScriptableObject
{
    public float hp;
    public float range;
    public float movementSpeed;
    public float rotationSpeed;
}
