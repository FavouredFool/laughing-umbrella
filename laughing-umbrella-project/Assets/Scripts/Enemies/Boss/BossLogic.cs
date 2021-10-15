using UnityEngine;
using System.Collections;

public class BossLogic : MonoBehaviour {

	#region Variables

	public GameObject boss;

	public GameObject laserAbility;
	public GameObject orbAbility;
	public GameObject flowerAbility;
	
	public float attackDowntime = 3.0f;

	public enum BossState { INTRO, FIGHT, END };
	BossState bossState;


	int lastAttack = -1;
	int attack = -1;
	bool attackActive = false;

	#endregion


	#region UnityMethods

	protected void Start() {
		SwapState(BossState.FIGHT);
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
		GameObject test = Instantiate(flowerAbility, new Vector3(), Quaternion.identity, boss.transform);
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
