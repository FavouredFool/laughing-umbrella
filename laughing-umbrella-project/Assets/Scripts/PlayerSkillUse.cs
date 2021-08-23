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

                // Component wird zerst�rt
                Destroy((Object)activeSkill);
                activeSkill = null;

                // Farbe wird zur�ckge�ndert
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

            // Nur F�higkeit aufheben wenn F�higkeit vorher null war
            if (activeSkill == null)
            {
                // F�higkeit von Enums �ber Switch Case. MUSS F�R JEDE F�HIGKEIT ERWEITERT WERDEN -> MUSS ICH NOCH �NDERN
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

            // Orb wird zerst�rt
            Destroy(collision.gameObject);
        }
    }

    #endregion
}
