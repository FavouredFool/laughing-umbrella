using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	#region Variables
	public GameObject player;
	#endregion
	
	
	#region UnityMethods

	public void PlayGame()
    {
		PlayerValues.health = player.GetComponent<PlayerActions>().maxHealth;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

	public void QuitGame()
    {
		Debug.Log("QUIT");
		//UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
    }

	public void RetryGame()
    {
		PlayerValues.health = player.GetComponent<PlayerActions>().maxHealth;
		SceneManager.LoadScene(1);
    }

	public void ToMenu()
    {
		SceneManager.LoadScene(0);
	}
	
	#endregion
}
