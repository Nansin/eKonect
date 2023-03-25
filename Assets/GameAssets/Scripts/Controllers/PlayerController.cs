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

public class PlayerController : LivingThingBase
{
    [Header("Gun")]
    [SerializeField] private Gun gun;

    private GameObject target;
    private Vector3 direction = new Vector3();
    private float movementSpeed;
    private bool allowMove;
    private PlayerState playerState;

    private PlayerDatabase playerData;

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
    public Gun Gun { get => gun; set => gun = value; }


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

    private void FindTarget(bool isUpdate)
    {
        if (IsDie)
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
    }
    #endregion private method

    #region public method
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
    #endregion public method
}