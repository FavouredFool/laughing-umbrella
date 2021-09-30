using UnityEngine;
using Pathfinding;
using System.Collections;

public class OrcPathfinder : MonoBehaviour
{

	#region Variables

	[Header("Pathfinding-Variables")]
	// Die Nähe die der Guard zu seinem Ziel-Waypoint haben muss um auf den nächsten Waypoint umzuschlagen.
	public float nextWaypointDistance = 1f;
	public float refreshDelay = 0.2f;
	public float visionRadius = 5f;
	public LayerMask obstructionLayers;

	GameObject foundTarget;

	enum OrcState { SEARCHING, WALKING };
	OrcState orcState;

	// private Variables
	Vector2 direction = Vector2.down;
	float timeLastAttack;
	int currentWaypoint = 0;

	// Components
	Seeker seeker;
	Path path;
	Rigidbody2D rb;
	OrcActions oActions;

	// Flags
	bool activeAttack = false;
	bool found = false;

	#endregion


	#region UnityMethods

	protected void Start()
	{
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		oActions = GetComponent<OrcActions>();

		InvokeRepeating("UpdatePath", 0f, 0.5f);
		timeLastAttack = float.NegativeInfinity;


		StartCoroutine(BehaviourRoutine());
	}



	IEnumerator BehaviourRoutine()
	{
		orcState = OrcState.SEARCHING;

		while (true)
		{
			yield return new WaitForSeconds(refreshDelay);

			if (oActions.target && !oActions.getIsStunned())
			{
				if (orcState == OrcState.SEARCHING)
				{
					Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, visionRadius);
					found = false;
					foreach (Collider2D collided in rangeCheck)
					{
						if (collided.gameObject.transform.parent != null && collided.gameObject.transform.parent.gameObject == oActions.target)
						{
							Vector3 directionToTarget = (collided.gameObject.transform.position - transform.position).normalized;
							float distanceToTarget = Vector3.Distance(transform.position, collided.gameObject.transform.position);

							if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayers))
							{

								foundTarget = collided.gameObject;
								orcState = OrcState.WALKING;
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
				else if (orcState == OrcState.WALKING)
				{
					Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, visionRadius);
					found = false;
					foreach (Collider2D collided in rangeCheck)
					{
						if (collided.gameObject.transform.parent != null && collided.gameObject.transform.parent.gameObject == oActions.target)
						{
							foundTarget = collided.gameObject;
							found = true;
						}
					}
					if (!found)
					{
						foundTarget = null;
						orcState = OrcState.SEARCHING;
					}
				}
			}
		}
	}


	protected void UpdatePath()
	{
		if (seeker.IsDone() && foundTarget && !oActions.getIsStunned())
		{
			seeker.StartPath(rb.position, oActions.target.transform.position, OnPathComplete);
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

	protected void FixedUpdate()
	{

		if (orcState == OrcState.SEARCHING)
		{
			oActions.OrcSearching();
		}
		else if (orcState == OrcState.WALKING)
		{
			oActions.OrcMoving();
		}


		if (foundTarget && !oActions.getIsStunned())
		{
			if (!activeAttack)
			{


				if (path == null)
				{
					return;
				}

				if (currentWaypoint >= path.vectorPath.Count)
				{
					
					if (Time.time - timeLastAttack > oActions.attackDowntime)
					{
						activeAttack = true;
						oActions.StartAttack();
					}

					return;
				}

				direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

				float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

				rb.MovePosition(rb.position + direction * oActions.moveSpeed * Time.fixedDeltaTime);


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
