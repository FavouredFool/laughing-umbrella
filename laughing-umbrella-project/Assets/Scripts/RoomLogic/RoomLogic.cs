using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomLogic : MonoBehaviour {

	#region Variables
	public GameObject enemiesObj;
	public GameObject stairs;
	public GameObject HUD;
	public bool healRoom;

	bool stairsActive = false;

	

	#endregion
	
	
	#region UnityMethods

    protected void Start() {
		stairs.SetActive(false);

		// BackgroundMusic
		AudioManager audioManager = FindObjectOfType<AudioManager>();
		if (!audioManager.IsPlaying("MusicLevel") && !audioManager.IsPlaying("MusicMain"))
        {
			if (healRoom)
            {
				audioManager.Play("MusicMain");
			} else
            {
				audioManager.Play("MusicLevel");
			}
			
        } else
        {
			if (healRoom)
			{
				audioManager.Pause("MusicLevel");

				if (audioManager.IsPlaying("MusicMain"))
					audioManager.Unpause("MusicMain");
				else 
					audioManager.Play("MusicMain");
			}
			if (!healRoom)
			{
				if (audioManager.IsPlaying("MusicLevel"))
					audioManager.Unpause("MusicLevel");
				else
					audioManager.Play("MusicLevel");


				audioManager.Pause("MusicMain");
			}
		}

		

    }

    protected void Update() {

		if (enemiesObj == null || (enemiesObj.transform.childCount == 0 && !stairsActive))
        {
			spawnStairs();
			stairsActive = true;
        }

		if (Input.GetKeyDown(KeyCode.Escape))
        {
			if(MainScript.gameIsPaused)
            {
				Resume();
				FindObjectOfType<AudioManager>().Unpause("MusicLevel");
				FindObjectOfType<AudioManager>().Unpause("MusicBoss");
				FindObjectOfType<AudioManager>().Pause("MusicMain");

			} else
            {
				Pause();
				FindObjectOfType<AudioManager>().Pause("MusicLevel");
				FindObjectOfType<AudioManager>().Pause("MusicBoss");

			}
        }
        
		
    }


	protected void spawnStairs()
    {
		if (enemiesObj != null)
        {
			FindObjectOfType<AudioManager>().Play("RoomCleared");
		}

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
		if (FindObjectOfType<AudioManager>() != null)
        {
			FindObjectOfType<AudioManager>().Play("ButtonClick");
			FindObjectOfType<AudioManager>().Unpause("MusicLevel");
			FindObjectOfType<AudioManager>().Unpause("MusicBoss");
			FindObjectOfType<AudioManager>().Pause("MusicMain");

		}
		

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
