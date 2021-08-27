using UnityEngine;
using Pathfinding;

public class GuardPathfinder: MonoBehaviour {

	#region Variables
	public Transform target;
	public float nextWaypointDistance = 1f;

	public Transform enemyGFX;

	private Vector2 direction;
	bool activeAttack = false;
	
	Path path;
	int currentWaypoint = 0;
	bool reachedEndOfPath = false;

	Seeker seeker;
	Rigidbody2D rb;
	GuardActions gActions;
	#endregion


	#region UnityMethods

	void Start() {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		gActions = GetComponent<GuardActions>();

		InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

	void UpdatePath()
    {
        if (seeker.IsDone())
        {
			seeker.StartPath(rb.position, target.position, OnPathComplete);
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
				//Debug.Log("angriff!");
				activeAttack = true;
				gActions.StartAttack();
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

			// Flip Sprite
			if (rb.velocity.x >= 0.01f)
			{
				enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
			}
			else if (rb.velocity.x <= -0.01f)
			{
				enemyGFX.localScale = new Vector3(1f, 1f, 1f);
			}


		}
        
    }

	public void EndAttack()
    {
		// flippe Flag um Gegner wieder laufen zu lassen
		activeAttack = false;

	}

	public Vector2 getDirection()
    {
		return direction;
    }

	#endregion
}
