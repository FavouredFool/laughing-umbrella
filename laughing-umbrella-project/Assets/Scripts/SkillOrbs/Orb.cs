using UnityEngine;

public class Orb : MonoBehaviour {


    #region Variables
    public SkillEnum.Skill skillEnum;

    // Höhere Zahl -> Langsamer
    int CREATEANIMATIONSPEED = 25;
    Vector3 maxScale;
    #endregion

    

    #region UnityMethods

    public void Start()
    {
        maxScale = transform.localScale;

        transform.localScale = new Vector3(0, 0, 0);
    }
    public void Update()
    {
        if (transform.localScale.x < maxScale.x)
        {
            transform.localScale += maxScale * 1/CREATEANIMATIONSPEED;
        }
        
    }


    #endregion
}
