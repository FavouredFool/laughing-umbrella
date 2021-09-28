using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnemyCollision : MonoBehaviour {

	#region Variables
	string ENEMY_TAG = "Enemy";
	string STAIR_TAG = "Stairs";
    #endregion


    #region UnityMethods


    public void OnTriggerEnter2D(Collider2D collision)
	{
		// Collision with an enemy
		if (collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag.Equals(ENEMY_TAG))
		{
			Vector2 knockbackDirection = gameObject.transform.position - collision.transform.position;
			transform.parent.GetComponent<PlayerActions>().getDamaged(collision.gameObject.transform.parent.GetComponent<Enemy>().attackDamage, knockbackDirection, collision.gameObject.transform.parent.GetComponent<Enemy>().knockbackStrength);
		} else if (collision.gameObject.tag.Equals(STAIR_TAG))
        {
			// Collision with Stairs
			PlayerValues.health = transform.parent.gameObject.GetComponent<PlayerActions>().getCurrentHealth();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}




	}

	#endregion
}
