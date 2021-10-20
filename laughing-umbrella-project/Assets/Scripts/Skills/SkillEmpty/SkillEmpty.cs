using UnityEngine;

public class SkillEmpty : MonoBehaviour, ISkill
{
    [Header("GameObjects")]
    public GameObject lineRendererObj;
    public GameObject circleOverlay;
    [Header("Layers")]
    public LayerMask enemyLayers;
    [Header("SkillVariables")]
    public float clickRadius = 0.5f;
    public float connectionTotalTime = 3f;
    public float speedMultiplier = 0.5f;
    public float maxDist = 5f;
    [Header("Color")]
    public Color lineColor;
    public Color circleColor;

    GameObject activeLineRenderer;
    LineRenderer activeLineRendererComp;

    GameObject activeCircleRenderer;

    

    GameObject player;
    GameObject activeConnection;
    float connectionStartTime;

    int currentHealth;

    Gradient gradient;

    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;


    protected void Start()
    {
        connectionStartTime = float.PositiveInfinity;
        player = gameObject.transform.parent.gameObject;
        currentHealth = player.GetComponent<PlayerActions>().getCurrentHealth();

        // Farbgradient aufsetzen
        gradient = new Gradient();
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.white;
        colorKey[0].time = 0f;
        colorKey[1].color = lineColor;
        colorKey[1].time = 1f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1f;
        alphaKey[0].time = 0f;
        alphaKey[1].alpha = 1f;
        alphaKey[1].time = 1f;

        gradient.SetKeys(colorKey, alphaKey);
    }
    protected void Update()
    {
        if (activeConnection)
        {

            if (Input.GetMouseButtonDown(1))
            {
                // bei Swap connection brechen
                BreakConnection();
            }


            if (Time.time - connectionStartTime > 0 && Time.time - connectionStartTime < connectionTotalTime)
            {

                // Aktive Connection
                activeLineRendererComp.SetPosition(0, gameObject.transform.parent.position);
                activeLineRendererComp.SetPosition(1, activeConnection.transform.position);
                activeLineRendererComp.startColor = gradient.Evaluate((Time.time - connectionStartTime) / connectionTotalTime);
                activeLineRendererComp.endColor = gradient.Evaluate((Time.time - connectionStartTime) / connectionTotalTime);


                // Connection trennen wenn Schaden genommen wird
                int activeHealth = player.GetComponent<PlayerActions>().getCurrentHealth();
                if (currentHealth > activeHealth)
                {
                    BreakConnection();
                    currentHealth = activeHealth;
                }

                // Connection trennen wenn Slot schon voll ist
                if (player.GetComponent<PlayerSkillUse>().GetActiveSkill() != player.GetComponent<PlayerSkillUse>().GetEmptySkill())
                {
                    BreakConnection();
                }

                // Connection trennen wenn Spieler zu weit von Gegner entfernt ist
                if(activeConnection)
                {
                    if (Vector2.Distance(activeConnection.transform.position, player.transform.position) > maxDist)
                    {
                        BreakConnection();
                    }
                }
                
            }

            if (Time.time - connectionStartTime >= connectionTotalTime)
            {
                // Connection ist vorbei
                GiveSkill();
                BreakConnection();
            }

        }

    }

    public void UseSkill()
    {

        

        // 1. Target wird gesucht
        // Richtung definieren
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPosition, clickRadius, enemyLayers);

        if (colliders.Length > 0)
        {
            // Herausfinden welcher Enemy am nähesten zum Spieler ist
            float shortestDist = float.PositiveInfinity;
            float tempDist;
            int distIndex = 0;

            for (int i = 0; i < colliders.Length; i++)
            {
                tempDist = Vector3.Distance(gameObject.transform.position, colliders[i].gameObject.transform.position);

                if (shortestDist > tempDist)
                {
                    distIndex = i;
                    shortestDist = tempDist;
                }
            }

            if (!activeConnection)
            {
                BuildConnection(colliders[distIndex]);

            }
            else
            {
                // Vorherige Connection zerstören
                BreakConnection();

                // Neue Connection aufbauen
                BuildConnection(colliders[distIndex]);
            }
            
        } else
        {
            BreakConnection();
        }
    }

    public void BuildConnection(Collider2D collider)
    {

        FindObjectOfType<AudioManager>().Play("SlotlessStart");

        activeLineRenderer = Instantiate(lineRendererObj, gameObject.transform);
        activeLineRendererComp = activeLineRenderer.GetComponent<LineRenderer>();
        activeLineRendererComp.positionCount = 2;


        activeConnection = collider.gameObject;
        connectionStartTime = Time.time;

        // Slow down player while channeling
        player.GetComponent<PlayerActions>().moveSpeed *= speedMultiplier;

        //Draw Circle
        activeCircleRenderer = Instantiate(circleOverlay, activeConnection.transform);
        activeCircleRenderer.GetComponent<CircleShape>().SetValues(maxDist*0.9f);
        activeCircleRenderer.GetComponent<LineRenderer>().startColor = circleColor;
        activeCircleRenderer.GetComponent<LineRenderer>().endColor = circleColor;


    }

    public void BreakConnection()
    {
        
        if (activeConnection)
        {
            activeConnection = null;
            connectionStartTime = float.PositiveInfinity;
            Destroy(activeLineRenderer);
            Destroy(activeCircleRenderer);

            // Speed up Player again
            player.GetComponent<PlayerActions>().moveSpeed /= speedMultiplier;
        }
    }

    void GiveSkill()
    {
        FindObjectOfType<AudioManager>().Play("SlotlessEnd");
        player.GetComponent<PlayerSkillUse>().GetSkill(activeConnection.GetComponent<Enemy>().enemyOrb.gameObject);
    }


    public void CleanUp()
    {
        // Bisher kein Cleanup nötig
    }

}