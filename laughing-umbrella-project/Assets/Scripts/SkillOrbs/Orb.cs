using UnityEngine;

public class Orb : MonoBehaviour {


    #region Variables

    // Skillenum -> repräsentiert Skills 
    public SkillEnum.Skill skillEnum;
    public float despawnTime = 10f;
    [Range(0f, 1f)]
    public float despawnEffectStart = 0.85f;
    public float magneticRadius = 1f;
    public float magneticSpeed = 50f;
    // Konstanten für Sinuswelle
    // Höhere Zahl -> Langsamer
    readonly float createAnimationSpeed = 25;
    readonly float movementAnimationSpeed = 0.5f;

    // Höhere Zahl -> geringere Amplitude
    readonly float movementAnimationAmplitude = 10;
    readonly string PLAYER_TAG = "Player";

    // Variablen
    Vector3 maxScale;
    Vector3 startPoint;

    // Time
    float startTime;

    // Components
    CircleCollider2D cCollider;

    // Enum
    enum OrbState { SPAWNING, FLOATING, DRAGGED, DESPAWNING };
    OrbState orbState;

    

    #endregion

    

    #region UnityMethods

    public void Start()
    {
        maxScale = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);

        startPoint = transform.position;
        startTime = Time.time;

        cCollider = GetComponent<CircleCollider2D>();

        orbState = OrbState.SPAWNING;

    }
    public void Update()
    {

        switch (orbState) {

        case OrbState.SPAWNING:
            transform.localScale += maxScale * 1 / createAnimationSpeed;

            if (transform.localScale.x >= maxScale.x)
            {
                orbState = OrbState.FLOATING;
            }
            break;
        case OrbState.FLOATING:
            transform.position = startPoint + new Vector3(0, Mathf.Sin(Time.time / movementAnimationSpeed) / movementAnimationAmplitude, 0);

            if (Time.time - startTime > despawnTime * despawnEffectStart)
            {
                orbState = OrbState.DESPAWNING;
            }
            break;
        case OrbState.DESPAWNING:
            // in dem letzten 25% vor Despawn wird Orb kleiner
            // Interpolation
            transform.localScale = Vector3.Lerp(maxScale, Vector3.zero, ((Time.time - startTime) / despawnTime - despawnEffectStart) * 1 / (1 - despawnEffectStart));

            if (Time.time - startTime > despawnTime)
            {
                // Despawn
                Destroy(gameObject);
            }
            break;
        }

        // Magnetisierung
        if (!(orbState == OrbState.SPAWNING))
        {
            // Magnetisierung
            // Wenn Spieler in Gegend entdeckt wird, wird sich auf ihn zu bewegt über "moveTowards"
            Collider2D[] allColliders = Physics2D.OverlapCircleAll(cCollider.transform.position, magneticRadius);
            OrbState tempOrbState = orbState;
            bool found = false;

            foreach (Collider2D activeCollider in allColliders)
            {
                if (activeCollider.transform.parent != null && activeCollider.transform.parent.tag == PLAYER_TAG)
                {
                    if (activeCollider.transform.parent.GetComponent<PlayerSkillUse>().GetActiveSkill() == activeCollider.transform.parent.GetComponent<PlayerSkillUse>().GetEmptySkill())
                    {
                        found = true;
                        orbState = OrbState.DRAGGED;
                        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, activeCollider.transform.position, magneticSpeed * Time.deltaTime);
                    }
                    break;
                }
            }

            if (!found)
            {
                // Keinen Player gefunden
                orbState = tempOrbState;
            }
        }
    }

    public void SetTime(float despawnTime)
    {
        this.despawnTime = despawnTime;
    }



    #endregion
}
