using UnityEngine;

public class Fireball : MonoBehaviour {

	#region Variables
	Vector2 direction;
	float speed;
	int attackDamage;

	Rigidbody2D rb;

	string PLAYER_TAG = "Player";

	bool isActive = false;

	Animator animator;
	#endregion
	
	
	#region UnityMethods

    void Awake() {
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
    }

    void Update() {
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

		// Rotate
		float angle = Vector2.SignedAngle(Vector2.up, direction) + 180;
		gameObject.transform.rotation = Quaternion.Euler(0, 0, angle);

		animator.SetTrigger("grow");
    }



	public void startShooting()
    {
		animator.SetTrigger("fly");
		isActive = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.tag.Equals(PLAYER_TAG)) {
			collision.gameObject.GetComponent<PlayerActions>().getDamaged(attackDamage);
        }
		Destroy(gameObject);
    }

    #endregion
}
