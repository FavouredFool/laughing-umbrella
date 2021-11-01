using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Collections;

public class MenuScript : MonoBehaviour {

	#region Variables
	public GameObject player;
	public AudioMixer audioMixer;
    #endregion


    #region UnityMethods

    public void Start()
    {

		SetVolumeMusic();
		SetVolumeSound();

		if (FindObjectOfType<AudioManager>().IsPlaying("MusicMain"))
        {
			FindObjectOfType<AudioManager>().Unpause("MusicMain");

		} else
        {
			FindObjectOfType<AudioManager>().Play("MusicMain");
		}


		
	}

	public void SetVolumeMusic()
	{
		PlayerPrefs.SetFloat("volumeMusic", 0.75f);
		float volume = 0.75f;

		if (volume <= 0.01f)
		{
			audioMixer.SetFloat("volumeMusic", -80);
		}
		else
		{
			audioMixer.SetFloat("volumeMusic", Mathf.Log10(volume) * 20);
		}
	}

	public void SetVolumeSound()
	{
		PlayerPrefs.SetFloat("volumeSound", 0.75f);
		float volume = 0.75f;

		if (volume <= 0.01f)
		{
			audioMixer.SetFloat("volumeSound", -80);
		}
		else
		{
			audioMixer.SetFloat("volumeSound", Mathf.Log10(volume) * 20);
		}
	}

	public void PlayGame()
	{
		FindObjectOfType<AudioManager>().Stop("MusicMain");
		FindObjectOfType<AudioManager>().Stop("MusicLevel");
		FindObjectOfType<AudioManager>().Stop("MusicBoss");

		FindObjectOfType<AudioManager>().Play("MusicLevel");
		FindObjectOfType<AudioManager>().Play("ButtonClick");
		
		MainScript.health = player.GetComponent<PlayerActions>().maxHealth;
		MainScript.maxHealth = player.GetComponent<PlayerActions>().maxHealth;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

		MainScript.startTime = Time.time;
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

		FindObjectOfType<AudioManager>().Stop("MusicMain");
		FindObjectOfType<AudioManager>().Stop("MusicLevel");
		FindObjectOfType<AudioManager>().Stop("MusicBoss");


		FindObjectOfType<AudioManager>().Play("MusicLevel");
		FindObjectOfType<AudioManager>().Play("ButtonClick");

		MainScript.maxHealth = player.GetComponent<PlayerActions>().maxHealth;
		MainScript.health = player.GetComponent<PlayerActions>().maxHealth;
		SceneManager.LoadScene(1);

		MainScript.startTime = Time.time;
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
