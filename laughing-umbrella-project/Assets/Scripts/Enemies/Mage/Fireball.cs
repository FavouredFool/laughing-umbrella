using UnityEngine;

public class Fireball : MonoBehaviour {

	#region Variables
	Vector2 direction;
	float speed;
	int attackDamage;

	Rigidbody2D rb;

	string PLAYER_TAG = "Player";
	#endregion
	
	
	#region UnityMethods

    void Start() {
		rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
		rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
	}

	public void SetValues(Vector2 direction, float speed, int attackDamage)
    {
		this.direction = direction;
		this.speed = speed;
		this.attackDamage = attackDamage;
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
