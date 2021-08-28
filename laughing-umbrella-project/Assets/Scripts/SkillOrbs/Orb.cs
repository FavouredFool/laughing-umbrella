using UnityEngine;

public class Orb : MonoBehaviour {


    #region Variables
    public SkillEnum.Skill skillEnum;

    // Höhere Zahl -> Langsamer
    float CREATEANIMATIONSPEED = 25;
    float MOVEMENTANIMATIONSPEED = 0.5f;

    // Höhere Zahl -> geringere Amplitude
    float MOVEMENTANIMATIONAPLITUDE = 10;
        
    Vector3 maxScale;
    Vector3 startPoint;
    #endregion

    

    #region UnityMethods

    public void Start()
    {
        maxScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);

        startPoint = transform.position;

    }
    public void Update()
    {
        if (transform.localScale.x < maxScale.x)
        {
            transform.localScale += maxScale * 1/CREATEANIMATIONSPEED;
        } else
        {
            transform.position = startPoint + new Vector3(0, Mathf.Sin(Time.time/MOVEMENTANIMATIONSPEED)/MOVEMENTANIMATIONAPLITUDE, 0);
        }
        
    }


    #endregion
}
