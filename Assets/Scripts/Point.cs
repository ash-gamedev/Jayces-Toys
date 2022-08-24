using UnityEngine;
using System.Collections;

public class Point : MonoBehaviour
{
    EtchSketchGame game;
    public Transform startPosition = null;

    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<EtchSketchGame>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("InvisiblePoint"))
        {
            if (collision.GetComponent<Point>() != null && collision.CompareTag("Point") && startPosition != null && startPosition != collision.transform && game.isDragActive)
            {
                if (game.CanConnectToPoint(collision.transform))
                    game.ConnectToPoint(collision.transform);
            }
        }
    }
}