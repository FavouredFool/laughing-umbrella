using UnityEngine;

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

    [Header("Effects")]
    // Besiegt-Effect
    public GameObject killedObj;

    int currentHealth;

    #endregion


    #region UnityMethods

    protected void Start()
    {
        currentHealth = maxHealth;
        if (healthBar)
        {
            // Für Lebensleiste
            healthBar.SetMaxHealth(maxHealth);
        }
        
    }

    public void getDamaged(int attackDamage)
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
            if (healthBar)
            {
                // Healthbar neu setzen
                healthBar.SetHealth(currentHealth);
            }
        }
    }

    protected void getDestroyed()
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
	
	#endregion
}
