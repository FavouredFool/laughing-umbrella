using UnityEngine;

public class MainScript : MonoBehaviour {

	#region Variables
	// Static variable projectwide
	public static bool gameIsPaused = false;
	public static int health { get; set; }
	public static int maxHealth { get; set; }

	public static float startTime { get; set; }

	public static float totalTime { get; set; }
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
	
	#endregion
}
