using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{

    private LineRenderer lr;
    private List<Transform> points;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void SetUpLine(List<Transform> points)
    {
        lr.positionCount = points.Count;
        this.points = points;
    }

    public void RemoveLine()
    {
        this.points.Clear();
        lr.positionCount = 0;
    }

    private void Update()
    {
        if(points != null)
        {
            for (int i = 0; i < points?.Count; i++)
            {
                lr.SetPosition(i, points[i].position);
            }
        }
    }
}