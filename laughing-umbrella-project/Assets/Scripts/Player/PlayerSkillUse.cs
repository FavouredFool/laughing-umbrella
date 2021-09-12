using UnityEngine;
using System.Collections.Generic;

public class PlayerSkillUse : MonoBehaviour {

    #region Variables
    
    // Collider vom Unterobject
    public CapsuleCollider2D playerCollider;
    // Layer auf dem Orbs aufgesammelt werden können
    public LayerMask orbLayers;

    // Skills deklarieren
    GameObject activeSkill;
    GameObject backupSkill;
    GameObject tempSkill;
    GameObject emptySkill;

    PlayerActions playerActions;

    // Children deklarieren
    Dictionary<string, GameObject> allSkills;

    // Konstante für Tags
    string ORB_TAG = "Orb";

    #endregion


    #region UnityMethods

    protected void Start() {

        playerActions = GetComponent<PlayerActions>();

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


        orbLayers = LayerMask.GetMask(ORB_TAG);
    }

    protected void Update()
    {
        // Linksklick -> Nutze Fähigkeit
        if (Input.GetMouseButtonDown(0))
        {
            if (activeSkill != emptySkill)
            {
                // Skill wird genutzt
                activeSkill.GetComponent<ISkill>().UseSkill();
                gameObject.GetComponent<Animator>().SetTrigger("cast");

                activeSkill = emptySkill;

                // Wenn man bei Linksklick direkt wieder auf Orb steht wird nähestehenster eingezogen:
                checkForOrb();


            }
            else
            {
                // EmptySkill wird genutzt
                gameObject.GetComponent<Animator>().SetTrigger("cast");
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

            // Wenn man bei Rechtsklick direkt wieder auf Orb steht wird nähestehenster eingezogen:
            checkForOrb();

        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Check ob der Spieler mit einem Orb kollidiert ist
        if (collision.gameObject.tag.Equals(ORB_TAG))
        {
            // Nur Fähigkeit aufheben wenn Fähigkeit vorher null war
            if (activeSkill == emptySkill)
            {
                // den Skill des Orbs absorbieren und Orb zerstören
                GetSkill(collision.gameObject);
                Destroy(collision.gameObject);
            }
        }
    }


    protected void checkForOrb()
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
                GetSkill(colliders[distIndex].gameObject);
                // Orb wird zerstört
                Destroy(colliders[distIndex].gameObject);
            }
        }
    }

    public void GetSkill(GameObject orb)
    {
        // Fähigkeit von Enums über Switch Case. MUSS FÜR JEDE FÄHIGKEIT ERWEITERT WERDEN
        // SkillEmpty wird nicht aufgeführt, da er nicht durch Orbs erreichbar ist
        // Zugriff über Children in Hashmap
        switch(orb.GetComponent<Orb>().skillEnum)
        {
            case SkillEnum.Skill.SKILLSLASH:
                activeSkill = Instantiate(allSkills["SkillSlash"], gameObject.transform);
                break;
            case SkillEnum.Skill.SKILLBAT:
                activeSkill = Instantiate(allSkills["SkillBat"], gameObject.transform);
                break;
            case SkillEnum.Skill.SKILLFIRE:
                activeSkill = Instantiate(allSkills["SkillFire"], gameObject.transform);
                break;
        }

        if (playerActions.getDashCount() < 2)
        {
            playerActions.setDashCount(playerActions.getDashCount() + 1);
        }

    }

    public GameObject GetActiveSkill()
    {
        return activeSkill;
    }

    public GameObject GetBackupSkill()
    {
        return backupSkill;
    }

    public GameObject GetEmptySkill()
    {
        return emptySkill;
    }

    #endregion
}
