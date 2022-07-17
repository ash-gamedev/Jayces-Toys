using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class DrawLine : MonoBehaviour
    {
        public GameObject linePrefab;
        public GameObject currentLine;

        public LineRenderer lineRenderer;
        public List<Vector2> fingerPositions;

        bool isDrawing = false;

        // Update is called once per frame
        void Update()
        {
            if (Touchscreen.current?.primaryTouch?.press?.isPressed != null)
            {
                if (!Touchscreen.current.primaryTouch.press.isPressed)
                {
                    if (isDrawing)
                        FinishLine();
                    return;
                }

                // get touch position
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

                // convert to world space
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

                // check if draw is already active
                if (isDrawing)
                {
                    // update line if certain distance away
                    if (Vector2.Distance(worldPosition, fingerPositions[fingerPositions.Count - 1]) > .1f)
                    {
                        UpdateLine(worldPosition);
                    }
                }
                else
                {
                    CreateLine(worldPosition);
                }
            }
        }

        void CreateLine(Vector2 worldPosition)
        {
            isDrawing = true;

            // sound effect
            AudioManager.instance.PlaySoundEffect(EnumSoundName.Drawing);

            // instantiate new line
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();

            // clear points from previous line
            fingerPositions.Clear();

            fingerPositions.Add(worldPosition);
            fingerPositions.Add(worldPosition);

            // update line renderer & edfe collider
            lineRenderer.SetPosition(0, fingerPositions[0]);
            lineRenderer.SetPosition(1, fingerPositions[1]);
        }

        void UpdateLine(Vector2 newFingerPos)
        {
            fingerPositions.Add(newFingerPos);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        }

        void FinishLine()
        {
            isDrawing = false;
            AudioManager.instance.StopSoundEffect(EnumSoundName.Drawing);
        }
    }
}