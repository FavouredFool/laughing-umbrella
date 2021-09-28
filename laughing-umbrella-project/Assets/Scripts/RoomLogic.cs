using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLogic : MonoBehaviour {

	#region Variables
	public GameObject enemiesObj;
	public GameObject stairs;

	#endregion
	
	
	#region UnityMethods

    protected void Start() {
		stairs.SetActive(false);
    }

    protected void Update() {

		if (enemiesObj.transform.childCount == 0)
        {
			spawnStairs();
        }
        
		
    }


	protected void spawnStairs()
    {
		stairs.SetActive(true);
    }
	
	#endregion
}
