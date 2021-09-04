using UnityEngine;
using System.Collections;

public class BatActions : Enemy {

    #region Variables

    [Header("Layers")]
    // Objekte auf diesen Layers gelten als "Wand" an die die Feldermaus sich hängt.
    public LayerMask obstructionLayers;

    [Header("Bat-Specific Variables")]
    // So oft checkt die Fledermaus nach dem Spieler in ihrem VisionRadius wenn sie an einer Wand hängt in Sek.
    public float refreshDelay = 0.2f;
    // Von wie weit entfernt die Fledermaus den Spieler sieht
    public float visionRadius = 7;
    // Wie lange die Fledermaus nach dem Berühren einer Wand wartet, bis sie erneut zu suchen beginnt in Sek.
    public float sleepTime = 2;
    // Wenn die Fledermaus diese Zeit in Sek. lang fliegt, geht sie automatisch in den Resting-Zustand über (um endloses festgesetzt sein zu vermeiden).
    public int antiStuckSavetimeInSec = 10;
    
    public enum BatState { RESTING, FLYING }
    BatState batState;

    // Flags
    bool sleep = false;

    // Time Measurement
    float flyingTime;

    // Some Variables
    Vector2 flyingDirection;

    // Components
    Rigidbody2D rb;
    Animator animator;
    

	#endregion
	
	
	#region UnityMethods

    new protected void Start() {
        // Start von "Enemy" aufrufen
        base.Start();

        batState = BatState.RESTING;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(BehaviourRoutine());
    }

    protected void Update()
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

    private IEnumerator BehaviourRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(refreshDelay);

        while (true)
        {
            yield return wait;
            if (target)
            {

                if (sleep)
                {
                    yield return new WaitForSeconds(1f);
                    sleep = false;
                }
                else if (batState == BatState.RESTING)
                {
                    Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, visionRadius);

                    bool found = false;
                    foreach (Collider2D collided in rangeCheck)
                    {
                        if (collided.gameObject.transform.parent != null && collided.gameObject.transform.parent.gameObject == target)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
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
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if colliding with wall -> go back to Resting if true

        batState = BatState.RESTING;
        sleep = true;
        animator.SetBool("resting", true);
    }
	#endregion
}
