using UnityEngine;

public class SkillEmpty : MonoBehaviour, ISkill
{

    public GameObject lineRendererObj;
    public LayerMask enemyLayers;
    public float clickRadius = 0.5f;
    public float connectionTotalTime = 3f;
    public float speedMultiplier = 0.5f;

    GameObject activeLineRenderer;
    LineRenderer activeLineRendererComp;

    GameObject player;
    GameObject activeConnection;
    float connectionStartTime;

    int currentHealth;


    protected void Start()
    {
        connectionStartTime = float.PositiveInfinity;
        player = gameObject.transform.parent.gameObject;
        currentHealth = player.GetComponent<PlayerActions>().getCurrentHealth();
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

            // 2. Verbindung mit Target wird aufgebaut - Charakter wird geslowed

            if (Time.time - connectionStartTime > 0 && Time.time - connectionStartTime < connectionTotalTime)
            {

                // Aktive Connection
                activeLineRendererComp.SetPosition(0, gameObject.transform.parent.position);
                activeLineRendererComp.SetPosition(1, activeConnection.transform.position);


                // Connection trennen wenn Schaden genommen wird
                int activeHealth = player.GetComponent<PlayerActions>().getCurrentHealth();
                if (currentHealth > activeHealth)
                {
                    BreakConnection();
                    currentHealth = activeHealth;
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
            } else
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
        activeLineRenderer = Instantiate(lineRendererObj, gameObject.transform);
        activeLineRendererComp = activeLineRenderer.GetComponent<LineRenderer>();
        activeLineRendererComp.positionCount = 2;


        activeConnection = collider.gameObject;
        connectionStartTime = Time.time;

        // Slow down player while channeling
        player.GetComponent<PlayerActions>().moveSpeed *= speedMultiplier;

    }

    public void BreakConnection()
    {
        if (activeConnection)
        {
            activeConnection = null;
            connectionStartTime = float.PositiveInfinity;
            Destroy(activeLineRenderer);

            // Speed up Player again
            player.GetComponent<PlayerActions>().moveSpeed /= speedMultiplier;
        }
    }

    void GiveSkill()
    {
        player.GetComponent<PlayerSkillUse>().GetSkill(activeConnection.GetComponent<Enemy>().enemyOrb.gameObject);
    }


    public void CleanUp()
    {
        // Bisher kein Cleanup nötig
    }

}