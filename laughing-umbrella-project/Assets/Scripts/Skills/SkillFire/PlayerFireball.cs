using UnityEngine;

public class PlayerFireball : MonoBehaviour
{

	#region Variables

	public GameObject explosion;

	// private Variablen
	Vector2 direction;
	float speed;
	int attackDamage;
	float knockbackStrength;
	float explosionRange;

	// Componenten
	Rigidbody2D rb;
	CircleCollider2D cCollider;
	Animator animator;

	// Konstanten
	readonly string ENEMY_TAG = "Enemy";
	readonly string BOSS_TAG = "Boss";

	// Flags
	bool isActive = false;
	bool alreadyExploded = false;
	
	#endregion


	#region UnityMethods

	protected void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		cCollider = GetComponent<CircleCollider2D>();
	}

	protected void Update()
	{
		if (isActive)
		{
			rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
		}

	}

	public void SetValues(Vector2 direction, float speed, int attackDamage, float knockbackStrength, float explosionRange)
	{
		this.direction = direction;
		this.speed = speed;
		this.attackDamage = attackDamage;
		this.knockbackStrength = knockbackStrength;
		this.explosionRange = explosionRange;

		animator.SetTrigger("grow");
	}

	public void startShooting()
	{
		animator.SetTrigger("fly");
		isActive = true;
	}

    protected void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.transform.parent.tag.Equals(ENEMY_TAG) && !alreadyExploded)
		{
			//Vector2 knockbackDirection = direction.normalized;
			//collision.transform.parent.GetComponent<Enemy>().getDamaged(attackDamage, knockbackDirection, knockbackStrength);
			Explosion();
		}
		else if (collision.transform.parent != null && collision.transform.parent.parent != null && collision.transform.parent.tag.Equals(BOSS_TAG))
		{

			Explosion();
		}

	}

	
    protected void OnCollisionEnter2D(Collision2D collision)
    {
		if (!alreadyExploded)
			Explosion();
	}
	

	protected void Explosion()
    {
		alreadyExploded = true;
		GameObject explosionInstance = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);

		explosionInstance.GetComponent<Explosion>().SetValues(attackDamage, knockbackStrength, explosionRange);

		Destroy(gameObject);
    }


	#endregion
}
