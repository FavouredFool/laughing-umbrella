using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLogic : MonoBehaviour {

	#region Variables
	public GameObject enemiesObj;
	public GameObject stairs;
	public GameObject HUD;

	public static bool gameIsPaused = false;

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

		if (Input.GetKeyDown(KeyCode.Escape))
        {
			if(gameIsPaused)
            {
				Resume();
				
            } else
            {
				Pause();
				
            }
        }
        
		
    }


	protected void spawnStairs()
    {
		stairs.SetActive(true);
    }

	void Resume()
    {
		Time.timeScale = 1f;
		HUD.SetActive(true);
		SceneManager.UnloadSceneAsync(SceneManager.sceneCountInBuildSettings - 2);
		gameIsPaused = false;

		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	void Pause()
    {
		Time.timeScale = 0f;
		HUD.SetActive(false);
		Scene pauseScene = SceneManager.GetSceneByBuildIndex(SceneManager.sceneCountInBuildSettings - 2);
		SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 2, LoadSceneMode.Additive);
		gameIsPaused = true;

		SceneManager.sceneUnloaded += OnSceneUnloaded;
		
	}

	void OnSceneUnloaded(Scene pause)
    {
		Time.timeScale = 1f;
		HUD.SetActive(true);
		gameIsPaused = false;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}
	
	#endregion
}
