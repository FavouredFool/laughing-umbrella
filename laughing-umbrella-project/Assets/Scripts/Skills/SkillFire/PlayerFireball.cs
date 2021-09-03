using UnityEngine;

public class PlayerFireball : MonoBehaviour
{

	#region Variables
	Vector2 direction;
	float speed;
	int attackDamage;

	Rigidbody2D rb;
	CircleCollider2D cCollider;

	string ENEMY_TAG = "Enemy";

	bool isActive = false;

	Animator animator;
	#endregion


	#region UnityMethods

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		cCollider = GetComponent<CircleCollider2D>();
	}

	void Update()
	{
		if (isActive)
		{
			rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
		}

	}

	public void SetValues(Vector2 direction, float speed, int attackDamage)
	{
		this.direction = direction;
		this.speed = speed;
		this.attackDamage = attackDamage;

		animator.SetTrigger("grow");
	}



	public void startShooting()
	{
		animator.SetTrigger("fly");
		isActive = true;
	}



    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.gameObject.tag.Equals(ENEMY_TAG))
		{
			collision.transform.parent.GetComponent<Enemy>().getDamaged(attackDamage);
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		Destroy(gameObject);
	}

    #endregion
}
