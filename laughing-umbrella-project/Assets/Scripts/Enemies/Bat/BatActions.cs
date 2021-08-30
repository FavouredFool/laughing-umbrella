using UnityEngine;
using System.Collections;

public class BatActions : Enemy {

    #region Variables

    public Orb enemyOrb;
    public float refreshDelay = 0.2f;
    public float visionRadius;
    public GameObject target;
    public LayerMask obstructionLayers;
    public LayerMask targetLayer;
    

    public enum BatState { RESTING, FLYING }
    BatState batState;

    Vector2 flyingDirection;
    

	#endregion
	
	
	#region UnityMethods

    void Start() {
        batState = BatState.RESTING;
        StartCoroutine(RestingRoutine());
    }

    private IEnumerator RestingRoutine()
    {
        
        WaitForSeconds wait = new WaitForSeconds(refreshDelay);

        while (true)
        {
            yield return wait;

            if (batState == BatState.RESTING)
            {
                Debug.Log("Seaaarching");
                Collider2D rangeCheck = Physics2D.OverlapCircle(transform.position, visionRadius, targetLayer);
                if (rangeCheck)
                {

                    Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                    float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                    if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayers))
                    {
                        batState = BatState.FLYING;
                        flyingDirection = directionToTarget;
                        Debug.Log("FOUND!");
                    }
                }
            }
        }
    }

    void Update() {
        
        
        if (batState == BatState.RESTING)
        {
            // If resting

            // Search for Player
            // When found -> calculate line -> Go into flying mode

        }
        else if (batState == BatState.FLYING)
        {
            // If flying
            
            // Fly in a straight line
            // Check if colliding with NEW wall -> go back to Resting if true

        }


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
