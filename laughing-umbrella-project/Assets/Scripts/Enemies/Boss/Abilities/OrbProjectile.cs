using UnityEngine;

public class OrbProjectile : MonoBehaviour {

	#region Variables
	Vector2 direction;
	float orbSpeed;
	float orbDespawnTime;
	float startTime;
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

	public void SetValues(float orbSpeed, Vector2 direction, float orbDespawnTime)
    {
		this.orbSpeed = orbSpeed;
		this.direction = direction;
		this.orbDespawnTime = orbDespawnTime;
    }



	
	#endregion
}
