using UnityEngine;
using Pathfinding;

public class GuardPathfinder: MonoBehaviour {

	#region Variables
	public float nextWaypointDistance = 1f;

	private Vector2 direction;
	bool activeAttack = false;
	
	Path path;
	int currentWaypoint = 0;

	Seeker seeker;
	Rigidbody2D rb;
	GuardActions gActions;

	public GameObject target;

	public float attackDowntime = 3;
	float timeLastAttack;
	#endregion


	#region UnityMethods

	void Start() {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		gActions = GetComponent<GuardActions>();

		InvokeRepeating("UpdatePath", 0f, 0.5f);
		timeLastAttack = float.NegativeInfinity;

    }

	void UpdatePath()
    {
        if (seeker.IsDone())
        {
			seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
		}
		
	}

	void OnPathComplete(Path p)
	{
		if (!p.error)
        {
			path = p;
			currentWaypoint = 0;
        }
	}

    void FixedUpdate() {

		if (!activeAttack)
        {
			if (path == null)
			{
				return;
			}

			if (currentWaypoint >= path.vectorPath.Count)
			{
				if (Time.time - timeLastAttack > attackDowntime)
                {
					activeAttack = true;
					gActions.StartAttack();
				}
				
				return;
			}


			direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

			//Vector2 force = direction * speed * Time.fixedDeltaTime;

			//Vector2 movement = new Vector2(direction.x * speed * Time.fixedDeltaTime, direction.y * speed * Time.fixedDeltaTime);

			//rb.velocity += movement;
			//rb.AddForce(force);

			float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

			rb.MovePosition(rb.position + direction * gActions.moveSpeed * Time.fixedDeltaTime);


			if (distance < nextWaypointDistance)
			{
				currentWaypoint++;
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
