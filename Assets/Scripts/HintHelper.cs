using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class HintHelper : MonoBehaviour
{
    [SerializeField] GameObject FingerPointPrefab;
    [SerializeField] GameObject FingerPointUIPrefab;

    GameObject instantiatedGameObject;

    Game game;

    float secondsTillHint = 8f;
    float hintSeconds = 8f;

    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        // check for player input
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            // if player input, reset seconds
            secondsTillHint = hintSeconds;

            // get rid of previous hint?
            if (instantiatedGameObject != null)
                Destroy(instantiatedGameObject);
        }
        else
        {
            // update countdown
            secondsTillHint -= Time.deltaTime;

            // if x time has passed with no user input
            if (secondsTillHint <= 0)
            {
                secondsTillHint = hintSeconds + 8f;
                game.OnShowHint();
            }
        }
    }

    public void ClickAndDrag(Vector3 startPosition, Vector3 targetPosition)
    {
        Debug.Log("Click and drag");

        // Instantiate gameObject
        instantiatedGameObject = Instantiate(FingerPointPrefab, startPosition, Quaternion.identity);

        // Create a sequence
        Sequence mySequence = DOTween.Sequence();

        // scale in
        mySequence.Append(instantiatedGameObject.transform.DOScale(1f, 1f));

        mySequence.AppendInterval(0.5f);

        // scale down
        mySequence.Append(instantiatedGameObject.transform.DOScale(new Vector3(0.75f, 0.75f, 0), 0.50f));

        // move
        mySequence.Append(instantiatedGameObject.transform.DOMove(targetPosition, 2f));

        // fade out
        mySequence.Append(instantiatedGameObject.GetComponent<SpriteRenderer>().material.DOFade(0, 1.5f));

        Destroy(instantiatedGameObject, 8f);
    }

    void ClickAndClick(GameObject gameObject)
    {

    }
}