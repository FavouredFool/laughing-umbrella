using UnityEngine;
using System.Collections;

public class OrbScript : MonoBehaviour {

	#region Variables
	public GameObject orb;

	public int damage = 1;
	public float knockbackStrength = 3f;

	public float orbSpawnTime = 0.3f;
	public float orbSpawnDistance = 3.5f;
	public float orbSpeed = 3;
	public float orbDespawnTime = 10f;

	public int waveAmountVersion1 = 17;
	public float timeBetweenWavesVersion1 = 0.5f;
	float angleIncreaseVersion1 = 22.5f;
	float startAngleVersion1 = 90;

	public int waveAmountVersion2 = 8;
	public float timeBetweenWavesVersion2 = 0.5f;
	public float angleIncreaseVersion2 = 22.5f;


	float angle;
	#endregion


	#region UnityMethods

	protected void Start() {

		// Decide on which ability will be used

		switch (Random.Range(0,2))
        {
			case 0:
				StartCoroutine(ActiveAbilityVersion0());
				break;
			case 1:
				StartCoroutine(ActiveAbilityVersion1());
				break;
		}

		
    }


	IEnumerator ActiveAbilityVersion0()
    {
		// Version1
		// Clockwise vs Counterclockwise
		switch (Random.Range(0, 2))
		{
			case 0:
				angleIncreaseVersion1 *= -1;
				break;
		}


		for (int i = 0; i < waveAmountVersion1; i++)
        {
			FindObjectOfType<AudioManager>().Play("BossEnergyball");
			angle = (i * angleIncreaseVersion1) + startAngleVersion1;
			ShootOrb(angle);
			ShootOrb(angle + 180);
			ShootOrb(angle + 90);
			ShootOrb(angle + 270);

			yield return new WaitForSeconds(timeBetweenWavesVersion1);
		}
		
		CleanUp();
    }

	IEnumerator ActiveAbilityVersion1()
    {

		for (int i = 0; i < waveAmountVersion2; i++)
		{
			angle = i * angleIncreaseVersion2;

			for (int j = 0; j<2; j++)
            {
				FindObjectOfType<AudioManager>().Play("BossEnergyball");
				for (int k = 0; k<8; k++)
                {
					ShootOrb(angle + k*45);
				}
				

				yield return new WaitForSeconds(timeBetweenWavesVersion2);
			}

			yield return new WaitForSeconds(timeBetweenWavesVersion2);
		}

		CleanUp();
	}

	void ShootOrb(float angle)
    {
		GameObject orbInstance = Instantiate(orb);
		orbInstance.transform.position = gameObject.transform.position - new Vector3(0, orbSpawnDistance, 0);
		orbInstance.transform.position = RotatePointAroundPivot(orbInstance.transform.position, gameObject.transform.position, new Vector3(0,0, angle));
		Vector2 direction = orbInstance.transform.position - gameObject.transform.position;
		orbInstance.GetComponent<OrbProjectile>().SetValues(orbSpeed, direction, orbDespawnTime, damage, knockbackStrength, orbSpawnTime);
	}

	void CleanUp()
	{
		transform.parent.parent.GetComponent<BossLogic>().SetAttackActive(false);
		Destroy(gameObject);
	}

	protected Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 dir = point - pivot;
		dir = Quaternion.Euler(angles) * dir;
		point = dir + pivot;
		return point;
	}

	#endregion
}
