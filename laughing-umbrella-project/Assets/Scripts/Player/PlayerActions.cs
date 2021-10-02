using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerActions : MonoBehaviour {

	#region Variables

	[Header("Player-Variables")]
	public float moveSpeed;
	public int maxHealth;
	public float invincibleTime = 1f;
	public Color invinciblityColor = Color.white;
	public float dashDistance = 2;
	public float dashTime = 0.05f;
	public float invincibleTimeAfterDash = 0.2f;
	public float stunDuration = 0.3f;


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
	bool isStunned = false;

	#endregion


	#region UnityMethods

	protected void Start() {

		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
		currentHealth = maxHealth;


		if(MainScript.health != 0)
        {
			currentHealth = MainScript.health;
		}
		

	}

    protected void Update() {

		PlayerMoveControls();

		if (Input.GetKeyDown(KeyCode.Space) && dashCount > 0 && movement != Vector2.zero && !isStunned)
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
		StartCoroutine(BecomeInvincible(invincibleTimeAfterDash));
	}


	protected void PlayerMoveControls()
	{
		if (!isDashing && !isStunned)
		{
			movement.x = Input.GetAxisRaw("Horizontal");
			movement.y = Input.GetAxisRaw("Vertical");
			movement.Normalize();
		} else
        {
			movement = Vector2.zero;
        }
			
		if (movement != Vector2.zero)
		{
			animator.SetFloat("horizontal", movement.x);
			animator.SetFloat("vertical", movement.y);
		}
		animator.SetFloat("speed", movement.sqrMagnitude);
		
	}

    protected void FixedUpdate()
    {
		if (!isDashing && !isStunned)
        {
			//movement
			rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
		} 
    }

	

	public void getDamaged(int attackDamage, Vector2 knockbackDirection, float knockbackStrength)
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
				StartCoroutine(ToggleStun());
				CreateKnockback(knockbackDirection, knockbackStrength);
				StartCoroutine(BecomeInvincible(invincibleTime));
            }
		}
    }

	IEnumerator ToggleStun()
    {
		isStunned = true;
		Color backupColor = sr.color;
		sr.color = invinciblityColor;
		yield return new WaitForSeconds(stunDuration);
		sr.color = backupColor;
		isStunned = false;
    }

	void CreateKnockback(Vector2 knockbackDirection, float knockbackStrength)
    {
		rb.velocity = knockbackDirection * knockbackStrength;
    }

	IEnumerator BecomeInvincible(float invincibleTime)
    {
		isInvincible = true;
		yield return new WaitForSeconds(invincibleTime);

		isInvincible = false;
	}

	protected void getDestroyed()
    {
		Destroy(gameObject);
		SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
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

	public bool getIsStunned()
    {
		return isStunned;
    }
    #endregion
}
