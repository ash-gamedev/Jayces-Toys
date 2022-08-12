using System.Collections;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    // public variables
    [HideInInspector] public bool isDragging;
    [HideInInspector] public Vector3 StartPosition;
    [HideInInspector] public bool IsMoving 
    {
        get
        {
            bool movingToStart = (movementDestination == null && Vector2.Distance(StartPosition, transform.position) > .15f);
            bool movingToTarget = (movementDestination != null && dragTarget != null && Vector2.Distance(dragTarget.transform.position, transform.position) > .15f);
            return movingToStart || movingToTarget;
        }
    } 
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return spriteRenderer;
        }
        set
        {
            spriteRenderer = value;
        }
    }
    public DragTarget DragTarget
    {
        get
        {
            return dragTarget;
        }
        set
        {
            dragTarget = value;
            gameObject.transform.position = dragTarget.transform.position;
        }
    }
    public bool TargetReached => transform.position == dragTarget?.transform.position;

    // private variables
    private SpriteRenderer spriteRenderer;

    // serilzd fields
    [SerializeField] private DragTarget dragTarget;

    // movement
    private float movementTime = 15f;
    public System.Nullable<Vector3> movementDestination;

    Game game;

    private void Awake()
    {
        StartPosition = gameObject.transform.position;
        if (dragTarget != null)
            gameObject.transform.position = dragTarget.transform.position;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        game = FindObjectOfType<Game>();
    }

    private void FixedUpdate()
    {
        if (game.GameState == EnumGameState.LevelInPlay)
        {
            if (TargetReached == false)
            {
                if (movementDestination.HasValue)
                {
                    // reset movement destination is still dragging
                    if (isDragging)
                    {
                        movementDestination = null;
                        return;
                    }

                    if (transform.position == movementDestination)
                    {
                        spriteRenderer.sortingOrder = Layer.Default;
                        movementDestination = null;
                    }
                    else
                    {
                        // smoothly move
                        transform.position = Vector3.Lerp(transform.position, movementDestination.Value, movementTime * Time.fixedDeltaTime);
                    }
                }
                else
                {
                    if (isDragging || transform.position == StartPosition) return;

                    // smoothly move
                    transform.position = Vector3.Lerp(transform.position, StartPosition, movementTime * 0.25f * Time.fixedDeltaTime);
                }
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.transform.position == dragTarget?.transform.position && isDragging)
        {
            movementDestination = dragTarget.transform.position;
        }
    }
}