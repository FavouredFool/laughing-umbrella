using UnityEngine;

public class SkillBallRed : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Roter Ball wird geschossen.");
        CleanUp();
    }

    public void CleanUp()
    {
        Destroy(gameObject);
    }

}