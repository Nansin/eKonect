using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public enum PlayerState
{
    Stop,
    Moving,
    MoveAndAttack
}

public class PlayerLevelManager
{
    public int AttackLevel = 0;
    public int MovingLevel = 0;
    public int DamageLevel = 0;
}

public class PlayerController : LivingThingBase
{
    [Header("Gun")]
    [SerializeField] private Gun gun;

    private GameObject target;
    private Vector3 direction = new Vector3();
    [SerializeField] private float movementSpeed = 8f;
    private bool allowMove;
    private PlayerState playerState;

    private PlayerDatabase playerData;
    private bool isDie;

    private float vectorX = 0;
    private float vectorZ = 0;
    private bool isBoundaryX = false;
    private bool isBoundaryZ = false;

    private Rigidbody rigidBody;

    public static PlayerController Instance;

    public float MovementSpeed
    {
        get => movementSpeed;
        set
        {
            movementSpeed = value < playerData.movementSpeed ? playerData.movementSpeed : value;
            Debug.Log("MovementSpeed: " + MovementSpeed);
        }
    }

    public bool AllowMove { get => allowMove; set => allowMove = value; }
    public PlayerState PlayerState { get => playerState; set => playerState = value; }

    public int Hp { get => hp; set => hp = value; }

    #region private method

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerData = ConfigController.Instance.PlayerDatabase;
        hp = (int)playerData.hp;

        MovementSpeed = playerData.movementSpeed; // default

        rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        AllowMove = true;
        //BulletPool = gameController.HandgunBulletPool;
        //var beginLvl = GameController.Instance.GetLevelInfo().PlayerLevelBegin;
        //if (beginLvl > 0)
        //{
        //    if (beginLvl > 4)
        //    {
        //        var valueUpgrade = ConfigController.Instance.DamageDB.GetPlayerDataByLevel(beginLvl).valueUpgrade;
        //        UpgradeDamage(valueUpgrade);
        //    }
        //    else
        //    {
        //        var valueUpgrade = ConfigController.Instance.AttackSpeedDB.GetPlayerDataByLevel(beginLvl).valueUpgrade;
        //        UpgradeAttackSpeed(valueUpgrade);
        //    }

        //    var movementSpeedData = ConfigController.Instance.MovementSpeedDB.GetPlayerDataByLevel(beginLvl);
        //    MovementSpeed = movementSpeedData.valueUpgrade;
        //}
    }

    private void Update()
    {
        if (target && !target.activeSelf)
        {
            target = null;
        }
        FindTarget(!IsKeepTarget());
        LockOnTarget();
    }
    private bool IsKeepTarget()
    {
        if (target != null)
        {
            if (!target.activeSelf || // target has been death
                Vector3.Distance(target.transform.position, transform.position) > playerData.range)
            {
                target = null;
                return false;
            }

            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        if (isBoundaryZ)
        {
            if (vectorZ > 0 && direction.z > 0 || vectorZ < 0 && direction.z < 0)
            {
                direction = new Vector3(direction.x, direction.y, 0);
            }
        }

        if (isBoundaryX)
        {
            if (vectorX > 0 && direction.x > 0 || vectorX < 0 && direction.x < 0)
            {
                direction = new Vector3(0, direction.y, direction.z);
            }
        }

        if (AllowMove)
        {
            rigidBody.AddForce(direction * MovementSpeed, ForceMode.VelocityChange);
        }
    }

    //    private void OnTriggerEnter(Collider other)
    //    {
    //        if (other.tag.Equals(Tags.ENEMY))
    //        {
    //            IsTriggering = true;
    //            TimeResetTrigger = 0;
    //            countTrigger++;
    //        }

    //        // if (other.tag.Equals(Tags.BOUNDARY) || other.tag.Equals(Tags.TURRET))
    //        // {
    //        //     var size = other.GetComponent<BoxCollider>().size;
    //        //     int countBoundary = 0;
    //        //     if (other.transform.position.x + (size.x / 2) + playerCollider.radius / 2 > transform.position.x 
    //        //         && transform.position.x > other.transform.position.x - ((size.x / 2) + playerCollider.radius / 2))
    //        //     {
    //        //         vectorZ = direction.z;
    //        //         isBoundaryZ = true;
    //        //         countBoundary++;
    //        //         objectTrigger.Add(other.name, true);
    //        //     }
    //        //     if (other.transform.position.z + (size.z / 2) + playerCollider.radius / 2 > transform.position.z 
    //        //         && transform.position.z > other.transform.position.z - ((size.z / 2) + playerCollider.radius / 2))
    //        //     {
    //        //         vectorX = direction.x;
    //        //         isBoundaryX = true;
    //        //         countBoundary++;
    //        //         objectTrigger.Add(other.name, false);
    //        //     }
    //        //     if (countBoundary == 0) //default
    //        //     {
    //        //         vectorZ = direction.z;
    //        //         isBoundaryZ = true;
    //        //         objectTrigger.Add(other.name, true);
    //        //     }
    //        // }
    //    }

    //    private void OnTriggerExit(Collider other)
    //    {
    //        // if (other.tag.Equals(Tags.BOUNDARY) || other.tag.Equals(Tags.TURRET))
    //        // {
    //        //     if (objectTrigger[other.name])
    //        //     {
    //        //         vectorZ = 0;
    //        //         isBoundaryZ = false;
    //        //     }
    //        //     else
    //        //     {
    //        //         vectorX = 0;
    //        //         isBoundaryX = false;
    //        //     }
    //        //     objectTrigger.Remove(other.name);
    //        // }

    //        if (other.tag.Equals(Tags.ENEMY))
    //        {
    //            countTrigger--;
    //            if (countTrigger == 0)
    //                IsTriggering = false;
    //        }
    //    }

    //    private void OnTriggerStay(Collider other)
    //    {
    //        if (other.tag.Equals(Tags.ENEMY))
    //        {
    //            if (TimeCooldownHealth < 0)
    //            {
    //                TimeCooldownHealth = GetTimeCooldownHealth();
    //                var enemy = other.GetComponent<Enemy>();
    //                Hp -= enemy.DamageToPlayer;
    //                enemy.DropMoney(other.transform.position);
    //                var pos = transform.position;
    //                pos.y += 5f;
    //                if (Hp < 1)
    //                {
    //                    GameController.Instance.ShowText(pos, "DEAD", null);
    //                    SetPlayerDie();
    //                    CameraController.Instance.SetTarget(null);
    //                    DOVirtual.DelayedCall(GetPlayerDeadTime(), () =>
    //                    {
    //                        GameController.Instance.SetCameraTargetPlayerSpawn();
    //                    });
    //                    return;
    //                }
    //                GameController.Instance.ShowText(pos, "DANGER", null);
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// apply when both hand don't have target
    //    /// </summary>
    private void FindTarget(bool isUpdate)
    {
        if (isDie)
        {
            target = null;
            gun.HasEnemyInRange = false;

            return;
        }

        // continue attack current target
        if (!isUpdate)
        {
            gun.HasEnemyInRange = true;
            return;
        }

        Collider[] listEnemy = Physics.OverlapSphere(transform.position, /*playerData.weaponData.range*/5f, 1 << 7);

        bool canShoot = false;

        if (listEnemy.Length > 0)
        {
            float shortestDistanceLeft = float.PositiveInfinity;
            Vector3 EnemyDir;
            float distanceToEnemy;

            foreach (Collider enemy in listEnemy)
            {
                EnemyDir = transform.InverseTransformPoint(enemy.transform.position);
                distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
                if (EnemyDir.z > 0.0f)
                {
                    canShoot = true;
                    // check left
                    if (isUpdate)
                    {
                        if (distanceToEnemy > gun.GunLenght)
                        {
                            if (distanceToEnemy < shortestDistanceLeft)
                            {
                                target = enemy.gameObject;
                                shortestDistanceLeft = distanceToEnemy;
                            }
                        }
                    }
                }
            }
        }

        if (canShoot)
        {
            gun.HasEnemyInRange = true;
        }
        else
        {
            target = null;
            gun.HasEnemyInRange = false;
        }
    }

    private void LockOnTarget()
    {
        if (target != null)
        {
            Vector3 dir;
            dir = target.transform.position - transform.position;

            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * playerData.rotationSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        //LimitedRoTateLeft();
        //LimitedRoTateRight();
    }

    //    private void SetWeaponData(ref HandGun gun, bool isRotatePositive = true)
    //    {
    //        gun.Range = playerData.weaponData.range;
    //        gun.Damage = FixedGunDamage(playerData.weaponData.bulletData.damage);
    //        gun.BulletData.speed = playerData.weaponData.bulletData.speed;
    //        gun.AttackSpeed = playerData.weaponData.attackSpeed;
    //        gun.TurnRate = playerData.weaponData.turnRate;
    //        gun.RotateAngleIdle = isRotatePositive ? 60 : -60;
    //    }

    //    private void LimitedRoTateLeft()
    //    {
    //        //      ^ + z
    //        //      |
    //        //      |
    //        //      | ---------> + x

    //        var playerRelative = transform.InverseTransformPoint(HandGunLeft.FirePoint.position);

    //        if (playerRelative.z < 0 || playerRelative.x > 0)
    //        {
    //            HandGunLeft.Target = null;
    //        }
    //    }

    //    private void LimitedRoTateRight()
    //    {
    //        //      ^ + z
    //        //      |
    //        //      |
    //        //      | ---------> + x

    //        var playerRelative = transform.InverseTransformPoint(HandGunRight.FirePoint.position);
    //        if (playerRelative.z < 0 || playerRelative.x < 0)
    //        {
    //            HandGunRight.Target = null;
    //        }
    //    }

    //    private void SwapTagert()
    //    {
    //        var temp = HandGunLeft.Target;
    //        HandGunLeft.Target = HandGunRight.Target;
    //        HandGunRight.Target = temp;
    //    }

    //    private void SetTargerEmpty()
    //    {
    //        HandGunLeft.Target = null;
    //        HandGunRight.Target = null;
    //    }

    //    private float FixedGunDamage(float damage)
    //    {
    //        if (Prefs.CurrentLevel > 10)
    //        {
    //            return damage + (Prefs.CurrentLevel - 10);
    //        }

    //        return damage;
    //    }

    //    #endregion private method

    //    #region public method
    public void Move(Vector2 dicrection)
    {
        playerState = PlayerState.Moving;
        if (AllowMove)
        {
            direction = new Vector3(dicrection.x, 0, dicrection.y);
            if (target == null)
            {
                Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = rotation;
            }
        }
    }

    public void Stop()
    {
        playerState = PlayerState.Stop;
        direction = Vector3.zero;
    }
    //    public Transform GetPositionOfLastInventory()
    //    {
    //        Transform newTransform;
    //        Vector3 newPos = inventory.GetNewPosition();
    //        newTransform = SimplePool.Instance.Spawn(moneyPosObj, newPos, Quaternion.identity).transform;
    //        newTransform.transform.SetParent(this.transform);
    //        newTransform.transform.position = newPos;
    //        return newTransform;
    //    }

    //    public void PushMoneyToInventory(Money money)
    //    {
    //        var obj = SimplePool.Instance.Spawn(moneyHaveOutline.gameObject, money.transform.position, money.transform.rotation);
    //        var newMoney = obj.GetComponent<Money>();
    //        newMoney.transform.SetParent(money.transform.parent);
    //        newMoney.transform.localPosition = money.transform.localPosition;
    //        newMoney.transform.localScale = money.transform.localScale;

    //        money.gameObject.SetActive(false);
    //        SimplePool.Instance.Despawn(money.gameObject);

    //        money = newMoney;
    //        money.SetEnableCollider(false);
    //        money.SetStatic(false);

    //        MoneyValue++;
    //        if (MoneyValue > inventory.GridConstraintCount * inventory.SecondaryConstraintCount)
    //        {
    //            // money.SetMeshMoney(false);
    //            SimplePool.Instance.Despawn(money.gameObject);
    //        }
    //        else
    //            inventory.PushLast(money.transform);
    //    }

    //    public void PushMoney(Money money)
    //    {
    //        money.SetStatic(false);
    //        inventory.PushLast(money.transform);
    //    }

    //    public float GetTimeFrezze()
    //    {
    //        return playerData.specialMove.timeFreezeEnemy;
    //    }

    //    public Money PopMoney()
    //    {
    //        if (MoneyValue > 0)
    //        {
    //            MoneyValue--;
    //            Transform moneyObject = inventory.PopLast();
    //            if (moneyObject == null)
    //            {
    //                return null;
    //            }

    //            return moneyObject.GetComponent<Money>();
    //        }
    //#if UNITY_EDITOR
    //        Debug.LogError("[Game] Money <= 0! Check logic!!");
    //#endif
    //        return null;
    //    }

    //    public float GetTimeCooldownHealth()
    //    {
    //        return playerData.timeCooldown;
    //    }

    //    public float GetResurrectionTime()
    //    {
    //        return playerData.resurrectionTime;
    //    }

    //    public float GetPlayerDeadTime()
    //    {
    //        return playerData.playerDeadTime;
    //    }

    //    public float GetCameraTargetTime()
    //    {
    //        return playerData.cameraTargetTime;
    //    }

    //    public float GetRadiatingSkillTime()
    //    {
    //        return playerData.specialMove.radiatingSkillTime;
    //    }

    //    public float GetSkillVFXExistTime()
    //    {
    //        return playerData.specialMove.vFXExistTime;
    //    }

    //    public void SetPlayerDie()
    //    {
    //        this.isDie = true;
    //        SetTargerEmpty();
    //        transform.position = gameController.GetLevelInfo().GetPlayerDie().position;
    //    }

    //    public void SetPlayerSpawn()
    //    {
    //        //Particle run
    //        this.isDie = false;
    //        SetTargerEmpty();
    //        hp = (int)playerData.hp;
    //        transform.position = gameController.GetLevelInfo().GetPlayerSpawn().position;
    //        upgradeCompleteEffect?.Play();
    //    }

    //    public void UpgradeAttackSpeed(float attackSpeed)
    //    {
    //        HandGunLeft.AttackSpeed = attackSpeed;
    //        HandGunRight.AttackSpeed = attackSpeed;
    //        Debug.Log("AttackSpeed: " + attackSpeed);
    //    }

    //    public void UpgradeDamage(float damage)
    //    {
    //        damage = FixedGunDamage(damage);
    //        HandGunLeft.Damage = damage;
    //        HandGunRight.Damage = damage;
    //        Debug.Log("Damage: " + damage);
    //    }

    //    public void UpdatePos(Vector3 targetPos)
    //    {
    //        transform.position = targetPos;
    //    }

    //    public void OnPlayUpgradeVfx()
    //    {
    //        if (upgradeCompleteEffect)
    //        {
    //            upgradeCompleteEffect.Play();
    //        }
    //    }

    //    public ObjectPool<ParticleSystem> GetExplodePool()
    //    {
    //        return GameController.Instance.BulletExplodeVfxPool;
    //    }
    #endregion public method
}