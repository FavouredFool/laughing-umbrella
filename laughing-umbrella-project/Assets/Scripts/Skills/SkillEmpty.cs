using UnityEngine;

public class SkillEmpty : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Player hat keinen Skill");
    }

}