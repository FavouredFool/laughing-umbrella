using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

	#region Variables
	public GameObject laser;
	public bool counterclockwise = false;
	[Range(2,4)]
	public int laserAmount;
	public float laserSpawnDistance = 3.5f;

	readonly GameObject[] laserArray = new GameObject[4];

	
	public float moveDuration = 6f;
	public float totalAngle = 360f;

	bool moveLaser = false;

	float angle = 0;

	#endregion
	
	
	#region UnityMethods

    protected void Start() {

		StartCoroutine(ActiveAbility());

		
	}

    protected void Update() {

        if (moveLaser)
        {

			angle = totalAngle / moveDuration * Time.deltaTime;

			if (!counterclockwise)
				angle *= -1;

			transform.RotateAround(gameObject.transform.position, Vector3.forward, angle);
        }
    }

	IEnumerator ActiveAbility()
    {
		// Ablauf: Erst spawnen, dann positionieren, dann rotieren

		// Spawnen
		
		for (int i = 0; i < laserAmount; i++)
        {
			laserArray[i] = Instantiate(laser, gameObject.transform);
		}


		// Positionieren

		laserArray[0].transform.localPosition = new Vector3(laserSpawnDistance, 0, 0);
		laserArray[0].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

		laserArray[1].transform.localPosition = new Vector3(-laserSpawnDistance, 0, 0);
		laserArray[1].transform.localRotation = Quaternion.Euler(new Vector3(0,0,180));

		if (laserAmount > 2)
        {
			laserArray[2].transform.localPosition = new Vector3(0, laserSpawnDistance, 0);
			laserArray[2].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));

			if (laserAmount > 3)
            {
				laserArray[3].transform.localPosition = new Vector3(0, -laserSpawnDistance, 0);
				laserArray[3].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 270));
			}
		}

		yield return new WaitForSeconds(1);

		// rotieren
		moveLaser = true;

		yield return new WaitForSeconds(moveDuration);

		// Endpunkt des Lasers
		moveLaser = false;
		yield return new WaitForSeconds(1);

		CleanUp();

	}


	void CleanUp()
    {
		transform.parent.parent.GetComponent<BossLogic>().SetAttackActive(false);
		Destroy(gameObject);
    }
	
	#endregion
}
