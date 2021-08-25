using UnityEngine;

public class SkillSlash : MonoBehaviour, ISkill
{

    public float attackRange;

    void Start()
    {
        LayerMask enemyLayers = LayerMask.GetMask("Enemy");
        attackRange = 0.5f;
        
    }

    public void UseSkill()
    {
        Debug.Log("Slash!");

        // Get Mouse Position + Convert from Screen to World-Coordinates

        /*Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Instantiate(gameObject, new Vector3(worldPosition.x, worldPosition.y, 1), Quaternion.identity);*/


    }


}