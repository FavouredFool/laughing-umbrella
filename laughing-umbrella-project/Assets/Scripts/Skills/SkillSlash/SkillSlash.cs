using UnityEngine;

public class SkillSlash : MonoBehaviour, ISkill
{
    public float SLASHWIDTH;
    public float SLASHLENGTH;

    public float ATTACKSTARTDISTANCE = 0.7f;

    // Unangenehmer Angleoffset
    private readonly int ANGLEOFFSETFORANIMATION = 20;

    public int ATTACKDAMAGE;

    public GameObject gfxChild;

    public LayerMask enemyLayers;

    // Mousepos
    Vector2 worldPosition;
    Vector2 playerToMouseVector;

    Vector3 rotatedPointFar;
    Vector3 rotatedPointNear;


    public void UseSkill()
    {
        // Get Mouse Position + Convert from Screen to World-Coordinates
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        playerToMouseVector = worldPosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        // Winkel berechnen
        float anglePlayerToMouse = Vector2.SignedAngle(Vector2.up, playerToMouseVector);

        // Kollisionspunkt des Rechtecks vorne rechts berechnen
        Vector3 pointNear = new Vector3(gameObject.transform.position.x + SLASHWIDTH / 2, gameObject.transform.position.y, gameObject.transform.position.z);
        rotatedPointNear = RotatePointAroundPivot(pointNear, gameObject.transform.position, new Vector3(0, 0, anglePlayerToMouse));

        // Kollisionspunkt des Rechtecks hinten links berechnen
        Vector3 pointFar = new Vector3(gameObject.transform.position.x - SLASHWIDTH / 2, gameObject.transform.position.y + SLASHLENGTH, gameObject.transform.position.z);
        rotatedPointFar = RotatePointAroundPivot(pointFar, gameObject.transform.position, new Vector3(0, 0, anglePlayerToMouse));



        // Bewegen und rotieren von GFX zum Alignen der Animation
        float angleChildToMouse = Vector2.SignedAngle(Vector2.down, playerToMouseVector);
        Vector2 startPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - ATTACKSTARTDISTANCE, gameObject.transform.position.z);
        gfxChild.transform.position = RotatePointAroundPivot(startPos, gameObject.transform.position, new Vector3(0, 0, angleChildToMouse));
        gfxChild.transform.rotation = Quaternion.Euler(0, 0, angleChildToMouse - ANGLEOFFSETFORANIMATION);

        // Animation abspielen
        gfxChild.GetComponent<SkillSlashGFX>().PlayAnimation();
        
    }

    public void CreateHitbox()
    {
        

        // Gegner bei Slash detecten
        Collider2D[] enemiesHit = Physics2D.OverlapAreaAll(rotatedPointNear, rotatedPointFar, enemyLayers);

        foreach (Collider2D enemy in enemiesHit)
        {
            enemy.gameObject.GetComponent<Enemy>().getDamaged(ATTACKDAMAGE);
        }
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

    /*
    void OnDrawGizmos()
    {

        Gizmos.DrawWireSphere(rotatedPointNear, 0.5f);
        Gizmos.DrawWireSphere(rotatedPointFar, 0.5f);
        Gizmos.DrawWireSphere(gfxChild.transform.position, 1f);
        Gizmos.DrawWireSphere(gameObject.transform.position, 1f);
    }
    */
    

}