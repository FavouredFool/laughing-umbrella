using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour {

	#region Variables
	string ENEMY_TAG = "Enemy";
	#endregion
	
	
	#region UnityMethods
	public void OnTriggerEnter2D(Collider2D collision)
	{
		// Collision with an enemy
		if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag.Equals(ENEMY_TAG))
			transform.parent.GetComponent<PlayerActions>().getDamaged(collision.gameObject.transform.parent.GetComponent<Enemy>().attackDamage);
	}

	#endregion
}
