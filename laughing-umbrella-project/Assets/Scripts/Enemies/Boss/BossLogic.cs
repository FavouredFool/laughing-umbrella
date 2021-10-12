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
				//switch (Random.Range(0, 3))
				switch(1)
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
		Instantiate(flowerAbility, boss.transform);
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
