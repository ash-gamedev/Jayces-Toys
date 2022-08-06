using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Card : MonoBehaviour
{
    [SerializeField] public Image Image;

    public CardAnimation cardAnimation;
    MatchingCardGame matchingCardGame;

    [HideInInspector] public bool isHidden = false; // card is hiddren when matched

    private void Awake()
    {
        cardAnimation = GetComponent<CardAnimation>();
    }

    private void Start()
    {
        matchingCardGame = FindObjectOfType<MatchingCardGame>();
    }

    public void OnClick()
    {
        if (matchingCardGame?.SelectedCards.Count < 2 && matchingCardGame?.GameState == EnumGameState.LevelInPlay && cardAnimation.isActive == false)
            FlipCard();
    }

    public void FlipCard()
    {
        // TODO: add animation
        // TODO: add sound

        // if fliiping from back to front, add to selected card list
        if (cardAnimation.mCardState == CardState.Back)
            FlipCardToFront();
        else // remove from list
            FlipCardToBack();
    }

    public void FlipCardToFront()
    {
        matchingCardGame.SelectCard(this);
        cardAnimation.StartFront();
    }

    public void FlipCardToBack()
    {
        matchingCardGame.DeselectCard(this);
        cardAnimation.StartBack();
    }

    public void HideCard()
    {
        GetComponent<Button>().enabled = false;

        StartCoroutine(WaitAndHideCard());
    }

    IEnumerator WaitAndHideCard()
    {
        yield return new WaitUntil(() => cardAnimation.isActive == false);

        isHidden = true;

        gameObject.transform.DOScale(Vector3.zero, 2f);
    }
}