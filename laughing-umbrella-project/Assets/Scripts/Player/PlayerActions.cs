using UnityEngine;
using System;
using System.Collections;

public class PlayerActions : MonoBehaviour {

	#region Variables

	[Header("Player-Variables")]
	public float moveSpeed;
	public int maxHealth;
	public float invincibleTime = 1f;
	public Color invinciblityColor = Color.white;
	public float dashDistance = 2;
	public float dashTime = 0.05f;
	public float invincibleAfterDash = 0.2f;


	public GameObject playerCollision;

	// private Variables
	int currentHealth;
	Vector2 movement;
	int dashCount = 0;

	// Components
	private Rigidbody2D rb;
	private Animator animator;
	private SpriteRenderer sr;

	// Flags
	bool isDashing = false;
	bool isInvincible = false;

	#endregion


	#region UnityMethods

	protected void Start() {

		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		currentHealth = maxHealth;

	}

    protected void Update() {

		PlayerMoveControls();

		if (Input.GetKeyDown(KeyCode.Space) && dashCount > 0)
		{
			StartCoroutine(Dash());
		}

	}

	IEnumerator Dash()
    {
        isDashing = true;
		dashCount -= 1;
		playerCollision.SetActive(false);
        rb.velocity = Vector3.zero;
		rb.AddForce(dashDistance / dashTime * movement, ForceMode2D.Impulse);
		yield return new WaitForSeconds(dashTime);
		isDashing = false;
		playerCollision.SetActive(true);
		StartCoroutine(BecomeInvincible(invincibleAfterDash, false));
	}


	protected void PlayerMoveControls()
	{
		if(!isDashing)
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
	}

    protected void FixedUpdate()
    {

		if (!isDashing)
        {
			//movement
			rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
		} 
    }

	

	public void getDamaged(int attackDamage)
    {
		// Check ob Schaden genommen werden sollte
		if(!isInvincible)
        {
			// Invincible-Time überschritten
			currentHealth -= attackDamage;
			if (currentHealth <= 0)
			{
				getDestroyed();
			} else
            {
				StartCoroutine(BecomeInvincible(invincibleTime, true));
            }
		}
    }

	IEnumerator BecomeInvincible(float invincibleTime, bool colorCoded)
    {
		isInvincible = true;
		Color tempColor = sr.color;

		if (colorCoded)
        {
			sr.color = invinciblityColor;
		}
		
		
		yield return new WaitForSeconds(invincibleTime - invincibleTime / 8);
		
		if (colorCoded)
        {
			sr.color = tempColor;
		}
		

		// nachdem Farbe weg ist, gibt es noch eine kurze Unsichtbarkeitszeit
		yield return new WaitForSeconds(invincibleTime / 8);

		isInvincible = false;
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

	public int getDashCount()
    {
		return dashCount;
    }

	public void setDashCount(int dashCount)
    {
		this.dashCount = dashCount;
    }
    #endregion
}
