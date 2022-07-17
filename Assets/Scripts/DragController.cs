using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class DragController : MonoBehaviour
    {
        public Draggable LastDragged => lastDragged;

        private bool isDragActive = false;
        private Vector3 worldPosition;
        private Draggable lastDragged;

        private void Awake()
        {
            // only want one instance of controller
            DragController[] controllers = FindObjectsOfType<DragController>();
            if(controllers.Length > 1)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            if(Touchscreen.current?.primaryTouch?.press?.isPressed != null)
            {
                if (!Touchscreen.current.primaryTouch.press.isPressed)
                {
                    if (isDragActive)
                        Drop();
                    return;
                }

                // get touch position
                Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

                // convert to world space
                worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

                // check if drag is already active
                if (isDragActive)
                {
                    Drag();
                }
                else
                {
                    RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
                    if (hit.collider != null)
                    {
                        Draggable draggable = hit.transform.gameObject.GetComponent<Draggable>();
                        if (draggable != null)
                        {
                            lastDragged = draggable;
                            InitDrag();
                        }
                    }
                }
            }
        }

        void InitDrag()
        {
            UpdateDragStatus(true);
        }

        void Drag()
        {
            // only move if the target hasn't been reached
            if(lastDragged.TargetReached == false)
                lastDragged.transform.position = new Vector3(worldPosition.x, worldPosition.y, lastDragged.transform.position.z);
        }

        void Drop()
        {
            UpdateDragStatus(false);
        }

        void UpdateDragStatus(bool isDragging)
        {
            isDragActive = lastDragged.isDragging = isDragging;
            lastDragged.SpriteRenderer.sortingOrder = isDragging ? Layer.Dragging : Layer.Default;
        }
    }
}