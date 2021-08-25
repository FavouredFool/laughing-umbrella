using UnityEngine;

public class SkillSlash : MonoBehaviour, ISkill
{

    public float attackRange;
    LayerMask enemyLayers;

    BoxCollider2D hitBox;

    Vector3 rotatedPointNear;
    Vector3 rotatedPointFar;

    float SLASHWIDTH = 2;
    float SLASHLENGTH = 2;



    void Start()
    {
        enemyLayers = LayerMask.GetMask("Enemy");
        attackRange = 0.5f;


    }

    void Update()
    {

        // AUS PERFORMANCEGRÜNDEN SOLLTE DAS DIREKT IN use() VERSCHOBEN WERDEN WENN ICH HIER FERTIG BIN. DAS IST NUR FÜR DIE GIZMOS

        // Get Mouse Position + Convert from Screen to World-Coordinates
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // Winkel berechnen
        Vector2 playerToMouseVector = worldPosition - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        float anglePlayerToMouse = Vector2.SignedAngle(Vector2.up, playerToMouseVector);

        // Kollisionspunkt des Rechtecks vorne rechts berechnen
        Vector3 pointNear = new Vector3(gameObject.transform.position.x + SLASHWIDTH / 2, gameObject.transform.position.y, gameObject.transform.position.z);
        rotatedPointNear = RotatePointAroundPivot(pointNear, gameObject.transform.position, new Vector3(0, 0, anglePlayerToMouse));

        // Kollisionspunkt des Rechtecks hinten links berechnen
        Vector3 pointFar = new Vector3(gameObject.transform.position.x - SLASHWIDTH / 2, gameObject.transform.position.y + SLASHLENGTH, gameObject.transform.position.z);
        rotatedPointFar = RotatePointAroundPivot(pointFar, gameObject.transform.position, new Vector3(0, 0, anglePlayerToMouse));


    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot;
        dir = Quaternion.Euler(angles) * dir;
        point = dir + pivot;
        return point;
    }

    public void UseSkill()
    {
        Debug.Log("Slash!");

        // Gegner bei Slash detecten
        Collider2D[] enemiesHit = Physics2D.OverlapAreaAll(rotatedPointNear, rotatedPointFar, enemyLayers);

        foreach(Collider2D enemy in enemiesHit)
        {
            Debug.Log(enemy.name);
        }
        

    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(rotatedPointNear, 0.5f);
        Gizmos.DrawWireSphere(rotatedPointFar, 0.5f);
    }
    

}