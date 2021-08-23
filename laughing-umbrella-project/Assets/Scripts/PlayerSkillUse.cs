using UnityEngine;

public class PlayerSkillUse : MonoBehaviour {

	#region Variables
	ISkill activeSkill;
	ISkill backupSkill;

    private string ORB_TAG = "Orb";

    SpriteRenderer sr;
    Color ORIGINCOLOR;
    #endregion


    #region UnityMethods

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        ORIGINCOLOR = sr.color;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (activeSkill != null)
            {
                // Skill wird genutzt
                activeSkill.UseSkill();

                // Component wird zerstört
                Destroy((Object)activeSkill);
                activeSkill = null;

                // Farbe wird zurückgeändert
                sr.color = ORIGINCOLOR;

            }
            else
            {
                Debug.Log("Der Spieler hat keinen aktiven Skill");
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(ORB_TAG))
        {
            Orb orb = collision.gameObject.GetComponent<Orb>();

            // Nur Fähigkeit aufheben wenn Fähigkeit vorher null war
            if (activeSkill == null)
            {
                // Fähigkeit von Enums über Switch Case. MUSS FÜR JEDE FÄHIGKEIT ERWEITERT WERDEN -> MUSS ICH NOCH ÄNDERN
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
                }
            }

            // Player nimmt Farbe des Orbs an
            sr.color = collision.gameObject.GetComponent<SpriteRenderer>().color;

            // Orb wird zerstört
            Destroy(collision.gameObject);
        }
    }

    #endregion
}
