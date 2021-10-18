using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {

	#region Variables
	public GameObject player;
	#endregion
	
	
	#region UnityMethods

	public void PlayGame()
    {

		FindObjectOfType<AudioManager>().Play("testSound");

		MainScript.health = player.GetComponent<PlayerActions>().maxHealth;
		MainScript.maxHealth = player.GetComponent<PlayerActions>().maxHealth;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		

		StartCoroutine(tested());
    }

	IEnumerator tested ()
    {
		yield return new WaitForSeconds(2);
		
	}

	public void QuitGame()
    {
		Debug.Log("QUIT");
		//UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
    }

	public void RetryGame()
    {
		MainScript.maxHealth = player.GetComponent<PlayerActions>().maxHealth;
		MainScript.health = player.GetComponent<PlayerActions>().maxHealth;
		SceneManager.LoadScene(1);
    }

	public void ToMenu()
    {
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
