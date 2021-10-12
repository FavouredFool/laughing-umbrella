using UnityEngine;
using System.Collections;

public class OrbScript : MonoBehaviour {

	#region Variables
	public GameObject orb;
	public int waveAmount = 17;
	public float orbSpawnDistance = 3.5f;
	public float orbSpeed = 3;
	public float timeBetweenWaves = 0.5f;
	public float orbDespawnTime = 10f;

	float angleIncreaseVersion1 = 22.5f;
	float startAngleVersion1 = 90;
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


		for (int i = 0; i < waveAmount; i++)
        {
			float angle = (i * angleIncreaseVersion1) + startAngleVersion1;
			ShootOrb(angle);
			ShootOrb(angle + 180);

			yield return new WaitForSeconds(timeBetweenWaves);
		}
		
		CleanUp();
    }

	IEnumerator ActiveAbilityVersion1()
    {

		for (int i = 0; i < waveAmount; i++)
		{
			float angle = (i * angleIncreaseVersion1) + startAngleVersion1;
			ShootOrb(angle);
			ShootOrb(angle + 180);

			yield return new WaitForSeconds(timeBetweenWaves);
		}
		CleanUp();
	}

	void ShootOrb(float angle)
    {
		GameObject orbInstance = Instantiate(orb);
		orbInstance.transform.position = gameObject.transform.position - new Vector3(0, orbSpawnDistance, 0);
		orbInstance.transform.position = RotatePointAroundPivot(orbInstance.transform.position, gameObject.transform.position, new Vector3(0,0, angle));
		Vector2 direction = orbInstance.transform.position - gameObject.transform.position;
		orbInstance.GetComponent<OrbProjectile>().SetValues(orbSpeed, direction, orbDespawnTime);
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
