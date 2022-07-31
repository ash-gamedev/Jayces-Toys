using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoeHeadPart : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;

    Draggable draggable;
    SpriteRenderer spriteRenderer;
    Vector3 startPosition;

    void Awake()
    {
        startPosition = gameObject.transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        draggable = GetComponent<Draggable>();
    }

    public void ReturnToStartPosition()
    {
        draggable.movementDestination = null;
        gameObject.transform.position = startPosition;
    }

    public void SetSpriteByIndex(int index)
    {
        Sprite sprite = sprites[index];
        spriteRenderer.sprite = sprite;
    }
}