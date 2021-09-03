using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageActions : Enemy {

	#region Variables
	public GameObject target;
	public GameObject fireball;


	public float initialWaittime = 2f;
	public float waitAfterTp = 3f;
	public float fireTpWait = 2f;
	public float tpDistanceToWall = 3f;

	public float fireballSpeed = 4f;
	public int fireballDamage = 1;
	public float createDistance = 2f;

	public float teleportDistance = 3;
	public int raysCast = 8;

	public Orb enemyOrb;

	bool hasTeleportedFlag = false;

	LayerMask teleportObstacles;
	GameObject thrownFireball;
	Animator animator;

	#endregion

	#region UnityMethods

	void Start()
    {
		teleportObstacles = LayerMask.GetMask("Obstacle");
		animator = GetComponent<Animator>();
		animator.SetFloat("horizontal", 0);
		animator.SetFloat("vertical", -1);

		//InvokeRepeating("doAction", initialWaittime, repeatActions);
		StartCoroutine(DoAction());
    }
	IEnumerator DoAction()
	{
		// initial wait
		yield return new WaitForSeconds(initialWaittime);

		while (true)
        {
			// 1. Fire fireball on target - instatiate next to the mage

			Vector2 directionToPlayer = (target.transform.position - gameObject.transform.position).normalized;

			thrownFireball = Instantiate(fireball, gameObject.transform.position + (Vector3) directionToPlayer * createDistance, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, directionToPlayer) + 180));

			// Animation hinzufügen
			animator.SetTrigger("cast");
			animator.SetFloat("horizontal", directionToPlayer.x);
			animator.SetFloat("vertical", directionToPlayer.y);

			thrownFireball.GetComponent<Fireball>().SetValues(directionToPlayer, fireballSpeed, fireballDamage);

			// 2. wait
			yield return new WaitForSeconds(fireTpWait);

			// 3. tp somewhere else
			animator.SetTrigger("teleport");
			

			yield return new WaitForSeconds(waitAfterTp);
			hasTeleportedFlag = false;
		}

	}

	void teleport()
    {
		// Activated by Animationend-Event
		if (!hasTeleportedFlag)
        {
			Vector3 position = findPosition();
			gameObject.transform.position = position;
			hasTeleportedFlag = true;
		}
		
	}

	Vector3 findPosition()
    {
		int angle = (int)Random.Range(0, 360);
		int angleFinal;
		//Vector2[] possiblePositions = new Vector2[raysCast];
		List<Vector2> possiblePositions = new List<Vector2>();
		Vector2 direction;

		for (int i=0; i<raysCast; i++)
        {
			// Define directions
			angleFinal = ((angle + (360 / raysCast) * i) % 360) - 180;
			
			direction.x = Mathf.Sin(Mathf.Deg2Rad * angleFinal);
			direction.y = Mathf.Cos(Mathf.Deg2Rad * angleFinal);
			direction = direction.normalized;

			if (validPos(direction, i))
            {
				possiblePositions.Add(gameObject.transform.position + new Vector3(direction.x * teleportDistance, direction.y * teleportDistance, 0));
            }
		}

		// Check if pos was found
		if (possiblePositions.Count <= 0)
        {
			return gameObject.transform.position;
        }

		// Find pos that's furthest away from Player
		float maxDistanceToPlayer = float.NegativeInfinity;
		Vector2 bestPos = gameObject.transform.position;

		for (int i=0; i<possiblePositions.Count; i++)
        {
			float distance = Vector2.Distance(target.transform.position, possiblePositions[i]);
			if (distance > maxDistanceToPlayer)
            {
				maxDistanceToPlayer = distance;
				bestPos = possiblePositions[i];
            }
        }
		return bestPos;
    }

	bool validPos(Vector2 direction, int i) {

		// cast ray
		if (!Physics2D.Raycast(gameObject.transform.position, direction, teleportDistance, teleportObstacles))
        {
			// keine Kollision gefunden

			// Check ob Position Nahe oder in der Wand wäre 
			if (!Physics2D.OverlapCircle(gameObject.transform.position + new Vector3(direction.x * teleportDistance, direction.y * teleportDistance, 0), tpDistanceToWall, teleportObstacles)) {
				return true;
			}

			return false;
        }
		return false;
	}

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
