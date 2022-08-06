using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MatchingCardGame : Game
{
    [SerializeField] List<Sprite> cardSprites;
    [SerializeField] GameObject cardPrefab;
    [SerializeField] RectTransform cardParentTransform;

    public List<Card> SelectedCards;
    int numberOfSets = 3;
    int numberOfSetsMatched = 0;

    System.Random random;

    #region Game states
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();

        random = new System.Random();

        SelectedCards = new List<Card>();

        OnPrepareLevel();
    }

    public override void OnPrepareLevel()
    {
        base.OnPrepareLevel();

        // set up
        SelectSetOfCards();

        // play level
        OnPlayLevel();
    }

    public override bool IsLevelComplete()
    {
        // when there's no more cards
        return numberOfSetsMatched == numberOfSets;
    }
    #endregion

    #region Game Functions
    public void SelectCard(Card card)
    {
        SelectedCards.Add(card);
        
        if (SelectedCards.Count == 2)
        {
            StartCoroutine(CheckForMatchingCards());
        }
    }

    public void DeselectCard(Card card)
    {
        SelectedCards.Remove(card);
    }

    void SelectSetOfCards()
    {
        numberOfSetsMatched = 0;

        // remove any previous cards
        List<Card> cards = FindObjectsOfType<Card>().ToList();
        foreach(Card card in cards)
            Destroy(card.gameObject);

        List<Sprite> sprites = new List<Sprite>();

        // get list of random sprites
        for (int i = 0; i < numberOfSets; i++)
        {
            // get random
            int index = random.Next(cardSprites.Count);
            Sprite currentSprite = cardSprites[index];

            // add to list of selected sprites
            sprites.Add(currentSprite);
            sprites.Add(currentSprite);

            // remove from list (so it doesn't get selected again)
            cardSprites.Remove(currentSprite);
        }

        // shuffle list of sprites
        List<Sprite> shuffledSprites = Shuffle(sprites);

        // instantiate cards with sprites
        foreach (Sprite sprite in shuffledSprites)
        {
            InstantiateCard(sprite);
        }
    }

    void InstantiateCard(Sprite sprite)
    {
        GameObject cardInstance = Instantiate(cardPrefab, cardParentTransform);
        cardInstance.GetComponent<Card>().Image.sprite = sprite;
    }

    IEnumerator CheckForMatchingCards()
    {
        Card card1 = SelectedCards[0];
        Card card2 = SelectedCards[1];

        // wait for any card flipping animations to start
        yield return new WaitUntil(() => card1.cardAnimation.mCardState == CardState.Front && card2.cardAnimation.mCardState == CardState.Front);

        yield return new WaitForSeconds(1f);

        // check if they're matching
        if (card1.Image.sprite.name == card2.Image.sprite.name)
        {
            // TODO: sound effect
            // TODO: animation
            Debug.Log("Hiding cards. " + card1.Image.sprite.name + " " + card2.Image.sprite.name);

            card1.HideCard();
            card2.HideCard();

            numberOfSetsMatched++;
        }
        else
        {
            // wrong - flip both cards over
            // TODO: sound effect
            Debug.Log("Flipping cards");

            card1.FlipCardToBack();
            card2.FlipCardToBack();
        }

        SelectedCards.Clear();

    }
    #endregion

    #region Helper Functions
    private List<Sprite> Shuffle(List<Sprite> list)
    {
        List<Sprite> listCopy = new List<Sprite>(list);
        int n = listCopy.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Sprite value = listCopy[k];
            listCopy[k] = listCopy[n];
            listCopy[n] = value;
        }

        return listCopy;
    }

    #endregion
}