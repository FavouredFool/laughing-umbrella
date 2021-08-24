using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour {

	#region Variables

	public float moveSpeed;

	private float xInput;
	private float yInput;
	private float xMove;
	private float yMove;

	private Rigidbody2D myBody;

	public enum Direction { FRONT, BACK, LEFT, RIGHT };
	private Direction playerDirection;

	public Animator animator;
	
	#endregion
	
	
	#region UnityMethods

    void Start() {

		myBody = GetComponent<Rigidbody2D>();
		playerDirection = Direction.FRONT;

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

		if (xMove != 0 || yMove != 0)
        {
			playerDirection = determineDirection(xMove, yMove);
		}
		

	}


	Direction determineDirection(float xMove, float yMove)
    {
		if (Math.Abs(xMove) > Math.Abs(yMove))
        {
			// Läuft rechts/links
			if(xMove <= 0)
            {
				animator.SetInteger("direction", 2);
				return Direction.LEFT;
			}
			else
            {
				animator.SetInteger("direction", 3);
				return Direction.RIGHT;
			}
        }
		else
        {
			// Läuft vorne/hinten
			if(yMove <= 0)
            {
				animator.SetInteger("direction", 0);
				Debug.Log("Front");
				return Direction.FRONT;
            }
			else
            {
				animator.SetInteger("direction", 1);
				Debug.Log("back");
				return Direction.BACK;
            }
        }
    }

    




    #endregion
}
