using UnityEngine;

public class OrbProjectile : MonoBehaviour {

	#region Variables
	public LayerMask wallLayer;
	Vector2 direction;
	float orbSpeed;
	float orbDespawnTime;
	float startTime;
	int damage;
	float knockbackStrength;
	Rigidbody2D rb;
	SpriteRenderer sr;
	CircleCollider2D collider;

	bool startUp;

	float orbSpawnTime;
	float t;
	#endregion


	#region UnityMethods

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		collider = GetComponent<CircleCollider2D>();
		collider.enabled = false;
		sr.color = new Color(1, 1, 1, 0);

		startTime = Time.time;
    }

    void Update() {
		
		if (Time.time - startTime > orbSpawnTime)
        {
			collider.enabled = true;

			rb.MovePosition(rb.position + direction * orbSpeed * Time.fixedDeltaTime);
		} else
        {
			t = (Time.time - startTime) / orbSpawnTime;
			sr.color = new Color(1, 1, 1, t);
        }
		
	}

	public void SetValues(float orbSpeed, Vector2 direction, float orbDespawnTime, int damage, float knockbackStrength, float orbSpawnTime)
    {
		this.orbSpeed = orbSpeed;
		this.direction = direction;
		this.orbDespawnTime = orbDespawnTime;
		this.damage = damage;
		this.knockbackStrength = knockbackStrength;
		this.orbSpawnTime = orbSpawnTime;

	}

	public void OnTriggerEnter2D(Collider2D collision)
    {
		if (wallLayer == (wallLayer | (1 << collision.gameObject.layer)))
        {
			Destroy(gameObject);
        }
    }

	public Vector2 GetDirection()
    {
		return direction;
    }

	public int GetDamage()
    {
		return damage;
    }

	public float GetKnockbackStrength()
    {
		return knockbackStrength;
    }



	
	#endregion
}
