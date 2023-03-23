using System;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : MonoBehaviour
{
	[SerializeField]
	private GameObject target;

	private float damage = 0;

	public ParticleSystem muzzleEffect;
	public Transform FirePoint;

	public float Range;
	//public float Damage;
	public float AttackSpeed;
	public float TurnRate = 2f;
	public float RotateAngleIdle;

	//public BulletData BulletData;
	public Transform EndBulletPoint { get; private set; }
	public GameObject Target { get => target; set => target = value; }

	private float fireCountdown = 0f;
	public float HandLength { get; set; }

	public bool HasEnemyInRange { get; set; } = false;
    public float Damage { get => damage; set => damage = value; }

    // Use this for initialization
    void Start()
	{
		FirePoint.localPosition = new Vector3(0, FirePoint.localPosition.y, FirePoint.localPosition.z);
		SetEndRangePosition(Range);
		if (muzzleEffect) muzzleEffect.Stop();
	}

	//public void UpdateTarget(List<Enemy> enemies)
	//{
	//	if (target != null && !enemies.Contains(target))
	//	{
	//		target = null;
	//	}

	//	if (target == null)
	//	{
	//		float shortestDistance = Mathf.Infinity;
	//		Enemy nearestEnemy = null;
	//		foreach (var enemy in enemies)
	//		{
	//			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
	//			if (distanceToEnemy < shortestDistance)
	//			{
	//				shortestDistance = distanceToEnemy;
	//				nearestEnemy = enemy;
	//			}
	//		}

	//		if (nearestEnemy != null && shortestDistance <= Range)
	//		{
	//			target = nearestEnemy;
	//		}
	//		else
	//		{
	//			target = null;
	//		}
	//	}
	//}

	// Update is called once per frame
	void Update()
	{
		if (HasEnemyInRange && fireCountdown <= 0f && AttackSpeed > 0)
		{
			Shoot();
			fireCountdown = 1f / AttackSpeed;
		}

		fireCountdown -= Time.deltaTime;
		if (!HasEnemyInRange)
		{
			fireCountdown = 0;
		}

		// enemy has been death
		if (target && !target.activeSelf)
		{
			target = null;
		}

		if (target == null)
		{
			OnIdle();
			return;
		}

		LockOnTarget();
	}

	void LockOnTarget()
	{
		Vector3 dir = target.transform.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * TurnRate).eulerAngles;
		transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
	}

	private void Shoot()
	{
		//Bullet bullet = PlayerController.Instance.BulletPool.Get();

		//if (bullet != null)
		//{
		//	bullet.transform.position = FirePoint.position;
		//	bullet.transform.rotation = FirePoint.rotation;
		//	bullet.Speed = BulletData.speed;
		//	bullet.Damage = Damage;
		//	bullet.Seek(EndBulletPoint.position);
		//	bullet.ExplodedCallBack = OnExplodeRelease;
		//	if (muzzleEffect)
		//	{
		//		muzzleEffect.transform.position = FirePoint.position;
		//		muzzleEffect.transform.rotation = FirePoint.rotation;
		//		muzzleEffect.Play();
		//	}
		//	bullet.gameObject.SetActive(true);
		//}
	}

	public void SetEndRangePosition(float range)
	{
		Vector3 firePointShadow = new Vector3(transform.position.x, 0, transform.position.z);
		Vector3 mineShadow = new Vector3(FirePoint.position.x, 0, FirePoint.position.z);
		HandLength = Vector3.Distance(firePointShadow, mineShadow);

		var offset = range - HandLength;

		if (EndBulletPoint == null)
		{
			EndBulletPoint = new GameObject("endBulletPoint").transform;
			EndBulletPoint.position = FirePoint.position + FirePoint.forward * offset;
			EndBulletPoint.rotation = FirePoint.rotation;
			EndBulletPoint.parent = FirePoint.parent;
		}
		else
		{
			EndBulletPoint.position = FirePoint.position + FirePoint.forward * offset;
			EndBulletPoint.rotation = FirePoint.rotation;
		}
	}

	private void OnIdle()
	{
		transform.localRotation = Quaternion.Lerp(transform.localRotation,
			Quaternion.Euler(0, RotateAngleIdle, 0), Time.deltaTime * TurnRate / 5);
	}

	private void OnExplodeRelease(Transform bullet)
	{
		//var explodeVfx = PlayerController.Instance?.GetExplodePool()?.Get();
		//if (explodeVfx)
		//{
		//	explodeVfx.transform.position = bullet.transform.position;
		//	explodeVfx.transform.rotation = bullet.transform.rotation;
		//	explodeVfx.Play();
		//	DOVirtual.DelayedCall(explodeVfx.main.duration,
		//		() => PlayerController.Instance.GetExplodePool().Release(explodeVfx));
		//}
	}
}
