using UnityEngine;

public class SkillBallGreen : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Gruener Ball wird geschossen.");
        cleanUp();
    }

    public void cleanUp()
    {
        Destroy(gameObject);
    }

}