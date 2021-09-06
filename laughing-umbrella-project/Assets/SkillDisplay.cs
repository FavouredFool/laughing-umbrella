using UnityEngine;
using UnityEngine.UI;

public class SkillDisplay : MonoBehaviour {

	#region Variables

	public GameObject player;

	public Image frontImage;
	public Image backImage;
	public Image animationImage;
	public Image frontOrbHolder;
	public Image backOrbHolder;

	public Sprite orbSlashSprite;
	public Sprite orbBatSprite;
	public Sprite orbFireSprite;

	PlayerSkillUse playerSkill;

	bool activeAnimation;

	#endregion
	
	
	#region UnityMethods

    protected void Start() {
		animationImage.gameObject.SetActive(false);
		frontImage.enabled = true;
		backImage.enabled = true;
		frontOrbHolder.enabled = false;
		backOrbHolder.enabled = false;

		activeAnimation = false;

		playerSkill = player.GetComponent<PlayerSkillUse>();
	}

    protected void Update()
    {
		if (!activeAnimation)
        {
			UpdateImages();

			if (Input.GetMouseButtonDown(1))
			{
				SwapSkillsStart();
			}
		}
		
			

	}

    public void SwapSkillsStart()
    {
		// Disable everything, enable animationImage
		animationImage.gameObject.SetActive(true);
		frontImage.enabled = false;
		backImage.enabled = false;
		frontOrbHolder.enabled = false;
		backOrbHolder.enabled = false;

		activeAnimation = true;

		// Play animation
		animationImage.gameObject.GetComponent<Animator>().SetTrigger("swap");
		
	}

	public void SwapSkillsEnd()
    {
		// Enable everything, disable animationImage
		animationImage.gameObject.SetActive(false);
		frontImage.enabled = true;
		backImage.enabled = true;

		activeAnimation = false;
	}

	void UpdateImages()
    {
		// Set the Skill into the images
		switch (playerSkill.GetActiveSkill().name)
		{
			case "SkillSlash(Clone)":
				frontOrbHolder.sprite = orbSlashSprite;
				break;
			case "SkillBat(Clone)":
				frontOrbHolder.sprite = orbBatSprite;
				break;
			case "SkillFire(Clone)":
				frontOrbHolder.sprite = orbFireSprite;
				break;
			case "SkillEmpty(Clone)":
				frontOrbHolder.sprite = null;
				break;
		}

		switch (playerSkill.GetBackupSkill().name)
		{
			case "SkillSlash(Clone)":
				backOrbHolder.sprite = orbSlashSprite;
				break;
			case "SkillBat(Clone)":
				backOrbHolder.sprite = orbBatSprite;
				break;
			case "SkillFire(Clone)":
				backOrbHolder.sprite = orbFireSprite;
				break;
			case "SkillEmpty(Clone)":
				backOrbHolder.sprite = null;
				break;
		}

		if (frontOrbHolder.sprite)
			frontOrbHolder.enabled = true;
		else
			frontOrbHolder.enabled = false;

		if (backOrbHolder.sprite)
			backOrbHolder.enabled = true;
		else
			backOrbHolder.enabled = false;

		
	}
	
	#endregion
}
