using UnityEngine;

public class SkillSlashGFX : MonoBehaviour {

    #region Variables

    #endregion

    #region UnityMethods


    public void PlayAnimation()
    {
        // Entkoppeln, sodass Schwert sich nicht mitbewegt
		//transform.parent.parent = null;

        // Animation abspielen
        GetComponent<Animator>().Play("Ability_Slash");
	}
    
    public void CreateHitbox()
    {
        // Aufgerufen in Animation
        transform.parent.GetComponent<SkillSlash>().CreateHitbox();
    }

	public void CleanUp()
    {
		Destroy(gameObject);
    }
    
	#endregion
}
