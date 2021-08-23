using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	#region Variables

	public float moveSpeed;

	private float xInput;
	private float yInput;
	private float xMove;
	private float yMove;

	private Rigidbody2D myBody;

	
	
	#endregion
	
	
	#region UnityMethods

    void Start() {

		myBody = GetComponent<Rigidbody2D>();
	}

    void Update() {
		PlayerMoveControls();
    }


	void PlayerMoveControls()
	{
		xInput = Input.GetAxisRaw("Horizontal");
		yInput = Input.GetAxisRaw("Vertical");

		xMove = xInput * moveSpeed;
		yMove = yInput * moveSpeed;

		myBody.velocity = new Vector2(xMove, yMove);

	}

    




    #endregion
}
