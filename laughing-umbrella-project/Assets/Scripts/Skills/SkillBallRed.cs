using UnityEngine;

public class SkillBallRed : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Roter Ball wird geschossen.");
        cleanUp();
    }

    public void cleanUp()
    {
        Destroy(gameObject);
    }

}