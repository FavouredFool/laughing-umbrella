using UnityEngine;

public class Orb : MonoBehaviour {


    #region Variables
    // Skillenum -> repräsentiert Skills 
    public SkillEnum.Skill skillEnum;
    public float despawnTime = 10f;
    [Range(0f, 1f)]
    public float despawnEffectStart = 0.85f;

    // Konstanten für Sinuswelle
    // Höhere Zahl -> Langsamer
    readonly float createAnimationSpeed = 25;
    readonly float movementAnimationSpeed = 0.5f;

    // Höhere Zahl -> geringere Amplitude
    readonly float movementAnimationAmplitude = 10;
        
    // Variablen
    Vector3 maxScale;
    Vector3 startPoint;

    // Time
    float startTime;

    

    #endregion

    

    #region UnityMethods

    public void Start()
    {
        maxScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);

        startPoint = transform.position;
        startTime = Time.time;

    }
    public void Update()
    {
        if (transform.localScale.x < maxScale.x)
        {
            transform.localScale += maxScale * 1/createAnimationSpeed;
        } else
        {
            transform.position = startPoint + new Vector3(0, Mathf.Sin(Time.time/movementAnimationSpeed)/movementAnimationAmplitude, 0);
        }

        if(Time.time - startTime > despawnTime * despawnEffectStart) {
            // in dem letzten 25% vor Despawn wird Orb kleiner
            // Interpolation
            transform.localScale = Vector3.Lerp(maxScale, Vector3.zero, ((Time.time - startTime) / despawnTime - despawnEffectStart) * 1/(1-despawnEffectStart));
        }

        if(Time.time - startTime > despawnTime)
        {
            // Despawn
            Destroy(gameObject);
        }
        
    }


    #endregion
}
