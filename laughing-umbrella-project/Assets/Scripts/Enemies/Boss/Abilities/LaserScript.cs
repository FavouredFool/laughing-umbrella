using UnityEngine;
using System.Collections;

public class LaserScript : MonoBehaviour {

	#region Variables
	public GameObject laser;
	[Range(2,4)]
	public int laserAmount;
	public float laserSpawnDistance = 3.5f;
	public float timeBeforeMoving = 1f;

	readonly GameObject[] laserArray = new GameObject[4];

	
	public float moveDuration = 6f;
	public float totalAngle = 360f;
	public float knockbackStrength = 1.5f;
	public int damage = 1;

	bool counterclockwise = false;

	bool moveLaser = false;
	bool fadeIn = false;
	float fadeInStart;
	float fadeInInter = 0;

	bool fadeOut = false;
	float fadeOutStart;
	float fadeOutInter;

	float angle = 0;

	#endregion
	
	
	#region UnityMethods

    protected void Start() {

		if (Random.Range(0,2) == 0)
        {
			counterclockwise = true;
		} else
        {
			counterclockwise = false;
        }

		StartCoroutine(ActiveAbility());

	}

    protected void Update() {

		if(fadeIn)
        {
			fadeInInter = (Time.time - fadeInStart) / (timeBeforeMoving / 4);
			foreach(GameObject laser in laserArray)
            {
				laser.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, fadeInInter);
            }
		}

		if (fadeOut)
		{
			fadeOutInter = 1 - (Time.time - fadeOutStart) / (timeBeforeMoving / 4);
			foreach (GameObject laser in laserArray)
			{
				laser.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, fadeOutInter);
			}
		}

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

		FindObjectOfType<AudioManager>().Play("BossLaser");

		// Spawnen

		for (int i = 0; i < laserAmount; i++)
        {
			laserArray[i] = Instantiate(laser, gameObject.transform);
			laserArray[i].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
			laserArray[i].GetComponent<BoxCollider2D>().enabled = false;
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

		fadeIn = true;
		fadeInStart = Time.time;
		yield return new WaitForSeconds(timeBeforeMoving/4);
		fadeIn = false;

		// Zeit vor Collider-Aktivierung
		yield return new WaitForSeconds(timeBeforeMoving / 2);

		for (int i = 0; i < laserAmount; i++)
		{
			laserArray[i].GetComponent<BoxCollider2D>().enabled = true;
		}

		yield return new WaitForSeconds(timeBeforeMoving / 4);



		// rotieren
		moveLaser = true;

		yield return new WaitForSeconds(moveDuration);

		// Endpunkt des Lasers
		moveLaser = false;
		yield return new WaitForSeconds(timeBeforeMoving*(3f/4));

		fadeOut = true;
		fadeOutStart = Time.time;
		yield return new WaitForSeconds(timeBeforeMoving / 4);
		fadeOut = false;

		CleanUp();

	}


	void CleanUp()
    {
		FindObjectOfType<AudioManager>().Stop("BossLaser");
		transform.parent.parent.GetComponent<BossLogic>().SetAttackActive(false);
		Destroy(gameObject);
    }
	
	#endregion
}
