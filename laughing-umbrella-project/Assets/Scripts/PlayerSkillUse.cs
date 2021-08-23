using UnityEngine;

public class PlayerSkillUse : MonoBehaviour {

    #region Variables
    ISkill activeSkill = null;
	ISkill backupSkill = null;
    ISkill tempSkill = null;
    ISkill emptySkill = null;

    private string ORB_TAG = "Orb";

    SpriteRenderer sr;
    Color EMPTYCOLOR;
    Color backupColor;
    Color activeColor;
    Color tempColor;
    #endregion


    #region UnityMethods

    void Start() {

        gameObject.AddComponent<SkillEmpty>();
        emptySkill = GetComponent<SkillEmpty>();
        activeSkill = emptySkill;
        backupSkill = emptySkill;

        sr = GetComponent<SpriteRenderer>();
        EMPTYCOLOR = sr.color;
        backupColor = EMPTYCOLOR;
    }

    private void Update()
    {
        // Rechtsklick -> Nutze Fähigkeit
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

            }
            else
            {
                // EmptySkill wird genutzt
                
                activeSkill.UseSkill();
            }
            
        }
        // Linksklick -> Swappe Fähigkeiten
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

        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check ob der Spieler mit einem Orb kollidiert ist
        if (collision.gameObject.tag.Equals(ORB_TAG))
        {
            Orb orb = collision.gameObject.GetComponent<Orb>();

            // Nur Fähigkeit aufheben wenn Fähigkeit vorher null war
            if (activeSkill == emptySkill)
            {
                // Fähigkeit von Enums über Switch Case. MUSS FÜR JEDE FÄHIGKEIT ERWEITERT WERDEN
                // SkillEmpty wird nicht aufgeführt, da er nicht durch Orbs erreichbar ist
                switch (orb.skillEnum)
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
        }
    }

    #endregion
}
