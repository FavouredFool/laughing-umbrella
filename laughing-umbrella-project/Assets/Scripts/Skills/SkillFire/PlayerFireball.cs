using UnityEngine;

public class PlayerFireball : MonoBehaviour
{

	#region Variables

	// private Variablen
	Vector2 direction;
	float speed;
	int attackDamage;
	float knockbackStrength;

	// Componenten
	Rigidbody2D rb;
	CircleCollider2D cCollider;
	Animator animator;

	// Konstanten
	readonly string ENEMY_TAG = "Enemy";

	// Flags
	bool isActive = false;
	
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

	public void SetValues(Vector2 direction, float speed, int attackDamage, float knockbackStrength)
	{
		this.direction = direction;
		this.speed = speed;
		this.attackDamage = attackDamage;
		this.knockbackStrength = knockbackStrength;

		animator.SetTrigger("grow");
	}

	public void startShooting()
	{
		animator.SetTrigger("fly");
		isActive = true;
	}

    protected void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.transform.parent.tag.Equals(ENEMY_TAG))
		{
			Vector2 knockbackDirection = direction.normalized;
			collision.transform.parent.GetComponent<Enemy>().getDamaged(attackDamage, knockbackDirection, knockbackStrength);
		}
	}

    protected void OnCollisionEnter2D(Collision2D collision)
    {
		Destroy(gameObject);
	}

    #endregion
}
