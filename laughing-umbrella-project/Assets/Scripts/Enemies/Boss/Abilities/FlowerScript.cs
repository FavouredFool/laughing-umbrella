using UnityEngine;
using System.Collections;

public class FlowerScript : MonoBehaviour {

	#region Variables

	public int damage;
	public float knockbackStrength;

	public int waveAmount = 5;

	public float timeUntilBlossom = 1f;
	public float timeBetweenWaves = 2f;
	public float timeActiveDangerous = 1f;

	public Sprite cropFlower;
	public Sprite blossomedFlower;

	GameObject[] flowerPatterns = new GameObject[3];

	bool fadeIn = false;
	GameObject fadeInPattern;
	float fadeInStart;
	float fadeInInter = 0;


	readonly string FLOWERPATTERN_TAG = "FlowerPattern";
	#endregion
	
	
	#region UnityMethods

    protected void Start() {

		// Establish FlowerPatterns
		int counter = 0;
		foreach (Transform child in GameObject.FindGameObjectWithTag(FLOWERPATTERN_TAG).transform)
        {
			flowerPatterns[counter] = child.gameObject;
			foreach(Transform flower in flowerPatterns[counter].transform)
            {
				flower.GetComponent<FlowerInstanceScript>().SetValues(damage, knockbackStrength);
				flower.GetComponent<BoxCollider2D>().enabled = false;
			}
			
			counter++;
		}


		StartCoroutine(ActiveAbility());
	}

    protected void Update()
    {
		if (fadeIn)
		{
			fadeInInter = (Time.time - fadeInStart) / (timeUntilBlossom / 4);
			foreach (Transform flower in fadeInPattern.transform)
			{
				flower.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, fadeInInter);
			}
		}

	}

    IEnumerator ActiveAbility()
    {

		// Angriffe bestimmen
		int[] allAttacks = new int[waveAmount];
		int lastAttack = -1;
		int rolledAttack = -1;

		bool test0 = true;
		bool test1 = true;
		bool test2 = true;

		do
		{
			for (int i = 0; i < waveAmount; i++)
			{
				do
				{
					rolledAttack = Random.Range(0, 3);
					allAttacks[i] = rolledAttack;

				} while (rolledAttack == lastAttack);

				lastAttack = rolledAttack;
			}

			// Endtest ob alle Versionen mindestens ein mal drin sind
			
			foreach (int j in allAttacks)
			{

				switch (j)
				{
					case 0:
						test0 = false;
						break;
					case 1:
						test1 = false;
						break;
					case 2:
						test2 = false;
						break;
				}
			}
		} while (test0 || test1 || test2);




		for (int i = 0; i < waveAmount; i++)
        {
			yield return StartCoroutine(SpawnPattern(flowerPatterns[allAttacks[i]]));

			yield return new WaitForSeconds(timeBetweenWaves);
		}

		CleanUp();
    }

	void CleanUp()
	{
		transform.parent.parent.GetComponent<BossLogic>().SetAttackActive(false);
		Destroy(gameObject);
	}

	IEnumerator SpawnPattern(GameObject flowerPattern)
    {
		flowerPattern.SetActive(true);

		foreach (Transform flower in flowerPattern.transform)
		{
			flower.GetComponent<SpriteRenderer>().sprite = cropFlower;
			flower.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
		}

		fadeIn = true;
		fadeInPattern = flowerPattern;
		fadeInStart = Time.time;

		yield return new WaitForSeconds(timeUntilBlossom);
		fadeIn = false;

		foreach (Transform flower in flowerPattern.transform)
        {
			flower.GetComponent<BoxCollider2D>().enabled = true;
			flower.GetComponent<SpriteRenderer>().sprite = blossomedFlower;
        }

		yield return new WaitForSeconds(timeActiveDangerous);

		foreach (Transform flower in flowerPattern.transform)
		{
			flower.GetComponent<BoxCollider2D>().enabled = false;
		}

		flowerPattern.SetActive(false);
    }



	#endregion
}
