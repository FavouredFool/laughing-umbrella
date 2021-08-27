using UnityEngine;

public class SkillEmpty : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Player hat keinen Skill");
        CleanUp();
    }

    public void CleanUp()
    {
        // Bisher kein Cleanup nötig
    }

}