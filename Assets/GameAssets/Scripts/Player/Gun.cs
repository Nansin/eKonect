using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject prefabBullet;
    [SerializeField] private Transform firePoint;
    private float damage;
    private bool hasEnemyInRange;

    private float fireCountdown = 0f;
    private GunData gunData;
    private float gunLenght;
    public bool HasEnemyInRange { get => hasEnemyInRange; set => hasEnemyInRange = value; }
    public float Damage { get => damage; set => damage = value; }
    public Transform EndBulletPoint { get; private set; }
    public float GunLenght { get => gunLenght; set => gunLenght = value; }

    private void Awake()
    {
        InitGun(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetEndRangePosition(gunData.range);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasEnemyInRange && fireCountdown <= 0f && gunData.attackSpeed > 0)
        {
            Shoot();
            fireCountdown = 1f / gunData.attackSpeed;
        }

        fireCountdown -= Time.deltaTime;
        if (!HasEnemyInRange)
        {
            fireCountdown = 0;
        }
    }

    private void InitGun(int playerLevel)
    {
        gunData = ConfigController.Instance.GunDatabase.gunDatas[playerLevel];
    }    
    private void SetEndRangePosition(float range)
    {
        Vector3 firePointShadow = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 mineShadow = new Vector3(firePoint.position.x, 0, firePoint.position.z);
        gunLenght = Vector3.Distance(firePointShadow, mineShadow);

        var offset = range - gunLenght;

        if (EndBulletPoint == null)
        {
            EndBulletPoint = new GameObject("endBulletPoint").transform;
            EndBulletPoint.position = firePoint.position + firePoint.forward * offset;
            EndBulletPoint.rotation = firePoint.rotation;
            EndBulletPoint.parent = firePoint.parent;
        }
        else
        {
            EndBulletPoint.position = firePoint.position + firePoint.forward * offset;
            EndBulletPoint.rotation = firePoint.rotation;
        }
    }
    private void Shoot()
    {
        GameObject bullet = SimplePool.Instance.Spawn(prefabBullet, firePoint.position, firePoint.rotation);

        if (bullet != null)
        {
            bullet.transform.rotation = firePoint.rotation;
            bullet.transform.SetParent(GameController.Instance.BulletParent.transform);
            bullet.GetComponent<Bullet>().InitBullet(gunData.bulletSpeed, gunData.damage, EndBulletPoint.position);
        }
    }
}
