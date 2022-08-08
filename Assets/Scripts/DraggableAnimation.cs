using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DraggableAnimation : MonoBehaviour
{
    Draggable draggable;

    // Use this for initialization
    void Awake()
    {
        draggable = GetComponent<Draggable>();
    }

    public void ShakeParts()
    {

    }

    public void StartAnimation()
    {
        draggable.movementDestination = null;

        // Create a sequence
        Sequence mySequence = DOTween.Sequence();

        // shake
        mySequence.Append(transform.DOShakePosition(1f, strength: 0.1f));

        // Delay the whole Sequence by 1 second
        //mySequence.PrependInterval(1);

        // move to start position
        mySequence.Append(transform.DOMove(draggable.StartPosition, 1f));
    }
}