using UnityEngine;

public class SkillBat : MonoBehaviour, ISkill
{

    public void UseSkill()
    {
        Debug.Log("Flederm�use fliegen");
        CleanUp();
    }

    public void CleanUp()
    {
        Destroy(gameObject);
    }

}