using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour {

	#region Variables

	public Image frontImage;
	public Image backImage;
	public Image animationImage;

	public Sprite frontOrb;

	#endregion
	
	
	#region UnityMethods

    void Start() {
		animationImage.enabled = false;
		frontImage.enabled = true;
		backImage.enabled = true;
    }

	public void SetFrontSkill(Sprite frontOrb)
    {
		// Set Image
    }

	public void SwapSkillsStart(Sprite frontOrb, Sprite backOrb)
    {

    }

	void UpdateImages()
    {
		// Set the Skill into the images
    }
	
	#endregion
}
