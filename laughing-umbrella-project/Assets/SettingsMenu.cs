using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

	public AudioMixer audioMixer;
	public Toggle fullScreenToggle;
	public Slider musicSlider;
	public Slider soundSlider;




	// Start is called before the first frame update
	public void Start()
	{

		musicSlider.value = PlayerPrefs.GetFloat("volumeMusic", 0.75f);
		soundSlider.value = PlayerPrefs.GetFloat("volumeSound", 0.75f);


		if (FindObjectOfType<AudioManager>().IsPlaying("MusicMain"))
		{
			FindObjectOfType<AudioManager>().Unpause("MusicMain");

		}
		else
		{
			FindObjectOfType<AudioManager>().Play("MusicMain");
		}

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

	public void SetVolumeMusic(float volume)
    {
		PlayerPrefs.SetFloat("volumeMusic", volume);

		if (volume <= 0.01f)
        {
			audioMixer.SetFloat("volumeMusic", -80);
		} else
        {
			audioMixer.SetFloat("volumeMusic", Mathf.Log10(volume) * 20);
		}
	}

	public void SetVolumeSound(float volume)
	{
		PlayerPrefs.SetFloat("volumeSound", volume);

		if (volume <= 0.01f)
		{
			audioMixer.SetFloat("volumeSound", -80);
		}
		else
		{
			audioMixer.SetFloat("volumeSound", Mathf.Log10(volume) * 20);
		}
	}


	public void SetFullscreen(bool isFullScreen)
    {
		Screen.fullScreen = isFullScreen;
    }

}
