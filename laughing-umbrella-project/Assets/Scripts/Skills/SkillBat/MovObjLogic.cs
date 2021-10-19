using UnityEngine;

public class MovObjLogic : MonoBehaviour {

	#region Variables
	// private Variablen
	float radius = 0;
	float speed = 0;
	float angle = 0;
	float angleCounter;

	// Flags
	bool isActive = false;

	readonly string ENEMY_TAG = "Enemy";
	readonly string BOSS_TAG = "Boss";

	#endregion


	#region UnityMethods
	protected void FixedUpdate() {
		if (isActive)
        {
			Vector2 position;
			position.x = gameObject.transform.parent.position.x + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
			position.y = gameObject.transform.parent.position.y + radius * Mathf.Cos(Mathf.Deg2Rad * angle);

			gameObject.transform.position = new Vector3(position.x, position.y, 0);


			angleCounter = (angleCounter + speed) % 360;
			angle = angleCounter - 180;
        }
    }

	public void CreateMovement(float radius, float angle, float speed)
    {
		FindObjectOfType<AudioManager>().Play("Fledermaus");

		this.radius = radius;
		this.angle = angle;
		this.speed = speed;
		angleCounter = (this.angle + 180) % 360;
		isActive = true;
	}

    protected void OnTriggerEnter2D(Collider2D collision)
    {
		if(collision.gameObject.transform.parent != null && collision.gameObject.transform.parent.tag.Equals(ENEMY_TAG))
        {

			Vector2 knockbackDirection = (collision.transform.position - gameObject.transform.position).normalized;
			// Mache Damage
			collision.gameObject.transform.parent.GetComponent<Enemy>().getDamaged(transform.parent.GetComponent<SkillBat>().attackDamage, knockbackDirection, transform.parent.GetComponent<SkillBat>().knockbackStrength);

			// Zerstöre Objekt
			Destroy(gameObject);
		}
		else if (collision.transform.parent != null && collision.transform.parent.parent != null && collision.transform.parent.tag.Equals(BOSS_TAG))
		{
			// Boss bekommt Schaden
			collision.gameObject.transform.parent.parent.GetComponent<BossLogic>().GetDamaged(transform.parent.GetComponent<SkillBat>().attackDamage);

			// Zerstöre Objekt
			Destroy(gameObject);
		}
	}

    #endregion
}
