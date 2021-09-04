using UnityEngine;
using System;

public class PlayerActions : MonoBehaviour {

	#region Variables

	[Header("Player-Variables")]
	public float moveSpeed;
	public int maxHealth;

	// private Variables
	int currentHealth;
	Vector2 movement;

	// Components
	private Rigidbody2D myBody;
	public Animator animator;

	#endregion


	#region UnityMethods

	protected void Start() {

		myBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		currentHealth = maxHealth;

	}

    protected void Update() {

		PlayerMoveControls();
    }


	protected void PlayerMoveControls()
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

    protected void FixedUpdate()
    {
		//movement in FixedUpdate()
		myBody.MovePosition(myBody.position + movement * moveSpeed * Time.fixedDeltaTime);
        
    }

	public void getDamaged(int attackDamage)
    {
		Debug.Log(attackDamage + " Schaden");
		currentHealth -= attackDamage;
		if (currentHealth <= 0)
        {
			getDestroyed();
        }
    }

	protected void getDestroyed()
    {
		Destroy(gameObject);
    }

	public int getMaxHealth()
    {
		return maxHealth;
    }

	public int getCurrentHealth()
    {
		return currentHealth;
    }

    #endregion
}
