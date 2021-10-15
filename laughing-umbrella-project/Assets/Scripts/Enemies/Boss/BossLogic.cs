using UnityEngine;
using System.Collections;

public class BossLogic : MonoBehaviour {

	#region Variables

	public GameObject boss;

	public GameObject laserAbility;
	public GameObject orbAbility;
	public GameObject flowerAbility;
	
	public float attackDowntime = 3.0f;

	public int health;
	int currentHealth;
	public int damage;
	public int knockbackStrength;
	public float colorChangeDuration = 0.5f;

	public enum BossState { INTRO, FIGHT, END };
	BossState bossState;

	Color tempColor;
	Color damageColor = Color.red;

	SpriteRenderer sr;


	int lastAttack = -1;
	int attack = -1;
	bool attackActive = false;

	#endregion


	#region UnityMethods

	protected void Start() {
		SwapState(BossState.FIGHT);
		sr = boss.GetComponent<SpriteRenderer>();

		currentHealth = health;
	}

    void SwapState(BossState state)
    {
		switch (state)
		{
			case BossState.INTRO:
				// Lil Intro Scenario + Camera Pan
				break;
			case BossState.FIGHT:
				// Fight starts
				StartCoroutine(FightBehaviour());
				break;
			case BossState.END:
				// Fight is over, boss is dead
				break;
		}
	}


	IEnumerator FightBehaviour()
    {
		// Different Attacks with different times are being calculated here -> Coroutine
		
		while (bossState != BossState.END)
		{
			if (!attackActive)
            {
				// Downtime between attacks
				yield return new WaitForSeconds(attackDowntime);

				// decide and do next attack
				attackActive = true;

				do
				{
					attack = Random.Range(0, 3);

				} while (attack == lastAttack);

				switch (attack)
				{
					case 0:
						AbilityLaser();
						lastAttack = 0;
						break;
					case 1:
						AbilityOrb();
						lastAttack = 1;
						break;
					case 2:
						AbilityFlower();
						lastAttack = 2;
						break;
				}
			} else
            {
				yield return new WaitForSeconds(0.2f);
            }
			
		}
    }

	public void GetDamaged(int attackDamage)
    {
		// Drop Orb
		//dropOrb();


		// Get damaged
		currentHealth -= attackDamage;
		if (currentHealth <= 0)
		{
			// Destroy Object
			GetDestroyed();
		}
		else
		{
			StartCoroutine(ChangeColor());
			/*
			if (healthBar)
			{
				// Healthbar neu setzen
				healthBar.SetHealth(currentHealth);
			}
			*/
		}
	}

	IEnumerator ChangeColor()
    {
		tempColor = sr.color;
		sr.color = damageColor;
		yield return new WaitForSeconds(colorChangeDuration);
		sr.color = tempColor;
	}

	void GetDestroyed()
    {
		// Create Effect
		//Instantiate(killedObj, gameObject.transform.position, Quaternion.identity);

		// Destroy Object
		Destroy(gameObject);
	}


	void AbilityLaser()
    {
		Instantiate(laserAbility, boss.transform.position - new Vector3(0, 2, 0), Quaternion.identity, boss.transform);
    }

	void AbilityOrb()
    {
		Instantiate(orbAbility, boss.transform.position - new Vector3(0,2,0), Quaternion.identity, boss.transform);
	}

	void AbilityFlower()
    {
		Instantiate(flowerAbility, new Vector3(), Quaternion.identity, boss.transform);
	}

	public void SetAttackActive(bool attackActive)
    {
		this.attackActive = attackActive;
    }

	public bool GetAttackActive()
    {
		return attackActive;
    }



	#endregion
}
