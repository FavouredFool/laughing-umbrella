using UnityEngine;
using System.Collections;

public class BossLogic : MonoBehaviour {

	#region Variables

	public GameObject boss;

	public GameObject laserAbility;
	public GameObject orbAbility;
	public GameObject flowerAbility;
	public GameObject[] orbs;
	public GameObject[] orbSpawners;
	
	public float attackDowntime = 3.0f;

	public int health;
	int currentHealth;
	public int damage;
	public int knockbackStrength;
	public float colorChangeDuration = 0.5f;

	public float regularOrbSpawnTime = 8f;
	public float regularOrbDespawnTime = 20f;

	public enum BossState { INTRO, FIGHT, END };
	BossState bossState;

	Color tempColor;
	Color damageColor = Color.red;

	SpriteRenderer sr;
	Animator animator;


	int lastAttack = -1;
	int attack = -1;
	bool attackActive = false;

	#endregion


	#region UnityMethods

	protected void Start() {
		SwapState(BossState.FIGHT);
		sr = boss.GetComponent<SpriteRenderer>();
		animator = boss.GetComponent<Animator>();

		currentHealth = health;
	}

    void SwapState(BossState state)
    {

		bossState = state;

		switch (state)
		{
			case BossState.INTRO:
				// Lil Intro Scenario + Camera Pan
				break;
			case BossState.FIGHT:
				// Fight starts
				StartCoroutine(FightBehaviour());
				StartCoroutine(regularOrbSpawn());
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
				yield return new WaitForSeconds(0.1f);

				// decide and do next attack
				attackActive = true;

				do
				{
					attack = Random.Range(0, 3);

				} while (attack == lastAttack);

				switch (attack)
				{
					case 0:
						animator.SetTrigger("laser");
						lastAttack = 0;
						break;
					case 1:
						animator.SetTrigger("orb");
						lastAttack = 1;
						break;
					case 2:
						animator.SetTrigger("flower");
						lastAttack = 2;
						break;
				}

				// Downtime between attacks
				yield return new WaitForSeconds(attackDowntime);

				switch (attack)
				{
					case 0:
						AbilityLaser();
						break;
					case 1:
						AbilityOrb();
						break;
					case 2:
						AbilityFlower();
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
		dropOrb();


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

	protected void dropOrb()
	{
		GameObject orb = Instantiate(orbs[Random.Range(0,4)], gameObject.GetComponent<OrbSpawn>().GetOrbSpawnPos(), Quaternion.identity);
		orb.GetComponent<Orb>().SetTime(regularOrbDespawnTime);
	}

	IEnumerator regularOrbSpawn()
    {

		Vector2 spawnPos = new Vector2();
		int spawnNr = -1;
		int lastSpawnNr = -1;

		while (bossState == BossState.FIGHT)
        {
			yield return new WaitForSeconds(regularOrbSpawnTime);

			do
			{
				spawnNr = Random.Range(0, 4);

			} while (spawnNr == lastSpawnNr);

			lastSpawnNr = spawnNr;

			switch(spawnNr)
            {
				case 0:
					spawnPos = orbSpawners[0].transform.position;
					break;
				case 1:
					spawnPos = orbSpawners[1].transform.position;
					break;
				case 2:
					spawnPos = orbSpawners[2].transform.position;
					break;
				case 3:
					spawnPos = orbSpawners[3].transform.position;
					break;
            }

			GameObject orb = Instantiate(orbs[Random.Range(0, 4)], spawnPos, Quaternion.identity);
			orb.GetComponent<Orb>().SetTime(regularOrbDespawnTime);

		}
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
