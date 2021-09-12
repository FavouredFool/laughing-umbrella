using UnityEngine;
using UnityEngine.UI;

public class DashDisplay : MonoBehaviour {

	#region Variables

	public GameObject player;

	public Image dashOne;
	public Image dashTwo;

	public Sprite dashFull;
	public Sprite dashEmpty;

	int dashCount;

    #endregion


    #region UnityMethods

    protected void Start()
    {
		dashOne.sprite = dashEmpty;
		dashTwo.sprite = dashEmpty;
    }

    protected void Update() {

		if (player)
        {
			dashCount = player.GetComponent<PlayerActions>().getDashCount();

			switch (dashCount)
			{
				case 0:
					dashOne.sprite = dashEmpty;
					dashTwo.sprite = dashEmpty;
					break;
				case 1:
					dashOne.sprite = dashFull;
					dashTwo.sprite = dashEmpty;
					break;
				case 2:
					dashOne.sprite = dashFull;
					dashTwo.sprite = dashFull;
					break;
			}
		}
		
			
		
    }
	
	#endregion
}
