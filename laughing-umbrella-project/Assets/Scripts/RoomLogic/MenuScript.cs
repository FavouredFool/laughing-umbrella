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
		FindObjectOfType<AudioManager>().Play("MusicMain");
	}

    public void PlayGame()
	{
		FindObjectOfType<AudioManager>().Stop("MusicMain");
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
	}

	public void UnloadScene()
    {
		SceneManager.UnloadSceneAsync(gameObject.scene);
	}
	
	#endregion
}
