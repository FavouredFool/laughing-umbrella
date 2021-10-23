using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {

	#region Variables
	public GameObject player;
    #endregion


    #region UnityMethods

    public void Start()
    {
		if (FindObjectOfType<AudioManager>().IsPlaying("MusicMain"))
        {
			FindObjectOfType<AudioManager>().Unpause("MusicMain");

		} else
        {
			FindObjectOfType<AudioManager>().Play("MusicMain");
		}
		
	}

    public void PlayGame()
	{
		FindObjectOfType<AudioManager>().Pause("MusicMain");
		FindObjectOfType<AudioManager>().Play("MusicLevel");
		FindObjectOfType<AudioManager>().Play("ButtonClick");
		
		MainScript.health = player.GetComponent<PlayerActions>().maxHealth;
		MainScript.maxHealth = player.GetComponent<PlayerActions>().maxHealth;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void HoverButton()
    {
		FindObjectOfType<AudioManager>().Play("ButtonHover");
	}

	public void QuitGame()
    {
		FindObjectOfType<AudioManager>().Play("ButtonClick");
		Debug.Log("QUIT");
		//UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
    }

	public void RetryGame()
    {

		FindObjectOfType<AudioManager>().Pause("MusicMain");
		FindObjectOfType<AudioManager>().Play("MusicLevel");

		FindObjectOfType<AudioManager>().Play("ButtonClick");
		MainScript.maxHealth = player.GetComponent<PlayerActions>().maxHealth;
		MainScript.health = player.GetComponent<PlayerActions>().maxHealth;
		SceneManager.LoadScene(1);
    }

	public void ToMenu()
    {
		FindObjectOfType<AudioManager>().Play("ButtonClick");
		MainScript.gameIsPaused = false;
		Time.timeScale = 1f;
		SceneManager.LoadScene(0);

		// Reset backgroundMusic
		if (FindObjectOfType<AudioManager>().IsPlaying("MusicLevel"))
		{
			FindObjectOfType<AudioManager>().Stop("MusicLevel");
		}

		if (FindObjectOfType<AudioManager>().IsPlaying("MusicBoss"))
		{
			FindObjectOfType<AudioManager>().Stop("MusicBoss");
		}
	}


	public void ToOptions()
    {
		FindObjectOfType<AudioManager>().Play("ButtonClick");
		SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 3);
	}


	public void UnloadScene()
    {
		SceneManager.UnloadSceneAsync(gameObject.scene);
	}

	
	#endregion
}
