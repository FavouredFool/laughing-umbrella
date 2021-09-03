using UnityEngine;

public class SkillFire : MonoBehaviour, ISkill {

	#region Variables
	
	#endregion
	
	
	#region UnityMethods

    void Start() {
        
    }

    void Update() {
        
    }

	public void UseSkill()
    {
        Debug.Log("FIREBALL!");
        CleanUp();
    }

	public void CleanUp()
    {
        Destroy(gameObject);
    }
	
	#endregion
}
