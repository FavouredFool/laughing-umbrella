using UnityEngine;

public class MageActions : MonoBehaviour {

	#region Variables
	public GameObject target;
	public GameObject fireball;


	public float initialWaittime = 2f;
	public float repeatActions = 6f;
	public float fireTpWait = 3f;

	public float fireballSpeed = 2f;
	public int fireballDamage = 1;

	GameObject thrownFireball;

	#endregion

	#region UnityMethods

	void Start()
    {

		InvokeRepeating("doAction", initialWaittime, repeatActions);
    }
	void doAction()
	{
		// 1. Fire fireball on target

		thrownFireball = Instantiate(fireball, gameObject.transform.position, Quaternion.identity);

		Vector2 directionToPlayer = target.transform.position - gameObject.transform.position;

		thrownFireball.GetComponent<Fireball>().SetValues(directionToPlayer, fireballSpeed, fireballDamage);
		


		// 2. wait
		// 3. tp somewhere else


	}

	
	
	
	#endregion
}
