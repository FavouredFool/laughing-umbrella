using UnityEngine;

public class GuardActions : Enemy, IMeleeAttackerActions {

    #region Variables

	[Header("Guard-Specific Variables")]
	// Weite des Slash-Angriffs.
    public float slashWidth = 2f;
	// Länge des Slash-Angriffs.
    public float slashLength = 3f;
	// Entfernung zur Wache bei der die Hitbox beginnt (unten Gizmos entkommentieren um die Hitbox besser zu sehen). 
    public float attackStartDistance = 0.7f;
	// Pause zwischen Angriffen in Sek.
	public float attackDowntime = 3f;

	public float attackDuration = 0.2f;

	public float totalAngle = 90f;

	// Für Gizmos
	Vector3 rotatedPointNear;
    Vector3 rotatedPointFar;

	Vector3 endpointRotated;

	Vector2 direction;
	float angle;

	// Components
	Animator animator;
	PathfinderForMelee pathfinder;

	// Flags
	bool createHitboxFlag = false;
	bool attackActive = false;

	float attackStarttime = float.NegativeInfinity;
	
	#endregion


	#region UnityMethods

	new protected void Start()
    {
		// Start von "Enemy" aufrufen
		base.Start();

        animator = GetComponent<Animator>();
		pathfinder = GetComponent<PathfinderForMelee>();

		direction = pathfinder.getDirection();

		animator.SetBool("searching", true);
	}

    protected void Update()
    {
		// Animationen setzen

		if (!isStunned)
		{

			if (direction != Vector2.zero)
			{
				animator.SetFloat("horizontal", direction.x);
				animator.SetFloat("vertical", direction.y);
			}


			// Vorübergehend für Gizmos (sonst wird's erst bei StartAttack() berechnet).
			// Lege Hitbox aus, teste auf Treffer in jeweilige Richtung. Wird aufgerufen aus Animation
			// Direction checken

			direction = pathfinder.getDirection();

			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				// rechts und links
				if (direction.x > 0)
					angle = 270f;

				else
					angle = 90f;
			}
			else
			{
				// oben und unten
				if (direction.y > 0)
					angle = 0f;
				else
					angle = 180f;
			}


			if (attackActive)
            {


				float angleInterpolated = (((Time.time - attackStarttime) / attackDuration) * totalAngle) - totalAngle/2;
				endpointRotated = RotatePointAroundPivot(transform.position + Vector3.up * slashLength, gameObject.transform.position, new Vector3(0, 0, angle - angleInterpolated));

				RaycastHit2D[] attackHits = Physics2D.LinecastAll(transform.position, endpointRotated);

				foreach (RaycastHit2D rayHit in attackHits)
				{
					Collider2D hit = rayHit.collider;
					if (hit.gameObject.transform.parent != null && hit.gameObject.transform.parent.gameObject == target)
					{
						Vector2 knockbackDirection = hit.gameObject.transform.parent.position - gameObject.transform.position;
						hit.gameObject.transform.parent.GetComponent<PlayerActions>().getDamaged(attackDamage, knockbackDirection, knockbackStrength);
						break;
					}
				}


				if (Time.time - attackStarttime > attackDuration)
				{
					attackActive = false;
				}

			}
		
		}
	}

	public void StartAttack()
	{
		// Starte Animation -> Blendtree für Richtung

		FindObjectOfType<AudioManager>().Play("SwordSlash");

		createHitboxFlag = true;
		animator.SetTrigger("attack");
	}


	public void CreateHitbox()
    {
		if (createHitboxFlag)
        {

			attackActive = true;
			attackStarttime = Time.time;

			createHitboxFlag = false;
        }
    }

	public void EndAttack()
	{
		// Sage dem Pathfinder, dass er wieder laufen darf
		pathfinder.EndAttack();

	}

	protected Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 dir = point - pivot;
		dir = Quaternion.Euler(angles) * dir;
		point = dir + pivot;
		return point;
	}
	
	public void AttackerMoving()
    {
		animator.SetBool("searching", false);
    }

	public void AttackerSearching()
    {
		animator.SetBool("searching", true);
    }

	public float GetAttackDowntime()
    {
		return attackDowntime;
    }
	


	
	/*
	void OnDrawGizmos()
	{

		Gizmos.DrawWireSphere(endpointRotated, 0.5f);

	}
	*/



    #endregion
}
