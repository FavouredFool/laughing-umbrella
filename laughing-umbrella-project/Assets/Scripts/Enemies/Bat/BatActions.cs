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
    public float sleepTime;
    public int antiStuckSavetimeInSec = 10;
    

    public enum BatState { RESTING, FLYING }
    BatState batState;

    bool sleep = false;
    float flyingTime;

    Vector2 flyingDirection;

    Rigidbody2D rb;
    Animator animator;
    

	#endregion
	
	
	#region UnityMethods

    void Start() {
        batState = BatState.RESTING;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(BehaviourRoutine());
    }

    private IEnumerator BehaviourRoutine()
    {
        
        WaitForSeconds wait = new WaitForSeconds(refreshDelay);

        while (true)
        {
            yield return wait;

            if (sleep)
            {
                yield return new WaitForSeconds(1f);
                sleep = false;
            }
            else if (batState == BatState.RESTING)
            {
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
                        flyingTime = Time.time;
                        animator.SetFloat("horizontal", flyingDirection.x);
                        animator.SetFloat("vertical", flyingDirection.y);
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (batState == BatState.FLYING)
        {
            if (Time.time - flyingTime > antiStuckSavetimeInSec)
            {
                // Fledermaus ist stuck -> resten
                batState = BatState.RESTING;
                sleep = true;
                animator.SetBool("resting", true);
            }

            rb.MovePosition(rb.position + flyingDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if colliding with wall -> go back to Resting if true

        batState = BatState.RESTING;
        sleep = true;
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
