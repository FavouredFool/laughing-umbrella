using UnityEngine;

public class SkillBat : MonoBehaviour, ISkill
{

    #region Variablen

    public float circleRadius;
    public GameObject movingObject;
    public float durationAmount;
    public int attackDamage = 10;
    public float speed = 2;

    GameObject[] objectArray = new GameObject[3];

    float startTime = float.PositiveInfinity;

    #endregion

    void Update()
    {
        // Abbruch nach gewisser Zeit
        if (Time.time - startTime > durationAmount)
        {
            CleanUp();
        }
    }


    public void UseSkill()
    {
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


        float startAngle = Random.Range(0, 360);
        objectArray[0].GetComponent<MovObjLogic>().CreateMovement(circleRadius, startAngle-180, speed);
        objectArray[1].GetComponent<MovObjLogic>().CreateMovement(circleRadius, ((startAngle+120f)%360)-180, speed);
        objectArray[2].GetComponent<MovObjLogic>().CreateMovement(circleRadius, ((startAngle-120f)%360)-180, speed);

    }

}