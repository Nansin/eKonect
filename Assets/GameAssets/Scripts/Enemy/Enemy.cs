using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : LivingThingBase
{

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp < 1)
        {
            gameController.ListEnemies.Remove(this);
            SimplePool.Instance.Despawn(this.gameObject);
        }
    }
}
