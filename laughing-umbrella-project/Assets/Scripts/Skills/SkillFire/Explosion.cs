using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    public float explosionRange;
    int explosionDamage;
    float explosionKnockback;

    readonly string ENEMY_TAG = "Enemy";

    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, explosionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.transform.parent != null && collider.transform.parent.tag.Equals(ENEMY_TAG))
            {
                Vector2 knockbackDirection = (collider.transform.position - gameObject.transform.position).normalized;
                collider.gameObject.transform.parent.GetComponent<Enemy>().getDamaged(explosionDamage, knockbackDirection, explosionKnockback);

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
