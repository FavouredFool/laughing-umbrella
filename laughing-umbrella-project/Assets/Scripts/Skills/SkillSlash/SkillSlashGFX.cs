using UnityEngine;

public class SkillSlashGFX : MonoBehaviour {

    #region Variables
    GameObject parent;
    #endregion

    #region UnityMethods

    public void Start()
    {
        parent = transform.parent.gameObject;
    }

    public void PlayAnimation()
    {
        // Entkoppeln, sodass Schwert sich nicht mitbewegt
		transform.parent.parent = null;

        // Animation abspielen
        GetComponent<Animator>().Play("Ability_Slash");
	}
    
    public void CreateHitbox()
    {
        // Aufgerufen in Animation
        parent.GetComponent<SkillSlash>().CreateHitbox();
    }

	public void CleanUp()
    {
		// Wird Aufgerufen bei Animation-End
		Destroy(gameObject.transform.parent.gameObject);
    }
    
	#endregion
}
