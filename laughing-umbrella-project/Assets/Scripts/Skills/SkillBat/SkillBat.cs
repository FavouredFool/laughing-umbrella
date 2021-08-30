using UnityEngine;

public class SkillBat : MonoBehaviour, ISkill
{

    #region Variablen

    public float circleRadius;
    public GameObject movingObject;
    public float durationAmount;

    GameObject[] objectArray = new GameObject[3];

    bool skillActive = false;
    float startTime = float.PositiveInfinity;

    #endregion

    void Update()
    {
        if (skillActive)
        {
            // Fledermäuse fliegen



        }

        // Abbruch nach gewisser Zeit
        if (Time.time - startTime > durationAmount)
        {
            CleanUp();
        }
    }


    public void UseSkill()
    {
        skillActive = true;
        startTime = Time.time;
        createMovingObjs();
    }

    public void CleanUp()
    {
        Destroy(gameObject);
    }

    void createMovingObjs()
    {

        objectArray[0] = Instantiate(movingObject, gameObject.transform);
        objectArray[1] = Instantiate(movingObject, gameObject.transform);
        objectArray[2] = Instantiate(movingObject, gameObject.transform);

        objectArray[0].GetComponent<MovObjLogic>().CreateMovement(circleRadius, 0f);
        objectArray[1].GetComponent<MovObjLogic>().CreateMovement(circleRadius, 120f);
        objectArray[2].GetComponent<MovObjLogic>().CreateMovement(circleRadius, -120f);

    }

}