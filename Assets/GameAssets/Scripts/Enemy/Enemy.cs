using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingThingBase
{
    public void InitEnemy(int level)
    {
        hp = ConfigController.Instance.EnemyDatabase.enemyDatas[level].health;
    }    

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp < 1)
        {
            IsDie = true;
            gameController.RemoveEnemy(this);
            SimplePool.Instance.Despawn(this.gameObject);
        }
    }
}
