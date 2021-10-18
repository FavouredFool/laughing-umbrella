using UnityEngine;
using System.Collections;


public class CircleShape : MonoBehaviour
{
    [Range(0, 50)]
    public int segments = 50;

    float radius = 5;

    LineRenderer line;

    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        
        line.positionCount = segments + 1;
        line.useWorldSpace = false;

        CreatePoints();
    }

    private void Update()
    {
        
    }

    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }

        
    }

    public void SetValues(float radius)
    {
        this.radius = radius;
    }
}
