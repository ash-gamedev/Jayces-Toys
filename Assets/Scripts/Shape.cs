using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Shape : MonoBehaviour
{
    [HideInInspector] public List<Vector3> Points;
    [HideInInspector] public List<Transform> TransformPoints;
    [HideInInspector] public List<Tuple<Vector3, Vector3>> LinesDrawn;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        // get points
        TransformPoints = GetComponentsInChildren<Transform>().ToList();
        // remove first point (center/parent)
        TransformPoints.RemoveAt(0);
        Points = new List<Vector3>();
        foreach (Transform transform in TransformPoints)
            Points.Add(transform.position);

        // get sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        // hide shape sprite
        ShowShape(false);

        LinesDrawn = new List<Tuple<Vector3, Vector3>>();
    }

    #region public functions
    // Show shape 
    public void ShowShape(bool showShape)
    {
        float transparency = showShape ? 1 : 0;

        // todo: add animatuin
        spriteRenderer.color = new Color(1, 1, 1, transparency);
    }

    // Get neighbour points
    public List<Transform> GetNeighbours(Transform point)
    {
        int index = Points.IndexOf(point.position);

        // get neighbour indexes
        int neighbourIndex1 = index + 1;
        int neighbourIndex2 = index - 1;

        // special cases when index is the first or last
        if (index == 0)
            neighbourIndex2 = Points.Count - 1;
        else if (index == Points.Count - 1)
            neighbourIndex1 = 0;

        // get neighbour transforms
        List<Transform> neighbourPoints = new List<Transform>();
        neighbourPoints.Add(TransformPoints[neighbourIndex1]);
        neighbourPoints.Add(TransformPoints[neighbourIndex2]);

        return neighbourPoints;
    }

    // Check if two points are neighbours
    public bool AreNeighbours(Transform point1, Transform point2)
    {
        bool areNeighbours = false;

        // get index
        int index1 = Points.IndexOf(point1.position);
        int index2 = Points.IndexOf(point2.position);

        // if indexes are beside each other
        if (index1 + 1 == index2 || index1 - 1 == index2)
            areNeighbours = true;

        // special case (when index is 0 or index is last element in list)
        if (index1 == 0 && index2 == Points.Count - 1)
            areNeighbours = true;
        else if (index2 == 0 && index1 == Points.Count - 1)
            areNeighbours = true;

        Debug.Log(point1.position + " " + point2.position);
        Debug.Log("Are neighbours: " + areNeighbours + " - Indexes: " + index1 + " " + index2);

        // otherwise not neighbours
        return areNeighbours;
    }

    // Check if there's a line between two points
    public bool IsLine(Transform point1, Transform point2)
    {
        Tuple<Vector3, Vector3> points1 = new Tuple<Vector3, Vector3>(point1.position, point2.position);
        Tuple<Vector3, Vector3> points2 = new Tuple<Vector3, Vector3>(point2.position, point1.position);

        if (LinesDrawn.Contains(points1) || LinesDrawn.Contains(points2))
            return true;
        else
            return false;
    }

    // Check if a point has two neighbours
    public bool IsPointFullyConnected(Transform point)
    {
        int countOfLines = 0;
        foreach (Tuple<Vector3, Vector3> line in LinesDrawn)
        {
            if (line.Item1 == point.position || line.Item2 == point.position)
                countOfLines++;
        }

        if (countOfLines == 2)
            return true;
        else
            return false;
    }
    #endregion

    #region private functions

    #endregion
}