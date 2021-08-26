using UnityEngine;

public class SkillBallYellow : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Gelber Ball wird geschossen.");
        cleanUp();
    }

    public void cleanUp()
    {
        Destroy(gameObject);
    }

}