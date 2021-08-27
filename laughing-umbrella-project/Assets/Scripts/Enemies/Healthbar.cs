using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {


	#region Variables
	public Slider slider;
	#endregion


	#region UnityMethods

	public void SetMaxHealth(int health)
    {
		slider.maxValue = health;
		slider.value = health;
    }
	public void SetHealth(int health)
    {
		slider.value = health;
    }

	#endregion
}
