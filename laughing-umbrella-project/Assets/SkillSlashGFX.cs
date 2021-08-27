using UnityEngine;

public class SkillSlashGFX : MonoBehaviour {

	#region Variables
	
	#endregion


	#region UnityMethods

	public void PlayAnimation()
    {
		// Entkoppeln, sodass Schwert sich nicht mitbewegt
		transform.parent = null;

		// Animation abspielen
		GetComponent<Animator>().Play("Ability_Slash");
	}

	public void cleanUp()
    {
		// Wird Aufgerufen bei Animation-End
		Destroy(gameObject);
    }

	#endregion
}
