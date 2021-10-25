using UnityEngine;
using Pathfinding;
using System.Collections;

public class PathfinderForMelee : MonoBehaviour
{

	#region Variables

	[Header("Pathfinding-Variables")]
	// Die Nähe die der Guard zu seinem Ziel-Waypoint haben muss um auf den nächsten Waypoint umzuschlagen.
	public float nextWaypointDistance = 1f;
	public float refreshDelay = 0.2f;
	public float visionRadius = 5f;
	public float attackDistance = 1.25f;
	public LayerMask obstructionLayers;

	GameObject foundTarget;

	enum AttackerState { SEARCHING, WALKING };
	AttackerState attackerState = AttackerState.SEARCHING;

	// private Variables
	Vector2 direction = Vector2.down;
	float timeLastAttack;
	int currentWaypoint = 0;

	// Components
	Seeker seeker;
	Path path;
	Rigidbody2D rb;
	Enemy enemyActions;
	IMeleeAttackerActions meleeActions;

	// Flags
	bool activeAttack = false;
	bool found = false;

	#endregion


	#region UnityMethods

	protected void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		enemyActions = GetComponent<Enemy>();
		meleeActions = GetComponent<IMeleeAttackerActions>();

		InvokeRepeating("UpdatePath", 0f, refreshDelay);
		timeLastAttack = float.NegativeInfinity;
	}



	protected void UpdatePath()
	{
		if (seeker.IsDone() && foundTarget && !enemyActions.getIsStunned())
		{
			seeker.StartPath(rb.position, enemyActions.target.transform.position, OnPathComplete);
		} 

		if (enemyActions.target && !enemyActions.getIsStunned())
		{
			if (attackerState == AttackerState.SEARCHING)
			{
				Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, visionRadius);
				found = false;
				foreach (Collider2D collided in rangeCheck)
				{
					if (collided.gameObject.transform.parent != null && collided.gameObject.transform.parent.gameObject == enemyActions.target)
					{
						Vector3 directionToTarget = (collided.gameObject.transform.position - transform.position).normalized;
						float distanceToTarget = Vector3.Distance(transform.position, collided.gameObject.transform.position);

						if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayers))
						{

							foundTarget = collided.gameObject;
							attackerState = AttackerState.WALKING;
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
			else if (attackerState == AttackerState.WALKING)
			{
				Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, visionRadius);
				found = false;
				foreach (Collider2D collided in rangeCheck)
				{
					if (collided.gameObject.transform.parent != null && collided.gameObject.transform.parent.gameObject == enemyActions.target)
					{
						foundTarget = collided.gameObject;
						found = true;
					}
				}
				if (!found)
				{
					foundTarget = null;
					attackerState = AttackerState.SEARCHING;
				}
			}
		}
	}

	protected void OnPathComplete(Path p)
	{
		if (!p.error)
		{
			path = p;
			currentWaypoint = 0;
		} else
        {
			Debug.Log("error");
        }
	}

	protected void FixedUpdate()
	{

		if (attackerState == AttackerState.SEARCHING)
		{
			meleeActions.AttackerSearching();
		}
		else if (attackerState == AttackerState.WALKING)
		{
			meleeActions.AttackerMoving();
		}


		if (foundTarget && !enemyActions.getIsStunned())
		{
			if (!activeAttack)
			{

				if (path == null)
				{
					return;
				}

				if (currentWaypoint >= path.vectorPath.Count)
				{
					return;
				}

				// Attack Range
				float distanceToTarget = Vector2.Distance(transform.position, foundTarget.transform.position);
				if (Time.time - timeLastAttack > meleeActions.GetAttackDowntime() && distanceToTarget <= attackDistance)
				{
					activeAttack = true;
					meleeActions.StartAttack();
				}

				float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
				if (distance < nextWaypointDistance)
				{
					currentWaypoint++;
				}

				Vector2 tempDirection = (new Vector2(path.vectorPath[currentWaypoint].x, path.vectorPath[currentWaypoint].y) - rb.position).normalized;

				if (tempDirection != Vector2.zero){
					direction = tempDirection;
                }

				rb.MovePosition(rb.position + direction * enemyActions.moveSpeed * Time.fixedDeltaTime);
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
