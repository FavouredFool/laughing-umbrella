using UnityEngine;

public class SkillMaceGFX : MonoBehaviour {

    #region Variables
    GameObject parent;
    #endregion

    #region UnityMethods


    public void PlayAnimation()
    {

        // Animation abspielen
        GetComponent<Animator>().Play("Ability_Morgenstern");
    }

    public void CreateHitboxCircle()
    {
        // Aufgerufen in Animation
        transform.parent.GetComponent<SkillMace>().CreateHitboxCircle();
    }

    public void CreateHitboxLine()
    {
        transform.parent.GetComponent<SkillMace>().CreateHitboxLine();
    }

    public void CleanUp()
    {
        // Wird Aufgerufen bei Animation-End
        transform.parent.GetComponent<SkillMace>().CleanUp();
        //Destroy(gameObject);
    }

    #endregion
}
