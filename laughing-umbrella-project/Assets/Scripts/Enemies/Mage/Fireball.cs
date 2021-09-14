using UnityEngine;

public class Fireball : MonoBehaviour {

	#region Variables

	// Variablen
	Vector2 direction;
	float speed;
	int attackDamage;
	float knockbackStrength;

	// Konstanten
	readonly string PLAYER_TAG = "Player";

	// Flags
	bool isActive = false;

	// Konstruktoren
	Animator animator;
	Rigidbody2D rb;

	#endregion


	#region UnityMethods

	protected void Awake() {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
    }

    protected void Update() {
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.tag.Equals(PLAYER_TAG)) {
			Vector2 knockbackDirection = direction;
			collision.gameObject.GetComponent<PlayerActions>().getDamaged(attackDamage, knockbackDirection, knockbackStrength);
        }
		Destroy(gameObject);
    }

    #endregion
}
