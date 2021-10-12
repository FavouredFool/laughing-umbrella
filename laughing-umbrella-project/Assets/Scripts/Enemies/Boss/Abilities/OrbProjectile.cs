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
	#endregion
	
	
	#region UnityMethods

    void Start() {
		rb = GetComponent<Rigidbody2D>();
		startTime = Time.time;
    }

    void Update() {
		
		if (Time.time - startTime > orbDespawnTime)
        {
			Destroy(gameObject);
        }

		rb.MovePosition(rb.position + direction * orbSpeed * Time.fixedDeltaTime);
	}

	public void SetValues(float orbSpeed, Vector2 direction, float orbDespawnTime, int damage, float knockbackStrength)
    {
		this.orbSpeed = orbSpeed;
		this.direction = direction;
		this.orbDespawnTime = orbDespawnTime;
		this.damage = damage;
		this.knockbackStrength = knockbackStrength;
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
