using UnityEngine;

public class SkillBat : MonoBehaviour, ISkill
{

    #region Variablen
    [Header("GameObjects")]
    // GameObject - welches das Sprite repräsentiert (sollte Unterobjekt sein).
    public GameObject movingObject;

    [Header("Bat-Variables")]
    // Schaden pro Fledermaus
    public int attackDamage = 10;
    // Geschwindigkeit des Kreises
    public float speed = 3.5f;
    // Radius des Kreises
    public float circleRadius = 4f;
    // Länge der Existenz des Kreises in Sek
    public float durationAmount = 6f;
    
    // ObjekteArray
    readonly GameObject[] objectArray = new GameObject[3];

    // Startzeit
    float startTime = float.PositiveInfinity;

    #endregion

    protected void Update()
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

    public void CleanUp()
    {
        Destroy(gameObject);
    }

}