using UnityEngine;

public class SkillBat : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Fledermäuse fliegen");
        CleanUp();
    }

    public void CleanUp()
    {
        Destroy(gameObject);
    }

}