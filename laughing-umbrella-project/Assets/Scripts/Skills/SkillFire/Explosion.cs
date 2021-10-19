using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float explosionRange;
    int explosionDamage;
    float explosionKnockback;

    readonly string ENEMY_TAG = "Enemy";
    readonly string BOSS_TAG = "Boss";

    // Start is called before the first frame update
    void Start()
    {

        FindObjectOfType<AudioManager>().Play("FireballExplosion");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, explosionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.transform.parent != null && collider.transform.parent.tag.Equals(ENEMY_TAG))
            {
                Vector2 knockbackDirection = (collider.transform.position - gameObject.transform.position).normalized;
                collider.gameObject.transform.parent.GetComponent<Enemy>().getDamaged(explosionDamage, knockbackDirection, explosionKnockback);

            }
            else if (collider.transform.parent != null && collider.transform.parent.parent != null && collider.transform.parent.tag.Equals(BOSS_TAG))
            {
                // Boss bekommt Schaden
                collider.gameObject.transform.parent.parent.GetComponent<BossLogic>().GetDamaged(explosionDamage);
            }
        }
    }

    
    public void SetValues(int damage, float knockback, float range)
    {
        explosionDamage = damage;
        explosionKnockback = knockback;
        explosionRange = range;
    }
    

    void AnimationEndExplosion()
    {
        Destroy(gameObject);
    }
}
