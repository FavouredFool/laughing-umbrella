using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

	#region Variables

	public float moveSpeed;
	Vector2 movement;

	private Rigidbody2D myBody;
	public Animator animator;
	
	#endregion
	
	
	#region UnityMethods

    void Start() {

		myBody = GetComponent<Rigidbody2D>();

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
		Debug.Log("aua");
    }

    #endregion
}
