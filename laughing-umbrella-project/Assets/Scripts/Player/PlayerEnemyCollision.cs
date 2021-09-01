using UnityEngine;

public class PlayerEnemyCollision : MonoBehaviour {

	#region Variables

	string ENEMY_TAG = "Enemy";
	#endregion
	
	
	#region UnityMethods
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag.Equals(ENEMY_TAG))
			transform.parent.GetComponent<PlayerActions>().getDamaged(collision.gameObject.transform.parent.GetComponent<Enemy>().attackDamage);
	}

	#endregion
}
