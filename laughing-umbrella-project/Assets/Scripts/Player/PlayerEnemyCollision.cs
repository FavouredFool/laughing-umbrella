using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnemyCollision : MonoBehaviour {

	#region Variables
	string ENEMY_TAG = "Enemy";
	string STAIR_TAG = "Stairs";
	string LASER_TAG = "BossLaser";
	string BALL_TAG = "BossBall";
	string FLOWER_TAG = "FlowerPattern";
	string BOSS_TAG = "Boss";
	#endregion


	#region UnityMethods


	public void OnTriggerEnter2D(Collider2D collision)
	{
		
		// Collision with an enemy
		if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag.Equals(ENEMY_TAG))
		{
			Vector2 knockbackDirection = (gameObject.transform.position - collision.transform.position).normalized;
			transform.parent.GetComponent<PlayerActions>().getDamaged(collision.gameObject.transform.parent.GetComponent<Enemy>().attackDamage, knockbackDirection, collision.gameObject.transform.parent.GetComponent<Enemy>().knockbackStrength);
		} else if (collision.gameObject.tag.Equals(LASER_TAG))
        {
			Vector2 knockbackDirection = (gameObject.transform.position - collision.transform.position).normalized;
			transform.parent.GetComponent<PlayerActions>().getDamaged(collision.transform.parent.GetComponent<LaserScript>().damage, knockbackDirection, collision.transform.parent.GetComponent<LaserScript>().knockbackStrength);
        } else if (collision.gameObject.tag.Equals(BALL_TAG))
        {
			Destroy(collision.gameObject);
			Vector2 knockBackDirection = collision.GetComponent<OrbProjectile>().GetDirection().normalized;
			transform.parent.GetComponent<PlayerActions>().getDamaged(collision.GetComponent<OrbProjectile>().GetDamage(), knockBackDirection, collision.GetComponent<OrbProjectile>().GetKnockbackStrength());
        } 
		else if (collision.gameObject.tag.Equals(FLOWER_TAG))
        {
			Vector2 knockBackDirection = (gameObject.transform.position - collision.transform.position).normalized;
			transform.parent.GetComponent<PlayerActions>().getDamaged(collision.GetComponent<FlowerInstanceScript>().GetDamage(), knockBackDirection, collision.GetComponent<FlowerInstanceScript>().GetKnockbackStrength());
		} 
		else if (collision.gameObject.tag.Equals(BOSS_TAG))
        {
			Vector2 knockbackDirection = (gameObject.transform.position - collision.transform.position).normalized;
			transform.parent.GetComponent<PlayerActions>().getDamaged(collision.transform.parent.parent.GetComponent<BossLogic>().damage, knockbackDirection, collision.transform.parent.parent.GetComponent<BossLogic>().knockbackStrength);
        }
		else if (collision.gameObject.tag.Equals(STAIR_TAG))
        {
			// Collision with Stairs
			MainScript.health = transform.parent.gameObject.GetComponent<PlayerActions>().getCurrentHealth();
			MainScript.maxHealth = transform.parent.gameObject.GetComponent<PlayerActions>().getMaxHealth();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}




	}

	#endregion
}
