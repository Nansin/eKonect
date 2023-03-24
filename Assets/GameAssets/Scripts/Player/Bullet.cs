using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	private Rigidbody rigid;

	private Vector3 endpoint;

	private float speed;
	private int damage;

	public void InitBullet(float speed, int damage, Vector3 endpoint)
    {
		this.speed = speed;
		this.damage = damage;
		this.endpoint = endpoint;
    }

	// Update is called once per frame
	protected virtual void Update ()
	{
		if (transform.InverseTransformPoint(endpoint).z > 0)
		{
			rigid.AddForce((endpoint - transform.position).normalized * speed, ForceMode.VelocityChange);
		}
		else
		{
			rigid.velocity = Vector3.zero;
			SimplePool.Instance.Despawn(this.gameObject);
		}
	}

	public virtual void Explode ()
	{
		rigid.velocity = Vector3.zero;
		SimplePool.Instance.Despawn(this.gameObject);
	}

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(Tags.ENEMY))
		{
			Enemy e = other.transform.GetComponent<Enemy>();
			if (e != null)
			{
				e.TakeDamage(damage);
			}

			Explode();
		}
	}
}
