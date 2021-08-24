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

    public LayerMask orbLayers;

    // Konstante f�r Tags
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
        // Linksklick -> Nutze F�higkeit
        if (Input.GetMouseButtonDown(0))
        {
            if (activeSkill != emptySkill)
            {
                // Skill wird genutzt
                activeSkill.UseSkill();

                // Component wird zerst�rt
                Destroy((Object)activeSkill);
                activeSkill = emptySkill;

                // Farbe wird zur�ckge�ndert
                activeColor = EMPTYCOLOR;
                sr.color = activeColor;

                // Wenn man bei Linksklick direkt wieder auf Orb steht wird n�hestehenster eingezogen:
                checkForOrb();

            }
            else
            {
                // EmptySkill wird genutzt
                activeSkill.UseSkill();
            }
            
        }
        // Rechtsklick -> Swappe F�higkeiten
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


            // Wenn man bei Rechtsklick direkt wieder auf Orb steht wird n�hestehenster eingezogen:
            checkForOrb();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check ob der Spieler mit einem Orb kollidiert ist
        if (collision.gameObject.tag.Equals(ORB_TAG))
        {
            // Nur F�higkeit aufheben wenn F�higkeit vorher null war
            if (activeSkill == emptySkill)
            {
                // den Skill des Orbs absorbieren und Orb zerst�ren
                GetSkill(collision);
            }
        }
    }


    void checkForOrb()
    {
        // Checkt ob Spieler auf Orb(s) steht. Falls ja wird der n�heste absorbiert.

        if (activeSkill == emptySkill)
        {
            // Get all Collisions at Player-Character 
            Collider2D[] colliders = Physics2D.OverlapCapsuleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), playerCollider.size, playerCollider.direction, 0, orbLayers);

            if (colliders.Length > 0)
            {
            // Herausfinden welcher Orb am n�hesten zum Spieler ist
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
                // den Skill des Orbs absorbieren und Orb zerst�ren
                GetSkill(colliders[distIndex]);
            }
        }
    }

    void GetSkill(Collider2D collision)
    {
        // F�higkeit von Enums �ber Switch Case. MUSS F�R JEDE F�HIGKEIT ERWEITERT WERDEN
        // SkillEmpty wird nicht aufgef�hrt, da er nicht durch Orbs erreichbar ist

        switch (collision.gameObject.GetComponent<Orb>().skillEnum)
        {
            case SkillEnum.Skill.SKILLBALLRED:
                gameObject.AddComponent<SkillBallRed>();
                activeSkill = GetComponent<SkillBallRed>();
                break;
            case SkillEnum.Skill.SKILLBALLGREEN:
                gameObject.AddComponent<SkillBallGreen>();
                activeSkill = GetComponent<SkillBallGreen>();
                break;
            case SkillEnum.Skill.SKILLBALLYELLOW:
                gameObject.AddComponent<SkillBallYellow>();
                activeSkill = GetComponent<SkillBallYellow>();
                break;
            case SkillEnum.Skill.SKILLSLASH:
                gameObject.AddComponent<SkillSlash>();
                activeSkill = GetComponent<SkillSlash>();
                break;
            default:
                throw new System.Exception("SWITCH_CASE_FEHLER");
        }

        // Player nimmt Farbe des Orbs an
        activeColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
        sr.color = activeColor;

        // Orb wird zerst�rt
        Destroy(collision.gameObject);
    }

    #endregion
}
