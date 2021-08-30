using UnityEngine;
using System;

public class PlayerActions : MonoBehaviour {

	#region Variables

	public float moveSpeed;
	public int maxHealth;
	private int health;
	Vector2 movement;

	private Rigidbody2D myBody;
	public Animator animator;

	string ENEMY_TAG = "Enemy";

	#endregion


	#region UnityMethods

	void Start() {

		myBody = GetComponent<Rigidbody2D>();
		health = maxHealth;

	}

    void Update() {
		//input in Update()
		PlayerMoveControls();
    }


	void PlayerMoveControls()
	{
		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");
		movement.Normalize();
		if (movement != Vector2.zero)
        {
			animator.SetFloat("horizontal", movement.x);
			animator.SetFloat("vertical", movement.y);
		}
		animator.SetFloat("speed", movement.sqrMagnitude);
	}

    void FixedUpdate()
    {
		//movement in FixedUpdate()
		myBody.MovePosition(myBody.position + movement * moveSpeed * Time.fixedDeltaTime);
        
    }

	public void getDamaged(int attackDamage)
    {
		Debug.Log(attackDamage + " Schaden");
		health -= attackDamage;
		if (health <= 0)
        {
			getDestroyed();
        }
    }

	public void getDestroyed()
    {
		Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
		
		if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag.Equals(ENEMY_TAG))
			getDamaged(collision.gameObject.transform.parent.GetComponent<Enemy>().attackDamage);
    }

    #endregion
}
