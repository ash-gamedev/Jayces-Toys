using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    private LineRenderer lr;
    private Transform[] points;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void SetUpLine(List<Transform> points)
    {
        lr.positionCount = points.Count;
        this.points = points.ToArray();

        foreach(Transform point in points)
            Debug.Log(point.position);
    }

    private void Update()
    {
        if(points != null)
        {
            for (int i = 0; i < points?.Length; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
    }
}