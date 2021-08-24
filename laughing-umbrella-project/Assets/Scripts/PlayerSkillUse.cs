using UnityEngine;
using System.Collections.Generic;

public class PlayerSkillUse : MonoBehaviour {

    #region Variables
    // Skills deklarieren
    ISkill activeSkill;
	ISkill backupSkill;
    ISkill tempSkill;
    ISkill emptySkill;

    // Farben deklarieren
    SpriteRenderer sr;
    Color EMPTYCOLOR;
    Color backupColor;
    Color activeColor;
    Color tempColor;


    CapsuleCollider2D playerCollider;

    // Konstante für Tags
    private string ORB_TAG = "Orb";
    #endregion


    #region UnityMethods

    void Start() {

        // Skill-Variablen initialisieren
        gameObject.AddComponent<SkillEmpty>();
        emptySkill = GetComponent<SkillEmpty>();
        activeSkill = emptySkill;
        backupSkill = emptySkill;

        // Farben Initialisieren
        sr = GetComponent<SpriteRenderer>();
        EMPTYCOLOR = sr.color;
        activeColor = EMPTYCOLOR;
        backupColor = EMPTYCOLOR;

        // Playercollider
        playerCollider = gameObject.GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        // Linksklick -> Nutze Fähigkeit
        if (Input.GetMouseButtonDown(0))
        {
            if (activeSkill != emptySkill)
            {
                // Skill wird genutzt
                activeSkill.UseSkill();

                // Component wird zerstört
                Destroy((Object)activeSkill);
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
                activeSkill.UseSkill();
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
            Collider2D[] colliders = Physics2D.OverlapCapsuleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), playerCollider.size, playerCollider.direction, 0);

            if (colliders.Length > 1)
            {
                List<Collider2D> orbColliders = new List<Collider2D>();

                // Nur die Orb-Collider herausziehen
                foreach (Collider2D col in colliders)
                {
                    if (col.gameObject.tag.Equals(ORB_TAG))
                    {
                        orbColliders.Add(col);
                    }
                }

                // Herausfinden welcher Orb am nähesten zum Spieler ist
                if (orbColliders.Count > 0)
                {
                    float shortestDist = float.PositiveInfinity;
                    float tempDist;
                    int distIndex = 0;

                    for (int i = 0; i < orbColliders.Count; i++)
                    {
                        tempDist = Vector3.Distance(gameObject.transform.position, orbColliders[i].gameObject.transform.position);

                        if (shortestDist > tempDist)
                        {
                            distIndex = i;
                            shortestDist = tempDist;
                        }
                    }
                    // den Skill des Orbs absorbieren und Orb zerstören
                    GetSkill(orbColliders[distIndex]);
                }
            }
        }
    }

    void GetSkill(Collider2D collision)
    {
        // Fähigkeit von Enums über Switch Case. MUSS FÜR JEDE FÄHIGKEIT ERWEITERT WERDEN
        // SkillEmpty wird nicht aufgeführt, da er nicht durch Orbs erreichbar ist

        switch (collision.gameObject.GetComponent<Orb>().skillEnum)
        {
            case SkillEnum.Skill.SkillBallRed:
                gameObject.AddComponent<SkillBallRed>();
                activeSkill = GetComponent<SkillBallRed>();
                break;
            case SkillEnum.Skill.SkillBallGreen:
                gameObject.AddComponent<SkillBallGreen>();
                activeSkill = GetComponent<SkillBallGreen>();
                break;
            case SkillEnum.Skill.SkillBallYellow:
                gameObject.AddComponent<SkillBallYellow>();
                activeSkill = GetComponent<SkillBallYellow>();
                break;
            default:
                throw new System.Exception("SWITCH_CASE_FEHLER");
        }

        // Player nimmt Farbe des Orbs an
        activeColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
        sr.color = activeColor;

        // Orb wird zerstört
        Destroy(collision.gameObject);
    }

    #endregion
}
