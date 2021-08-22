using UnityEngine;

public class CharMovement : MonoBehaviour {

	#region Variables
	[SerializeField]
	private float movementSpeed = 10f;

	[SerializeField]
	private float jumpForce = 5f;

	private float xMove;

	private Rigidbody2D myRigidbody;

    #endregion


    #region UnityMethods

    private void Awake()
    {
		myRigidbody = GetComponent<Rigidbody2D>();
		Debug.Log("start");
	}
    void Start() {

		

	}

    void Update() {

		PlayerMoveKeyboard();
		

	}

	void PlayerMoveKeyboard()
    {
		xMove = Input.GetAxisRaw("Horizontal");
		
		transform.position += new Vector3(xMove, 0f, 0f) * movementSpeed * Time.deltaTime;

	}
	
	#endregion
}
