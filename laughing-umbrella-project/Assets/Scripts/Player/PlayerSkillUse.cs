using UnityEngine;
using System.Collections.Generic;

public class PlayerSkillUse : MonoBehaviour {

    #region Variables
    // Skills deklarieren
    GameObject activeSkill;
    GameObject backupSkill;
    GameObject tempSkill;
    GameObject emptySkill;

    // Farben deklarieren
    SpriteRenderer sr;
    Color EMPTYCOLOR;
    Color backupColor;
    Color activeColor;
    Color tempColor;

    // Children deklarieren
    Dictionary<string, GameObject> allSkills;

    CapsuleCollider2D playerCollider;

    // Konstante für Tags
    private string ORB_TAG = "Orb";

    LayerMask orbLayers; 
    #endregion


    #region UnityMethods

    void Start() {

        allSkills = new Dictionary<string, GameObject>();

        // Read all Skills from a Ressource-File
        foreach (Object prefab in Resources.LoadAll("Prefabs/Skills", typeof(GameObject)))
        {
            allSkills.Add(prefab.name, (GameObject) prefab);
        }


        // EmptySkill active setzen
        emptySkill = Instantiate(allSkills["SkillEmpty"], gameObject.transform);
        activeSkill = emptySkill;
        backupSkill = emptySkill;

        

        // Farben Initialisieren
        sr = GetComponent<SpriteRenderer>();
        EMPTYCOLOR = sr.color;
        activeColor = EMPTYCOLOR;
        backupColor = EMPTYCOLOR;

        // Playercollider
        playerCollider = gameObject.GetComponent<CapsuleCollider2D>();

        orbLayers = LayerMask.GetMask(ORB_TAG);
    }

    private void Update()
    {
        // Linksklick -> Nutze Fähigkeit
        if (Input.GetMouseButtonDown(0))
        {
            if (activeSkill != emptySkill)
            {
                // Skill wird genutzt
                activeSkill.GetComponent<ISkill>().UseSkill();

                // Component wird zerstört
                Destroy(activeSkill);
                activeSkill = emptySkill;
                

                // Farbe wird zurückgeändert
                activeColor = EMPTYCOLOR;
                sr.color = activeColor;

                // Wenn man bei Linksklick direkt wieder auf Orb steht wird nähestehenster eingezogen:
                checkForOrb();

            }
            else
            {
                // EmptySkill wird genutzt
                activeSkill.GetComponent<ISkill>().UseSkill();
            }
        }

        // Rechtsklick -> Swappe Fähigkeiten
        if (Input.GetMouseButtonDown(1))
        {

            // Skills swappen
            tempSkill = activeSkill;
            activeSkill = backupSkill;
            backupSkill = tempSkill;
            tempSkill = emptySkill;

            // Farben swappen
            tempColor = activeColor;
            activeColor = backupColor;
            backupColor = tempColor;
            sr.color = activeColor;

            // Wenn man bei Rechtsklick direkt wieder auf Orb steht wird nähestehenster eingezogen:
            checkForOrb();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check ob der Spieler mit einem Orb kollidiert ist
        if (collision.gameObject.tag.Equals(ORB_TAG))
        {
            // Nur Fähigkeit aufheben wenn Fähigkeit vorher null war
            if (activeSkill == emptySkill)
            {
                // den Skill des Orbs absorbieren und Orb zerstören
                GetSkill(collision);
            }
        }
    }


    void checkForOrb()
    {
        // Checkt ob Spieler auf Orb(s) steht. Falls ja wird der näheste absorbiert.

        if (activeSkill == emptySkill)
        {
            // Get all Collisions at Player-Character 
            Collider2D[] colliders = Physics2D.OverlapCapsuleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), playerCollider.size, playerCollider.direction, 0, orbLayers);

            if (colliders.Length > 0)
            {
            // Herausfinden welcher Orb am nähesten zum Spieler ist
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
                // den Skill des Orbs absorbieren und Orb zerstören
                GetSkill(colliders[distIndex]);
            }
        }
    }

    void GetSkill(Collider2D collision)
    {
        // Fähigkeit von Enums über Switch Case. MUSS FÜR JEDE FÄHIGKEIT ERWEITERT WERDEN
        // SkillEmpty wird nicht aufgeführt, da er nicht durch Orbs erreichbar ist
        // Zugriff über Children in Hashmap
        switch(collision.gameObject.GetComponent<Orb>().skillEnum)
        {
            case SkillEnum.Skill.SKILLBALLRED:
                activeSkill = Instantiate(allSkills["SkillBallRed"], gameObject.transform);
                break;
            case SkillEnum.Skill.SKILLSLASH:
                activeSkill = Instantiate(allSkills["SkillSlash"], gameObject.transform);
                break;
            case SkillEnum.Skill.SKILLBALLGREEN:
                activeSkill = Instantiate(allSkills["SkillBallGreen"], gameObject.transform);
                break;
            case SkillEnum.Skill.SKILLBALLYELLOW:
                activeSkill = Instantiate(allSkills["SkillBallYellow"], gameObject.transform);
                break;
        }

        // Player nimmt Farbe des Orbs an
        activeColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
        sr.color = activeColor;

        // Orb wird zerstört
        Destroy(collision.gameObject);
    }

    #endregion
}
