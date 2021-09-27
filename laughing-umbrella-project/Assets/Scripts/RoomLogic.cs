using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLogic : MonoBehaviour {

	#region Variables
	public GameObject enemiesObj;


	enum RoomState { ACTIVE, FINISHED };
	RoomState roomState;


	#endregion
	
	
	#region UnityMethods

    protected void Start() {
		roomState = RoomState.ACTIVE;
	
        
    }

    protected void Update() {

		if (enemiesObj.transform.childCount == 0)
        {
			roomState = RoomState.FINISHED;
			transitionToNextRoom();
        }
        
		
    }

	protected void transitionToNextRoom()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

	}
	
	#endregion
}
