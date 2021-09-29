using UnityEngine;

public class CameraScript : MonoBehaviour {

	#region Variables
	Camera cam;
	#endregion
	
	
	#region UnityMethods

    protected void Start() {
		cam = gameObject.GetComponent<Camera>();
		
		if (Screen.width / Screen.height != 16f/9)
        {
			float startX = 0;
			float startY = 0;
			float width = Screen.width;
			float height = Screen.height;
			Rect camRect;

			// Change cam.rect
			if (Screen.width > Screen.height * 16f/9)
            {
				startX = (Screen.width - (Screen.height * 16f/9))/2;
				width = (Screen.width - (2 * startX)) / Screen.width;
				camRect = new Rect(startX / (Screen.width), 0, width, 1);
			} else
            {
				startY = (Screen.height - (Screen.width * 9f/16)) / 2;
				height = (Screen.height - (2 * startY)) / Screen.height;
				camRect = new Rect(0, startY / (Screen.height), 1, height);
            }

			cam.rect = camRect;
			
		}
		
		

	}

	#endregion
}
