using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class Draggable : MonoBehaviour
    {
        // public variables
        [HideInInspector] public bool isDragging;
        [HideInInspector] public Vector3 StartPosition;
       
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
        public DragTarget DragTarget => dragTarget;
        public bool TargetReached => targetReached;

        // private variables
        private SpriteRenderer spriteRenderer;

        // serilzd fields
        [SerializeField] private DragTarget dragTarget;
        
        // movement
        private float movementTime = 15f;
        public System.Nullable<Vector3> movementDestination;
        bool targetReached = false;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            StartPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if(targetReached == false)
            {
                if (movementDestination.HasValue)
                {
                    // reset movement destination is still dragging
                    if (isDragging)
                    {
                        //movementDestination = null;
                        return;
                    }

                    if (transform.position == movementDestination)
                    {
                        spriteRenderer.sortingOrder = Layer.Default;
                        movementDestination = null;
                        targetReached = true;
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

        private void OnTriggerStay2D(Collider2D collision)
        {
            if(collision.gameObject.transform.position == dragTarget.transform.position)
            {
                movementDestination = dragTarget.transform.position;
            }
        }
    }
}