using UnityEngine;

public class SkillBallGreen : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Gruener Ball wird geschossen.");
        CleanUp();
    }

    public void CleanUp()
    {
        Destroy(gameObject);
    }

}