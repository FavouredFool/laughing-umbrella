using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLogic : MonoBehaviour {

	#region Variables
	public GameObject enemiesObj;
	public GameObject stairs;


	enum RoomState { ACTIVE, FINISHED };
	RoomState roomState;


	#endregion
	
	
	#region UnityMethods

    protected void Start() {
		roomState = RoomState.ACTIVE;
		stairs.SetActive(false);
	
        
    }

    protected void Update() {

		if (enemiesObj.transform.childCount == 0)
        {
			roomState = RoomState.FINISHED;
			spawnStairs();
        }
        
		
    }


	protected void spawnStairs()
    {
		stairs.SetActive(true);
    }
	
	#endregion
}
