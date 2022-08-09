using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Card status, front and back
public enum CardState
{
    Front,
    Back
}
public class CardAnimation : MonoBehaviour
{
    public GameObject mFront; //card front
    public GameObject mBack; //Back of card
    [HideInInspector] public CardState mCardState = CardState.Front; //Card current status, front or back?
    public float mTime = 0.3f;
    public bool isActive = false; //true represents that the flip is being performed and must not be interrupted

    /// <summary>
    /// Initialize card angle according to mCardState
    /// </summary>
    public void Init()
    {
        if (mCardState == CardState.Front)
        {
            // If you start from the front, rotate the back 90 degrees so that you can't see the back.
            mFront.transform.eulerAngles = Vector3.zero;
            mBack.transform.eulerAngles = new Vector3(0, 90, 0);
        }
        else
        {
            // Starting from the back, the same thing
            mFront.transform.eulerAngles = new Vector3(0, 90, 0);
            mBack.transform.eulerAngles = Vector3.zero;
        }

        
    }
    private void Start()
    {
        Init();
    }

    public void EnableCard(bool enabled)
    {
        mFront.SetActive(enabled);
        mBack.SetActive(enabled);
    }

    /// <summary>
    /// The interface left for external calls
    /// </summary>
    public void StartBack()
    {
        if (isActive)
            return;
        StartCoroutine(ToBack());
    }
    /// <summary>
    /// The interface left for external calls
    /// </summary>
    public void StartFront()
    {
        if (isActive)
            return;
        StartCoroutine(ToFront());
    }
    /// <summary>
    /// Turn over to the back
    /// </summary>
    IEnumerator ToBack()
    {
        isActive = true;
        mFront.transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mBack.transform.DORotate(new Vector3(0, 0, 0), mTime);
        mCardState = CardState.Back;
        isActive = false;

    }
    /// <summary>
    /// Turn to the front
    /// </summary>
    IEnumerator ToFront()
    {
        isActive = true;
        mBack.transform.DORotate(new Vector3(0, 90, 0), mTime);
        for (float i = mTime; i >= 0; i -= Time.deltaTime)
            yield return 0;
        mFront.transform.DORotate(new Vector3(0, 0, 0), mTime);
        mCardState = CardState.Front;
        isActive = false;
    }
}
