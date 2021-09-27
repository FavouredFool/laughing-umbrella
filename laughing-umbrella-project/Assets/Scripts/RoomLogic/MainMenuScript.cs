using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	#region Variables
	
	#endregion
	
	
	#region UnityMethods

	public void PlayGame()
    {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

	public void QuitGame()
    {
		Debug.Log("QUIT");
		UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
    }
	
	#endregion
}
