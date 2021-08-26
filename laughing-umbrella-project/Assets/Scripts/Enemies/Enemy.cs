using UnityEngine;

public abstract class Enemy : MonoBehaviour {

	#region Variables
	public int maxHealth;
	public int currentHealth;
	public float moveSpeed;

	public Healthbar healthBar;

	Vector2 movement;
    #endregion


    #region UnityMethods

    private void Start()
    {
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
    }

    public void getDamaged(int attackDamage)
    {
        // Get damaged, ggf. drop orb
        currentHealth -= attackDamage;
        if (currentHealth <= 0)
        {
            // Destroy Object
            getDestroyed();
        } else
        {
            // Healthbar neu setzen
            healthBar.SetHealth(currentHealth);
        }
    }

    public abstract void getDestroyed();
	
	#endregion
}
