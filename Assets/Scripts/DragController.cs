using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class DragController : MonoBehaviour
{
    public Draggable LastDragged => lastDragged;

    private bool isDragActive = false;
    private Vector3 worldPosition;
    private Draggable lastDragged;

    private List<Draggable> draggables;
    bool allTargetsReached = false;
    private void Awake()
    {
        // only want one instance of controller
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // check if all targets reached
        if (allTargetsReached) return;

        draggables = FindObjectsOfType<Draggable>().ToList();
        List<Draggable> draggableTargetsReached = draggables.Where(x => x.TargetReached == false).ToList();
        if(draggableTargetsReached.Count == 0)
        {
            allTargetsReached = true;
            AudioManager.instance.PlaySoundEffect(EnumSoundName.Victory);
        }

        if (Touchscreen.current?.primaryTouch?.press?.isPressed != null)
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
        // audio played when target not reached
        if (lastDragged.movementDestination == null)
            AudioManager.instance.PlaySoundEffect(EnumSoundName.DraggablePickUp);
        UpdateDragStatus(true);
    }

    void Drag()
    {
        // only move if the target hasn't been reached
        if (lastDragged.TargetReached == false)
            lastDragged.transform.position = new Vector3(worldPosition.x, worldPosition.y, lastDragged.transform.position.z);
    }

    void Drop()
    {
        // audio played when target reached
        if(lastDragged.movementDestination != null && lastDragged.TargetReached == false)
            AudioManager.instance.PlaySoundEffect(EnumSoundName.DraggableDrop);
        UpdateDragStatus(false);
    }

    void UpdateDragStatus(bool isDragging)
    {
        isDragActive = lastDragged.isDragging = isDragging;

        StartCoroutine(SetSortOrder(isDragging, lastDragged));
    }

    IEnumerator SetSortOrder(bool isDragging, Draggable dragged)
    {
        if (!isDragging)
            yield return new WaitUntil(() => dragged.IsMoving == false);
                
        dragged.SpriteRenderer.sortingOrder = isDragging ? Layer.Dragging : Layer.Default;
        dragged.GetComponentInChildren<MeshRenderer>().sortingOrder = isDragging ? Layer.Dragging : Layer.Default;
    }
}