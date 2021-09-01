using UnityEngine;

public class MovObjLogic : MonoBehaviour {

	#region Variables
	bool isActive = false;
	float radius = 0;
	float angleCounter;
	float angle = 0;
	float speed = 0;
    #endregion


    #region UnityMethods
    void FixedUpdate() {
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
		this.radius = radius;
		this.angle = angle;
		this.speed = speed;
		angleCounter = (this.angle + 180) % 360;
		isActive = true;
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
		if(collision.gameObject.transform.parent.tag.Equals("Enemy"))
        {
			// Mache Damage
			collision.gameObject.transform.parent.GetComponent<Enemy>().getDamaged(transform.parent.GetComponent<SkillBat>().attackDamage);

			// Zerstöre Objekt
			Destroy(gameObject);
		}
	}

    #endregion
}
