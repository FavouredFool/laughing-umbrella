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

    Rigidbody2D rb;
    Animator animator;
    

	#endregion
	
	
	#region UnityMethods

    void Start() {
        batState = BatState.RESTING;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
                        animator.SetBool("resting", false);
                        flyingDirection = directionToTarget;
                        animator.SetFloat("horizontal", flyingDirection.x);
                        animator.SetFloat("vertical", flyingDirection.y);
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

            // Search for Player -> Coroutine
            // When found -> calculate line -> Go into flying mode

        }
        else if (batState == BatState.FLYING)
        {
            // If flying

            // Fly in a straight line
            rb.MovePosition(rb.position + flyingDirection * moveSpeed * Time.fixedDeltaTime);
            
            

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if colliding with wall -> go back to Resting if true

        batState = BatState.RESTING;
        animator.SetBool("resting", true);
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
