using UnityEngine;
using Pathfinding;
using System.Collections;

public class GuardPathfinder: MonoBehaviour {

	#region Variables

	[Header("Pathfinding-Variables")]
	// Die Nähe die der Guard zu seinem Ziel-Waypoint haben muss um auf den nächsten Waypoint umzuschlagen.
	public float nextWaypointDistance = 1f;
	public float refreshDelay = 0.2f;
	public float visionRadius = 5f;
	public LayerMask obstructionLayers;

	GameObject foundTarget;

	enum GuardState { SEARCHING, WALKING };
	GuardState guardState;

	// private Variables
	Vector2 direction = Vector2.down;
	float timeLastAttack;
	int currentWaypoint = 0;

	// Components
	Seeker seeker;
	Path path;
	Rigidbody2D rb;
	GuardActions gActions;

	// Flags
	bool activeAttack = false;
	bool found = false;

	#endregion


	#region UnityMethods

	protected void Start() {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		gActions = GetComponent<GuardActions>();

		InvokeRepeating("UpdatePath", 0f, 0.5f);
		timeLastAttack = float.NegativeInfinity;

		
		StartCoroutine(BehaviourRoutine());
	}

   
	
    IEnumerator BehaviourRoutine()
    {
		guardState = GuardState.SEARCHING;

		while (true)
        {
			yield return new WaitForSeconds(refreshDelay);

			if (gActions.target && !gActions.getIsStunned())
            {
				if (guardState == GuardState.SEARCHING)
				{
					Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, visionRadius);
					found = false;
					foreach (Collider2D collided in rangeCheck)
					{
						if (collided.gameObject.transform.parent != null && collided.gameObject.transform.parent.gameObject == gActions.target)
						{
							Vector3 directionToTarget = (collided.gameObject.transform.position - transform.position).normalized;
							float distanceToTarget = Vector3.Distance(transform.position, collided.gameObject.transform.position);

							if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayers))
							{
								
								foundTarget = collided.gameObject;
								guardState = GuardState.WALKING;
								found = true;
							}
							else
							{
								foundTarget = null;
							}
							break;
						}
					}

					if (!found)
					{
						foundTarget = null;
					}

				}
				else if (guardState == GuardState.WALKING)
				{
					Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, visionRadius);
					found = false;
					foreach (Collider2D collided in rangeCheck)
					{
						if (collided.gameObject.transform.parent != null && collided.gameObject.transform.parent.gameObject == gActions.target)
						{
							foundTarget = collided.gameObject;
							found = true;
						}
					} 
					if (!found)
                    {
						foundTarget = null;
						guardState = GuardState.SEARCHING;
                    }
				}
			}
		}
    }
	

	protected void UpdatePath()
    {
        if (seeker.IsDone() && foundTarget && !gActions.getIsStunned())
        {
			seeker.StartPath(rb.position, gActions.target.transform.position, OnPathComplete);
		}
		
	}

	protected void OnPathComplete(Path p)
	{
		if (!p.error)
        {
			path = p;
			currentWaypoint = 0;
        }
	}

    protected void FixedUpdate() {

		if (guardState == GuardState.SEARCHING)
		{
			gActions.GuardSearching();
		}
		else if (guardState == GuardState.WALKING)
		{
			gActions.GuardMoving();
		}


		if (foundTarget && !gActions.getIsStunned())
		{
			if (!activeAttack)
			{
				

				if (path == null)
				{
					return;
				}

				if (currentWaypoint >= path.vectorPath.Count)
				{
					if (Time.time - timeLastAttack > gActions.attackDowntime)
					{
						activeAttack = true;
						gActions.StartAttack();
					}

					return;
				}

				direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

				float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

				rb.MovePosition(rb.position + direction * gActions.moveSpeed * Time.fixedDeltaTime);


				if (distance < nextWaypointDistance)
				{
					currentWaypoint++;
				}
			}
		}
        
    }

	public void EndAttack()
    {
		// flippe Flag um Gegner wieder laufen zu lassen
		activeAttack = false;
		timeLastAttack = Time.time;

	}

	public Vector2 getDirection()
    {
		return direction;
    }

	#endregion
}
