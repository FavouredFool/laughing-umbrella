using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLogic : MonoBehaviour {

	#region Variables
	public GameObject enemiesObj;
	public GameObject stairs;
	public GameObject HUD;

	bool stairsActive = false;

	

	#endregion
	
	
	#region UnityMethods

    protected void Start() {
		stairs.SetActive(false);
    }

    protected void Update() {

		if (enemiesObj.transform.childCount == 0 && !stairsActive)
        {
			spawnStairs();
			stairsActive = true;
        }

		if (Input.GetKeyDown(KeyCode.Escape))
        {
			if(MainScript.gameIsPaused)
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
		FindObjectOfType<AudioManager>().Play("RoomCleared");
		stairs.SetActive(true);
    }

	void Resume()
    {
		Time.timeScale = 1f;
		HUD.SetActive(true);
		SceneManager.UnloadSceneAsync(SceneManager.sceneCountInBuildSettings - 2);
		MainScript.gameIsPaused = false;

		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	void Pause()
    {
		Time.timeScale = 0f;
		HUD.SetActive(false);
		Scene pauseScene = SceneManager.GetSceneByBuildIndex(SceneManager.sceneCountInBuildSettings - 2);
		SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 2, LoadSceneMode.Additive);
		MainScript.gameIsPaused = true;

		SceneManager.sceneUnloaded += OnSceneUnloaded;
		
	}

	void OnSceneUnloaded(Scene pause)
    {
		Time.timeScale = 1f;
		if (HUD != null)
        {
			HUD.SetActive(true);
		}
		
		MainScript.gameIsPaused = false;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}
	
	#endregion
}
