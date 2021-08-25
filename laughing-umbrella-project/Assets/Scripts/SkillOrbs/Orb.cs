using UnityEngine;

public class Orb : MonoBehaviour {


   

    #region Variables
    public SkillEnum.Skill skillEnum;
    #endregion

    

    #region Konstruktor
    public Orb(SkillEnum.Skill skillEnum)
    {
        this.skillEnum = skillEnum;
    }
    #endregion

    #region UnityMethods

    public void Start()
    {

    }


    #endregion
}
