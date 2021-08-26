using UnityEngine;

public class SkillSlash : MonoBehaviour, ISkill
{

    LayerMask enemyLayers;

    BoxCollider2D hitBox;

    Vector3 rotatedPointNear;
    Vector3 rotatedPointFar;

    public float SLASHWIDTH;
    public float SLASHLENGTH;

    public float ATTACKSTARTDISTANCE = 0.7f;

    // Unangenehmer Angleoffset
    private int ANGLEOFFSETFORANIMATION = 20;

    public int attackDamage;

    SpriteRenderer sr;
    public GameObject gfxChild;


    void Start()
    {
        sr = gfxChild.GetComponent<SpriteRenderer>();

        enemyLayers = LayerMask.GetMask("Enemy");
    }

    void Update()
    {

        

    }

    public void UseSkill()
    {

        // Get Mouse Position + Convert from Screen to World-Coordinates
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // Winkel berechnen
        Vector2 playerToMouseVector = worldPosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        float anglePlayerToMouse = Vector2.SignedAngle(Vector2.up, playerToMouseVector);

        // Kollisionspunkt des Rechtecks vorne rechts berechnen
        Vector3 pointNear = new Vector3(gameObject.transform.position.x + SLASHWIDTH / 2, gameObject.transform.position.y + ATTACKSTARTDISTANCE / 2, gameObject.transform.position.z);
        rotatedPointNear = RotatePointAroundPivot(pointNear, gameObject.transform.position, new Vector3(0, 0, anglePlayerToMouse));

        // Kollisionspunkt des Rechtecks hinten links berechnen
        Vector3 pointFar = new Vector3(gameObject.transform.position.x - SLASHWIDTH / 2, gameObject.transform.position.y + SLASHLENGTH, gameObject.transform.position.z);
        rotatedPointFar = RotatePointAroundPivot(pointFar, gameObject.transform.position, new Vector3(0, 0, anglePlayerToMouse));


        // Animation
        // Bewegen von gfxChild

        float angleChildToMouse = Vector2.SignedAngle(Vector2.down, playerToMouseVector);
        Vector2 startPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - ATTACKSTARTDISTANCE, gameObject.transform.position.z);
        gfxChild.transform.position = RotatePointAroundPivot(startPos, gameObject.transform.position, new Vector3(0, 0, angleChildToMouse));
        gfxChild.transform.rotation = Quaternion.Euler(0, 0, angleChildToMouse - ANGLEOFFSETFORANIMATION);

        // Gegner bei Slash detecten
        Collider2D[] enemiesHit = Physics2D.OverlapAreaAll(rotatedPointNear, rotatedPointFar, enemyLayers);

        foreach(Collider2D enemy in enemiesHit)
        {
            Debug.Log(enemy.name);
            enemy.gameObject.GetComponent<Enemy>().getDamaged(attackDamage);

        }


        // Animation abspielen
        gfxChild.GetComponent<Animator>().Play("Ability_Slash");


        // After Animation end -> cleanup
        

    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    public void cleanUp()
    {
        // Bisher kein Cleanup nötig
    }

    void OnDrawGizmos()
    {
        
        //Gizmos.DrawWireSphere(rotatedPointNear, 0.5f);
        //Gizmos.DrawWireSphere(rotatedPointFar, 0.5f);
        //Gizmos.DrawWireSphere(gfxChild.transform.position, 1f);
        
    }
    

}