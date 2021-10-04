using UnityEngine;
using System.Collections.Generic;

public class SkillSlash : MonoBehaviour, ISkill
{

    [Header("Graphical Representation")]
    // GameObject - welches das Sprite repräsentiert (sollte Unterobjekt sein).
    public GameObject gfxChild;

    [Header("Slash-Variables")]
    // Attackdamage des Slashs
    public int attackDamage = 10;
    // Breite des Slashs
    public float slashWidth = 4.3f;
    // Länge des Slashs
    public float slashLength = 3.4f;
    // Distanz zwischen Beginn des Slashs und Charakter
    public float attackStartDistance = 0.9f;
    // Knockback-Stärke
    public float knockbackStrength = 3f;

    public float attackDuration = 0.3f;

    public float totalAngle = 100f;

    // Unangenehmer Angleoffset
    private readonly int ANGLEOFFSETFORANIMATION = 20;

    // Konstante 
    readonly string ENEMY_TAG = "Enemy";

    Vector3 playerToMouseVector;
    Vector3 endpointRotated;
    float angleConverted;


    bool attackActive = false;
    float attackStarttime = float.NegativeInfinity;

    List<GameObject> hitObjects;

    protected void Start()
    {
         hitObjects = new List<GameObject>();
    }
    protected void Update()
    {
       

        if (attackActive)
        {
            float angleInterpolated = (((Time.time - attackStarttime) / attackDuration) * totalAngle) - totalAngle / 2;
            endpointRotated = RotatePointAroundPivot(transform.position + Vector3.up * slashLength, gameObject.transform.position, new Vector3(0, 0, angleConverted - angleInterpolated));

            RaycastHit2D[] attackHits = Physics2D.LinecastAll(transform.position, endpointRotated);
            
            foreach (RaycastHit2D rayHit in attackHits)
            {
                Collider2D hit = rayHit.collider;

                if (hit.transform.parent != null && hit.transform.parent.tag.Equals(ENEMY_TAG) && !hitObjects.Contains(hit.transform.parent.gameObject))
                {
                    hitObjects.Add(hit.transform.parent.gameObject);
                    Vector2 knockbackDirection = (hit.transform.position - gfxChild.transform.position).normalized;
                    hit.gameObject.transform.parent.GetComponent<Enemy>().getDamaged(attackDamage, knockbackDirection, knockbackStrength);
                }
            }

            if (Time.time - attackStarttime > attackDuration)
            {
                attackActive = false;
                CleanUp();
            }
        }
    }

    public void UseSkill()
    {

        // Get Mouse Position + Convert from Screen to World-Coordinates
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        playerToMouseVector = worldPosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        angleConverted = Mathf.Abs(Vector2.SignedAngle(Vector2.up, playerToMouseVector) + 360) % 360;

        // Bewegen und rotieren von GFX zum Alignen der Animation
        float angleChildToMouse = Vector2.SignedAngle(Vector2.down, playerToMouseVector);
        Vector2 startPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - attackStartDistance, gameObject.transform.position.z);
        gfxChild.transform.position = RotatePointAroundPivot(startPos, gameObject.transform.position, new Vector3(0, 0, angleChildToMouse));
        gfxChild.transform.rotation = Quaternion.Euler(0, 0, angleChildToMouse - ANGLEOFFSETFORANIMATION);
        

        // Animation abspielen
        gfxChild.GetComponent<SkillSlashGFX>().PlayAnimation();
        
    }

    public void CreateHitbox()
    {

        attackActive = true;
        attackStarttime = Time.time;



        /*
        // Gegner bei Slash detecten
        Collider2D[] enemiesHit = Physics2D.OverlapAreaAll(rotatedPointNear, rotatedPointFar);

        foreach (Collider2D collision in enemiesHit)
        {
            if (collision.transform.parent != null && collision.transform.parent.tag.Equals(ENEMY_TAG)){
                Vector2 knockbackDirection = (collision.transform.position - gfxChild.transform.position).normalized;
                collision.gameObject.transform.parent.GetComponent<Enemy>().getDamaged(attackDamage, knockbackDirection, knockbackStrength);
            }
        }
        */
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    public void CleanUp()
    {
        Destroy(gameObject);
    }

    
    void OnDrawGizmos()
    {

        //Gizmos.DrawWireSphere(rotatedPointNear, 0.5f);
        //Gizmos.DrawWireSphere(rotatedPointFar, 0.5f);
        //Gizmos.DrawWireSphere(gfxChild.transform.position, 1f);
        //Gizmos.DrawWireSphere(gameObject.transform.position, 1f);
        Gizmos.DrawWireSphere(endpointRotated, 0.5f);
    }
    
    

}