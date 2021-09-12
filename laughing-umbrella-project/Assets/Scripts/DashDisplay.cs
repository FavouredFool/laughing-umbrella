using UnityEngine;
using UnityEngine.UI;

public class DashDisplay : MonoBehaviour {

	#region Variables

	public GameObject player;

	public Image dashOne;
	public Image dashTwo;

	int dashCount;

	#endregion
	
	
	#region UnityMethods

    protected void Start() {
        
    }

    protected void Update() {

		if (player)
        {
			dashCount = player.GetComponent<PlayerActions>().getDashCount();

			switch (dashCount)
			{
				case 0:
					dashOne.enabled = false;
					dashTwo.enabled = false;
					break;
				case 1:
					dashOne.enabled = true;
					dashTwo.enabled = false;
					break;
				case 2:
					dashOne.enabled = true;
					dashTwo.enabled = true;
					break;
			}
		}
		
			
		
    }
	
	#endregion
}
