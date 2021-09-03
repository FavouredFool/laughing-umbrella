using UnityEngine;

public class SkillFire : MonoBehaviour, ISkill {

    #region Variables
    public float fireballSpeed = 4f;
    public int fireballDamage = 10;
    public float createDistance = 2f;

    public GameObject fireball;
    GameObject thrownFireball;

    #endregion


    #region UnityMethods

    void Start() {
        
    }


	public void UseSkill()
    {

        // Richtung definieren
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2 playerToMouseVector = (worldPosition - (Vector2)gameObject.transform.position).normalized;

        // Winkel
        float anglePlayerToMouse = Vector2.SignedAngle(Vector2.up, playerToMouseVector)+180;


        // Feuerball schieﬂen
        thrownFireball = Instantiate(fireball, transform.position + (Vector3)playerToMouseVector * createDistance, Quaternion.Euler(0,0,anglePlayerToMouse));

        thrownFireball.GetComponent<PlayerFireball>().SetValues(playerToMouseVector, fireballSpeed, fireballDamage);

        CleanUp();
    }

	public void CleanUp()
    {
        Destroy(gameObject);
    }
	
	#endregion
}
