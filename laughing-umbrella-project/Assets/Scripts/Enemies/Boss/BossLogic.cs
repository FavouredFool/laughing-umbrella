using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossLogic : MonoBehaviour {

	#region Variables

	public GameObject boss;

	public GameObject laserAbility;
	public GameObject orbAbility;
	public GameObject flowerAbility;
	public GameObject[] orbs;
	public GameObject[] orbSpawners;
	public GameObject wall;
	public GameObject player;
	public GameObject killedObj;
	public GameObject endEnemy;
	public GameObject healthBar;

	public float attackDowntime = 3.0f;

	public int health;
	int currentHealth;
	public int damage;
	public int knockbackStrength;
	public float colorChangeDuration = 0.5f;

	public float regularOrbSpawnTime = 8f;
	public float regularOrbDespawnTime = 20f;

	public float distanceForIntro = 8f;

	public enum BossState { INTRO, FIGHT, END };
	BossState bossState;


	SpriteRenderer sr;
	Animator animator;
	Healthbar healthSlider;

	float[] distanceToOrbSpawner = new float[4];
	


	int lastAttack = -1;
	int attack = -1;
	bool attackActive = false;

	readonly string PLAYER_TAG = "Player";
	readonly string ORB_TAG = "Orb";

	#endregion


	#region UnityMethods

	protected void Start() {

		

		SwapState(BossState.INTRO);
		
		sr = boss.GetComponent<SpriteRenderer>();
		animator = boss.GetComponent<Animator>();
		healthSlider = healthBar.GetComponent<Healthbar>();

		currentHealth = health;



		if (healthSlider)
		{
			healthSlider.SetMaxHealth(health);
		}
	}

    protected void Update()
    {

    }

    void SwapState(BossState state)
    {

		bossState = state;

		switch (state)
		{
			case BossState.INTRO:
				// Lil Intro Scenario + Camera Pan
				StartCoroutine(CheckPlayerPos());
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

	IEnumerator CheckPlayerPos()
    {
		bool foundPlayer = false;
		do
		{
			yield return new WaitForSeconds(0.2f);

			Collider2D[] collisions = Physics2D.OverlapCircleAll(gameObject.transform.position, distanceForIntro);

			foreach (Collider2D collision in collisions)
			{
				if (collision.transform.parent != null && collision.transform.parent.tag.Equals(PLAYER_TAG))
				{
					FindObjectOfType<AudioManager>().Stop("MusicLevel");
					FindObjectOfType<AudioManager>().Play("MusicBoss");
					foundPlayer = true;
					wall.SetActive(true);
					healthBar.SetActive(true);
					break;
				}
			}

		} while (!foundPlayer);

		// Camera Pan

		yield return new WaitForSeconds(1f);

		SwapState(BossState.FIGHT);

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

		FindObjectOfType<AudioManager>().Play("DamageEnemy");

		// Drop Orb
		SpawnOrb();

		// Get damaged
		currentHealth -= attackDamage;

		// Update Healthbar
		if (healthSlider)
		{
			healthSlider.SetHealth(currentHealth);
		}

		
		if (currentHealth <= 0)
		{
			// Disable healthbar
			healthBar.SetActive(false);

			// Destroy Object
			GetDestroyed();
		}
		else
		{
			StartCoroutine(ChangeColor());
		}
	}

	IEnumerator ChangeColor()
    {
		Color tempColor = Color.white;
		Color damageColor = Color.red;
		sr.color = damageColor;
		yield return new WaitForSeconds(colorChangeDuration);
		sr.color = tempColor;
	}

	void GetDestroyed()
    {
		FindObjectOfType<AudioManager>().Stop("MusicBoss");

		// Create Effect
		Instantiate(killedObj, gameObject.transform.position, Quaternion.identity);

		Destroy(endEnemy);

		// Destroy Object
		Destroy(gameObject);
	}


	IEnumerator regularOrbSpawn()
    {

		while (bossState == BossState.FIGHT)
        {
			yield return new WaitForSeconds(regularOrbSpawnTime);

			SpawnOrb();
		}
    }

	void SpawnOrb()
    {
		Vector2 spawnPos = new Vector2();
		int spawnNr = -1;

		bool found = false;
		bool closestFlag = false;
		int counter = 0;
		do
		{

			found = false;
			closestFlag = true;
			counter++;

			spawnNr = Random.Range(0, 4);

			Collider2D[] colliders = Physics2D.OverlapPointAll(orbSpawners[spawnNr].transform.position);
			foreach (Collider2D collider in colliders)
			{
				if (collider.gameObject.tag.Equals(ORB_TAG))
				{
					found = true;
				}
			}

			if (!found)
			{
				// Orb cant spawn at the closest position
				for (int i = 0; i < orbSpawners.Length; i++)
                {
					distanceToOrbSpawner[i] = Vector2.Distance(orbSpawners[i].transform.position, player.transform.position);
				}

				for (int i = 0; i < orbSpawners.Length; i++)
				{
					if (distanceToOrbSpawner[spawnNr] > distanceToOrbSpawner[i])
                    {
						closestFlag = false;
					}
				}
			}

			if (closestFlag)
            {
				found = true;
            }

			if (!found)
            {
				spawnPos = orbSpawners[spawnNr].transform.position;
				GameObject orb = Instantiate(orbs[Random.Range(0, 4)], spawnPos, Quaternion.identity);
				orb.GetComponent<Orb>().SetTime(regularOrbDespawnTime);
			}

		} while (found && counter < 20);
	} 


	void AbilityLaser()
    {
		Instantiate(laserAbility, boss.transform.position - new Vector3(0, 1.6f, 0), Quaternion.identity, boss.transform);
    }

	void AbilityOrb()
    {
		Instantiate(orbAbility, boss.transform.position - new Vector3(0,1.6f,0), Quaternion.identity, boss.transform);
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
