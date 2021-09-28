using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

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
	
	#endregion
}
