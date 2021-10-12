using UnityEngine;
using System.Collections;

public class FlowerScript : MonoBehaviour {

	#region Variables

	public float timeUntilBlossom = 1f;
	public float timeBetweenWaves = 2f;
	public float timeActiveDangerous = 1f;

	public Sprite cropFlower;
	public Sprite blossomedFlower;


	GameObject[] flowerPatterns = new GameObject[3];


	readonly string FLOWERPATTERN_TAG = "FlowerPattern";
	#endregion
	
	
	#region UnityMethods

    protected void Start() {

		// Establish FlowerPatterns
		int counter = 0;
		foreach (Transform child in GameObject.FindGameObjectWithTag(FLOWERPATTERN_TAG).transform)
        {
			flowerPatterns[counter] = child.gameObject;
			counter++;
		}


		StartCoroutine(ActiveAbility());
	}

	IEnumerator ActiveAbility()
    {
		
		yield return StartCoroutine(SpawnPattern(flowerPatterns[0]));

		yield return new WaitForSeconds(timeBetweenWaves);

		yield return StartCoroutine(SpawnPattern(flowerPatterns[1]));

		yield return new WaitForSeconds(timeBetweenWaves);

		yield return StartCoroutine(SpawnPattern(flowerPatterns[2]));


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
		}

		yield return new WaitForSeconds(timeUntilBlossom);

		foreach (Transform flower in flowerPattern.transform)
        {
			flower.GetComponent<SpriteRenderer>().sprite = blossomedFlower;
        }

		yield return new WaitForSeconds(timeActiveDangerous);

		flowerPattern.SetActive(false);
    }

	#endregion
}
