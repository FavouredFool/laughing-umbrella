using UnityEngine;
using Pathfinding;

public class GuardPathfinder: MonoBehaviour {

	#region Variables

	[Header("Pathfinding-Variables")]
	// Die Nähe die der Guard zu seinem Ziel-Waypoint haben muss um auf den nächsten Waypoint umzuschlagen.
	public float nextWaypointDistance = 1f;

	// private Variables
	Vector2 direction;
	float timeLastAttack;
	int currentWaypoint = 0;

	// Components
	Seeker seeker;
	Path path;
	Rigidbody2D rb;
	GuardActions gActions;

	// Flags
	bool activeAttack = false;

	#endregion


	#region UnityMethods

	protected void Start() {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		gActions = GetComponent<GuardActions>();

		InvokeRepeating("UpdatePath", 0f, 0.5f);
		timeLastAttack = float.NegativeInfinity;

    }

	protected void UpdatePath()
    {
        if (seeker.IsDone() && gActions.target)
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

		if (gActions.target)
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
