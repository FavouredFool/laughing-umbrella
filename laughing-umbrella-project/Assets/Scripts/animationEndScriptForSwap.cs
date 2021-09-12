using UnityEngine;

public class animationEndScriptForSwap : MonoBehaviour {

	#region Variables
	public GameObject HUD;
	#endregion
	
	
	#region UnityMethods

	public void AnimationEnd()
    {
		HUD.GetComponent<SkillDisplay>().SwapSkillsEnd();
	}
	
	#endregion
}
