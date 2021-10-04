using UnityEngine;
using System.Collections.Generic;

public class SkillMace : MonoBehaviour, ISkill {

	#region Variables
	public GameObject gfxChild;

    public int attackDamage = 1;
    public float knockbackStrengthLine = 10f;
    public float knockbackStrengthCircle = 5f;
    // Weite des Slash-Angriffs.
    public float lineAttackWidth = 1f;
    // Länge des Slash-Angriffs.
    public float lineAttackLength = 8f;

    public float circleAttackRadius = 4f;
    public float circleAttackDuration = 0.3f;

    // Zeitmesser für Circle-Attack
    float circleAttackStarttime = float.NegativeInfinity;

    bool circleAttackActive = false;


    Vector2 playerToMouseVector;
    Vector2 rotatedPointNear;
    Vector2 rotatedPointFar;
    Vector2 endpointRotated;



    float angleConverted;

    List<GameObject> hitObjects;


    string ENEMY_TAG = "Enemy";
    #endregion


    #region UnityMethods

    public void Start()
    {
        hitObjects = new List<GameObject>();
    }

    public void UseSkill()
    {
        /*
        // Stun player
        transform.parent.GetComponent<PlayerActions>().setIsStunned(true);
        transform.parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        */

        // SLow player 
        transform.parent.GetComponent<PlayerActions>().moveSpeed /= 2;


        // Get Mouse Position + Convert from Screen to World-Coordinates
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        playerToMouseVector = worldPosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        angleConverted = Mathf.Abs(Vector2.SignedAngle(Vector2.up, playerToMouseVector) + 360) % 360;

        // rotieren von Gameobject zum Alignen des GFX
        transform.rotation = Quaternion.Euler(0, 0, angleConverted);


        // Animation abspielen
        gfxChild.GetComponent<SkillMaceGFX>().PlayAnimation();

    }

    public void Update()
    {
        if (circleAttackActive)
        {

            // Interpoliere Grad von Anfang bis Ende um lange Line um Orc herum zu bewegen
            // Dafür -> RotateAround

            float angleInterpolated = ((Time.time - circleAttackStarttime) / (circleAttackDuration)) * 360;
            // BISHER NOCH DER FEHLER, DASS DIE ANIMATION NACH RECHTS IN DER ANIMATION COUNTER_CLOCKWISE IST
            endpointRotated = RotatePointAroundPivot(transform.position + Vector3.up * circleAttackRadius, gameObject.transform.position, new Vector3(0, 0, angleConverted + angleInterpolated));

            RaycastHit2D[] attackHits = Physics2D.LinecastAll(transform.position, endpointRotated);

            foreach (RaycastHit2D rayHit in attackHits)
            {
                Collider2D hit = rayHit.collider;

                if (hit.transform.parent != null && hit.transform.parent.tag.Equals(ENEMY_TAG) && !hitObjects.Contains(hit.transform.parent.gameObject))
                {
                    hitObjects.Add(hit.transform.parent.gameObject);
                    Vector2 knockbackDirection = (hit.transform.position - gfxChild.transform.position).normalized;
                    hit.gameObject.transform.parent.GetComponent<Enemy>().getDamaged(attackDamage, knockbackDirection, knockbackStrengthCircle);
                }
            }

            if (Time.time - circleAttackStarttime > circleAttackDuration)
            {
                circleAttackActive = false;
            }
        }
    }

	public void CreateHitboxCircle()
    {
        circleAttackStarttime = Time.time;
        circleAttackActive = true;

    }

	public void CreateHitboxLine()
    {

        // Hitbox aufbauen
        // Kollisionspunkt des Rechtecks vorne rechts berechnen
        Vector3 pointNear = new Vector3(gameObject.transform.position.x + lineAttackWidth / 2, gameObject.transform.position.y, gameObject.transform.position.z);
        rotatedPointNear = RotatePointAroundPivot(pointNear, gameObject.transform.position, new Vector3(0, 0, angleConverted));

        // Kollisionspunkt des Rechtecks hinten links berechnen
        Vector3 pointFar = new Vector3(gameObject.transform.position.x - lineAttackWidth / 2, gameObject.transform.position.y + lineAttackLength, gameObject.transform.position.z);
        rotatedPointFar = RotatePointAroundPivot(pointFar, gameObject.transform.position, new Vector3(0, 0, angleConverted));

        // Gegner bei Slash detecten
        Collider2D[] attackHits = Physics2D.OverlapAreaAll(rotatedPointNear, rotatedPointFar);

        foreach (Collider2D collision in attackHits)
        {
            if (collision.transform.parent != null && collision.transform.parent.tag.Equals(ENEMY_TAG))
            {
                Vector2 knockbackDirection = (collision.transform.position - gfxChild.transform.position).normalized;
                collision.gameObject.transform.parent.GetComponent<Enemy>().getDamaged(attackDamage, knockbackDirection, knockbackStrengthLine);
            }
        }

    }

	public void CleanUp()
    {
        //transform.parent.GetComponent<PlayerActions>().setIsStunned(false);
        transform.parent.GetComponent<PlayerActions>().moveSpeed *= 2;
        Destroy(gameObject);
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    /*
    void OnDrawGizmos()
    {

        //Gizmos.DrawWireSphere(rotatedPointNear, 0.5f);
        //Gizmos.DrawWireSphere(rotatedPointFar, 0.5f);
        //Gizmos.DrawWireSphere(gfxChild.transform.position, 1f);
        //Gizmos.DrawWireSphere(gameObject.transform.position, 1f);
        Gizmos.DrawWireSphere(endpointRotated, 0.5f);
    }
    */

    #endregion
}
