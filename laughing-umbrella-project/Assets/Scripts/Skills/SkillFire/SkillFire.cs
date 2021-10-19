using UnityEngine;

public class SkillFire : MonoBehaviour, ISkill {

    #region Variables
    [Header("GameObjects")]
    // Das GameObject was bei Nutzung gespawned wird
    public GameObject fireball;

    [Header("Fireball-Variables")]
    // Schaden des Feuerballs
    public int fireballDamage = 10;
    // Geschwindigkeit des Feuerballs
    public float fireballSpeed = 8f;
    // Distanz vom Spieler bei welcher der Feuerball erstellt wird
    public float createDistance = 2f;
    // St‰rke des Knockback
    public float knockbackStrength = 2f;

    public float explosionRange = 3f;


    GameObject thrownFireball;

    #endregion


    #region UnityMethods

	public void UseSkill()
    {

        FindObjectOfType<AudioManager>().Play("FireballCast");

        // Richtung definieren
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Vector2 playerToMouseVector = (worldPosition - (Vector2)gameObject.transform.position).normalized;

        // Winkel
        float anglePlayerToMouse = Vector2.SignedAngle(Vector2.up, playerToMouseVector)+180;


        // Feuerball schieﬂen
        thrownFireball = Instantiate(fireball, transform.position + (Vector3)playerToMouseVector * createDistance, Quaternion.Euler(0,0,anglePlayerToMouse));

        thrownFireball.GetComponent<PlayerFireball>().SetValues(playerToMouseVector, fireballSpeed, fireballDamage, knockbackStrength, explosionRange);

        CleanUp();
    }

	public void CleanUp()
    {
        Destroy(gameObject);
    }
	
	#endregion
}
