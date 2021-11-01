using UnityEngine;
using UnityEngine.Audio;

public class MainScript : MonoBehaviour {

	#region Variables
	// Static variable projectwide
	public static bool gameIsPaused = false;
	public static int health { get; set; }
	public static int maxHealth { get; set; }

	public static float startTime { get; set; }

	public static float totalTime { get; set; }

	public AudioMixer audioMixer;
	#endregion


	#region UnityMethods


	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void LoadMain()
    {
		GameObject main = GameObject.Instantiate(Resources.Load("Prefabs/Main")) as GameObject;
		GameObject.DontDestroyOnLoad(main);

		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 300;
    }

    public void Start()
    {
		SetVolumeMusic();
		SetVolumeSound();
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
	#endregion
}
