using UnityEngine;
using UnityEngine.UI;

public class HealthBox : MonoBehaviour {

	#region Variables
	public GameObject playerChar;

	public Image[] hearts;
	public Sprite fullHeart;
	public Sprite emptyHeart;

	int currentHealth;
	int maxHealth;

    #endregion


    #region UnityMethods



    protected void Update() {
		if (playerChar)
        {
			maxHealth = playerChar.GetComponent<PlayerActions>().getMaxHealth();
			currentHealth = playerChar.GetComponent<PlayerActions>().getCurrentHealth();
		} else
        {
			currentHealth = 0;
        }
		


		if (currentHealth > maxHealth)
        {
			currentHealth = maxHealth;
        }

		for(int i = 0; i < hearts.Length; i++)
        {
			if (i < currentHealth)
            {
				hearts[i].sprite = fullHeart;
            } else
            {
				hearts[i].sprite = emptyHeart;
            }

			if (i < maxHealth)
            {
				hearts[i].enabled = true;
            } else
            {
				hearts[i].enabled = false;
            }
        }
        
    }
	
	#endregion
}
