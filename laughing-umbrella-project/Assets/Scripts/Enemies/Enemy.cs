using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

    #region Variables

    [Header("General Object-Inputs")]
    // Das Ziel des Gegners. Sollte eig. immer der Spieler sein. Muss man jedes mal neu definieren, da Prefabs keine konkreten Objekte annehmen.
    public GameObject target;
    // Falls der Gegner als Unterobjekt einen "Canvas" und darunter eine "Healthbar" hat, muss die hier reingezogen werden. Wenn nicht kann's freigelassen werden.
    public Healthbar healthBar;
    // Der Orb den der Gegner fallen lässt. Hier wird eigentlich nach der Klasse "Orb" im Skript gefragt, aber es reicht auch das Prefab reinzuziehen da Unity intelligent ist.
    public Orb enemyOrb;

    [Header("General Enemy-Variables")]
    // Maximale Leben des Gegners.
    public int maxHealth = 15;
    // Geschwindigkeit des Gegners.
    public float moveSpeed = 5;
    // Schaden den der Gegner macht (meistens 1, maximal 2).
    public int attackDamage = 1;
    // Stunlänge nach Treffer
    public float stunDuration = 0.5f;
    // KnockbackStrength
    public float knockbackStrength = 10f;

    [Header("Effects")]
    // Besiegt-Effect
    public GameObject killedObj;

    int currentHealth;

    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    // Flags
    protected bool isStunned;

    #endregion


    #region UnityMethods

    protected void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        if (healthBar)
        {
            // Für Lebensleiste
            healthBar.SetMaxHealth(maxHealth);
        }
        
    }

    public void getDamaged(int attackDamage, Vector2 knockbackDirection, float knockbackStrength)
    {
        // Drop Orb
        dropOrb();


        // Get damaged
        currentHealth -= attackDamage;
        if (currentHealth <= 0)
        {
            // Destroy Object
            getDestroyed();
        } else
        {

            // Stun Enemy
            StartCoroutine(toggleStun());

            // get Knocked back
            createKnockback(knockbackDirection, knockbackStrength);

            if (healthBar)
            {
                // Healthbar neu setzen
                healthBar.SetHealth(currentHealth);
            }
        }
    }

    IEnumerator toggleStun()
    {
        isStunned = true;
        sr.color = Color.red;
        yield return new WaitForSeconds(stunDuration);
        sr.color = Color.white;
        isStunned = false;
    }

    private void createKnockback(Vector2 knockbackDirection, float knockbackStrength)
    {
        //rb.AddForce(knockbackDirection * knockbackStrength);

        rb.velocity = knockbackDirection * knockbackStrength;
        //rb.AddForce(knockbackDirection * knockbackStrength*100);


    }

    private void getDestroyed()
    {

        // Create Effect
        Instantiate(killedObj, gameObject.transform.position, Quaternion.identity);


        // Destroy Object
        Destroy(gameObject);
    }

    protected void dropOrb()
    {
        Instantiate(enemyOrb, gameObject.GetComponent<OrbSpawn>().GetOrbSpawnPos(), Quaternion.identity);
    }

    public bool getIsStunned()
    {
        return isStunned;
    }
	
	#endregion
}
