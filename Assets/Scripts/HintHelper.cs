using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;

public class HintHelper : MonoBehaviour
{
    [SerializeField] GameObject FingerPointPrefab;
    [SerializeField] GameObject FingerPointUIPrefab;

    GameObject instantiatedGameObject;
    Canvas canvas;

    Game game;

    float secondsTillHint = 8f;
    float hintSeconds = 8f;

    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<Game>();
        canvas = FindObjectOfType<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        // check for player input OR level not in play (ex. preparing level)
        if (Touchscreen.current.primaryTouch.press.isPressed || game.GameState != EnumGameState.LevelInPlay)
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
                Debug.Log("Showing hint");
                secondsTillHint = hintSeconds + 8f;
                game.OnShowHint();
            }
        }
    }

    public void ClickAndDrag(Vector3 startPosition, Vector3 targetPosition)
    {
        // Instantiate gameObject
        instantiatedGameObject = Instantiate(FingerPointPrefab, startPosition, Quaternion.identity);
        SpriteRenderer spriteRenderer = instantiatedGameObject.GetComponent<SpriteRenderer>();

        // Create a sequence
        Sequence mySequence = DOTween.Sequence();

        // fade in
        mySequence.Append(spriteRenderer.material.DOFade(1, 0.5f));

        // scale down
        mySequence.Append(instantiatedGameObject.transform.DOScale(new Vector3(0.75f, 0.75f, 0), 0.50f));

        // move
        mySequence.Append(instantiatedGameObject.transform.DOMove(targetPosition, 2f));

        // fade out
        mySequence.Append(spriteRenderer.material.DOFade(0, 1.5f));

        Destroy(instantiatedGameObject, 8f);
    }

    public void ClickAndClick(Vector3 startPosition, Vector3 targetPosition)
    {
        // Instantiate gameObject
        instantiatedGameObject = Instantiate(FingerPointUIPrefab, startPosition, Quaternion.identity, canvas.gameObject.transform);
        Image image = instantiatedGameObject.GetComponentInChildren<Image>();

        // Create a sequence
        Sequence mySequence = DOTween.Sequence();

        // fade in
        mySequence.Append(image.material.DOFade(1, 0.5f));

        // scale down (clicking)
        mySequence.Append(instantiatedGameObject.transform.DOScale(new Vector3(0.75f, 0.75f, 0), 0.50f));
        mySequence.Append(instantiatedGameObject.transform.DOScale(1f, 0.5f));

        // move
        mySequence.Append(instantiatedGameObject.transform.DOMove(targetPosition, 2f));

        // scale down (clicking)
        mySequence.Append(instantiatedGameObject.transform.DOScale(new Vector3(0.75f, 0.75f, 0), 0.50f));
        mySequence.Append(instantiatedGameObject.transform.DOScale(1f, 0.5f));

        // fade out
        mySequence.Append(image.material.DOFade(0, 1.5f));

        Destroy(instantiatedGameObject, 8f);
    }
}