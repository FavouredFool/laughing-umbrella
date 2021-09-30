using UnityEngine;

public class MainScript : MonoBehaviour {

	#region Variables
	// Static variable projectwide
	public static bool gameIsPaused = false;
	#endregion


	#region UnityMethods

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public static void LoadMain()
    {
		GameObject main = GameObject.Instantiate(Resources.Load("Prefabs/Main")) as GameObject;
		GameObject.DontDestroyOnLoad(main);
    }
	
	#endregion
}
