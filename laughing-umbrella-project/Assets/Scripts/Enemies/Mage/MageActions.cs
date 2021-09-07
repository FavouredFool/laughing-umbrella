using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MageActions : Enemy {

	#region Variables

	[Header("Mage-Specific GameObjects")]
	// Prefab, welches geschmissen wird
	public GameObject fireball;

	[Header("Fireball-Specific Variables")]
	// Geschwindigkeit des Feuerballs
	public float fireballSpeed = 8f;

	// Distanz vom Mage, die der Feuerball bei Kreation hat
	public float createDistance = 1.5f;

	[Header("Teleport-Specific Variables")]
	// Layer durch die nicht teleportiert werden kann.
	public LayerMask teleportObstacles;

	// Zahl von Rays in alle Richtungen vom Mage ausgehend um einen passenden Teleportationsort zu finden.
	public int raysCast = 64;

	// Distanz die durch den Teleport überbrückt wird
	public float teleportDistance = 6;

	// Distanz die ein Teleport-Ort zur Wand haben muss um valide zu sein.
	public float tpDistanceToWall = 3f;

	[Header("Time-Specific Variables")]
	// Wartezeit vor der ersten Aktion des Mages in Sek.
	public float initialWait = 2f;

	// Wartezeit zwischen dem Wurf des Feuerballs und dem Teleport in Sek.
	public float waitBetweenFireTp = 2f;

	// Wartezeit nach dem Teleport in Sek.
	public float waitAfterTp = 3f;

	// Components
	Animator animator;

	// Flag
	bool hasTeleportedFlag = false;
	#endregion

	#region UnityMethods

	new protected void Start()
    {
		// Start von "Enemy" aufrufen
		base.Start();

		animator = GetComponent<Animator>();
		animator.SetFloat("horizontal", 0);
		animator.SetFloat("vertical", -1);

		//InvokeRepeating("doAction", initialWaittime, repeatActions);
		StartCoroutine(DoAction());
    }
	protected IEnumerator DoAction()
	{
		// initial wait
		yield return new WaitForSeconds(initialWait);

		while (true)
        {
			if (target)
			{
				// 1. Fire fireball on target - instatiate next to the mage

				Vector2 directionToPlayer = (target.transform.position - gameObject.transform.position).normalized;

				GameObject thrownFireball = Instantiate(fireball, gameObject.transform.position + (Vector3)directionToPlayer * createDistance, Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, directionToPlayer) + 180));

				// Animation hinzufügen
				animator.SetTrigger("cast");
				animator.SetFloat("horizontal", directionToPlayer.x);
				animator.SetFloat("vertical", directionToPlayer.y);

				thrownFireball.GetComponent<Fireball>().SetValues(directionToPlayer, fireballSpeed, attackDamage);
			}
			// 2. wait
			yield return new WaitForSeconds(waitBetweenFireTp);

			if (target)
			{
				// 3. tp somewhere else
				animator.SetTrigger("teleport");
			}

			yield return new WaitForSeconds(waitAfterTp);
			hasTeleportedFlag = false;
		}

	}

	public void Teleport()
    {
		// Activated by Animationend-Event
		if (!hasTeleportedFlag && target)
        {
			Vector3 position = FindPosition();
			gameObject.transform.position = position;
			hasTeleportedFlag = true;
		}
		
	}

	protected Vector3 FindPosition()
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

			if (ValidPos(direction, i))
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

	protected bool ValidPos(Vector2 direction, int i) {

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

	
	
	#endregion
}
