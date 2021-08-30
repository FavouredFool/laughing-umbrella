using UnityEngine;

public class GuardActions : Enemy {

    #region Variables
    // Konstanten
    
    public float SLASHWIDTH;
    public float SLASHLENGTH;
    public float ATTACKSTARTDISTANCE = 0.7f;

	public Orb enemyOrb;

    public LayerMask playerLayers;

    Vector3 rotatedPointNear;
    Vector3 rotatedPointFar;

    Animator animator;
	GuardPathfinder pathfinder;

	Vector2 direction;
	bool createHitboxFlag = false;
    #endregion


    #region UnityMethods

    private void Start()
    {
        animator = GetComponent<Animator>();
		pathfinder = GetComponent<GuardPathfinder>();

		direction = pathfinder.getDirection();

		// Für Lebensleiste
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}

    private void Update()
    {

		// Animationen setzen
		
		if (direction != Vector2.zero)
		{
			animator.SetFloat("horizontal", direction.x);
			animator.SetFloat("vertical", direction.y);
		}
		

		// Vorübergehend für Gizmos

		// Lege Hitbox aus, teste auf Treffer in jeweilige Richtung. Wird aufgerufen aus Animation

		// Direction checken

		direction = pathfinder.getDirection();
		float angle;
		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
		{
			// rechts und links
			if (direction.x > 0)
			{
				// rechts
				angle = -90f;

			}
			else
			{
				// links
				angle = 90f;

			}
		}
		else
		{
			// oben und unten
			if (direction.y > 0)
			{
				// oben
				angle = 0f;
			}
			else
			{
				// unten
				angle = 180f;
			}
		}

		// Hitbox aufbauen
		// Kollisionspunkt des Rechtecks vorne rechts berechnen
		Vector3 pointNear = new Vector3(gameObject.transform.position.x + SLASHWIDTH / 2, gameObject.transform.position.y + ATTACKSTARTDISTANCE / 2, gameObject.transform.position.z);
		rotatedPointNear = RotatePointAroundPivot(pointNear, gameObject.transform.position, new Vector3(0, 0, angle));

		// Kollisionspunkt des Rechtecks hinten links berechnen
		Vector3 pointFar = new Vector3(gameObject.transform.position.x - SLASHWIDTH / 2, gameObject.transform.position.y + SLASHLENGTH, gameObject.transform.position.z);
		rotatedPointFar = RotatePointAroundPivot(pointFar, gameObject.transform.position, new Vector3(0, 0, angle));

	}

	public void StartAttack()
	{
		// Starte Animation -> Blendtree für Richtung
		createHitboxFlag = true;
		animator.SetTrigger("attack");
		

	}

	void CreateHitbox()
	{
		if (createHitboxFlag)
        {
			// Gegner bei Slash detecten
			Collider2D playerHit = Physics2D.OverlapArea(rotatedPointNear, rotatedPointFar, playerLayers);

			if (playerHit != null)
			{
				playerHit.gameObject.GetComponent<PlayerActions>().getDamaged(attackDamage);
			}

			createHitboxFlag = false;
		}
		

	}

	void EndAttack()
	{
		// Sage dem Pathfinder, dass er wieder laufen darf
		pathfinder.EndAttack();

	}

	Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 dir = point - pivot;
		dir = Quaternion.Euler(angles) * dir;
		point = dir + pivot;
		return point;
	}


	
	/*
	void OnDrawGizmos()
	{

		Gizmos.DrawWireSphere(rotatedPointNear, 0.5f);
		Gizmos.DrawWireSphere(rotatedPointFar, 0.5f);

	}*/

	protected override void dropOrb()
    {
		Instantiate(enemyOrb, gameObject.GetComponent<OrbSpawn>().GetOrbSpawnPos(), Quaternion.identity);
    }


	public override void getDestroyed()
    {
        // Destroy Object
        Destroy(gameObject);
    }

    #endregion
}
