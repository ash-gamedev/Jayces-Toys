using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoeHeadPart : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSpriteByIndex(int index)
    {
        Sprite sprite = sprites[index];
        spriteRenderer.sprite = sprite;
    }
}