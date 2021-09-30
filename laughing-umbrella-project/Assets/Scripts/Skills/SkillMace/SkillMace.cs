using UnityEngine;

public class SkillMace : MonoBehaviour, ISkill {

	#region Variables
	
	#endregion
	
	
	#region UnityMethods


	public void UseSkill()
    {
        Debug.Log("Mace-Attack!");
        CleanUp();
    }

	public void CleanUp()
    {
        Destroy(gameObject);
    }
	
	#endregion
}
