using UnityEngine;

public abstract class Enemy : MonoBehaviour {

	#region Variables
	public int maxHealth;
	public int currentHealth;
	public float moveSpeed;
    public int attackDamage;

    public Healthbar healthBar;

	Vector2 movement;

    Vector3 posOffset = new Vector3(0f, 0f, 0f);
    #endregion


    #region UnityMethods

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

    public abstract void getDestroyed();

    protected abstract void dropOrb();
	
	#endregion
}
