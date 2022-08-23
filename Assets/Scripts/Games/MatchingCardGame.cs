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

    GridLayoutGroup gridLayoutGroup;
    public List<Card> SelectedCards;
    int numberOfSetsMatched = 0;

    System.Random random;
    HintHelper hintHelper;

    int[] numberOfSetsPerLevel;

    #region Game states
    public override void OnPrepareGame()
    {
        base.OnPrepareGame();

        random = new System.Random();
        numberOfSetsPerLevel = new int[]{ 2, 3, 4 };
        SelectedCards = new List<Card>();
        gridLayoutGroup = cardParentTransform.GetComponent<GridLayoutGroup>();
        hintHelper = FindObjectOfType<HintHelper>();

        OnPrepareLevel();
    }

    public override void OnPrepareLevel()
    {
        base.OnPrepareLevel();

        // set up
        SelectSetOfCards();
    }

    public override bool IsLevelComplete()
    {
        // when there's no more cards
        return FindObjectsOfType<Card>().Where(x => x.isHidden == false).Count() == 0;
    }

    public override void OnShowHint()
    {
        List<Card> cards = FindObjectsOfType<Card>().ToList();


        Debug.Log(cards.Count);

        Card card1 = cards.Where(x => x.isHidden == false).FirstOrDefault();
        cards.Remove(card1);

        Debug.Log(cards.Count);

        if(card1 != null)
        {
            Card card2 = cards.Where(x => x.Image.sprite.name == card1.Image.sprite.name).FirstOrDefault();

            Debug.Log(card2);

            if (card2 != null)
                hintHelper.ClickAndClick(card1.transform.position, card2.transform.position);
        }
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

        List<Sprite> sprites = new List<Sprite>();

        // get list of random sprites
        int numberOfSets = numberOfSetsPerLevel[levelsCompleted];
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

        // set number of columns for grid layout based on number of sets
        gridLayoutGroup.constraintCount = numberOfSets;

        // instantiate cards with sprites
        StartCoroutine(InstantiateCards(shuffledSprites));
    }

    void InstantiateCard(Sprite sprite)
    {
        GameObject cardInstance = Instantiate(cardPrefab, cardParentTransform);
        Card card = cardInstance.GetComponent<Card>();
        card.Image.sprite = sprite;
        card.cardAnimation.EnableCard(false);
    }

    IEnumerator InstantiateCards(List<Sprite> sprites)
    {
        // remove any previous cards
        List<Card> previousCards = FindObjectsOfType<Card>().ToList();
        for (int i = previousCards.Count - 1; i >= 0; i--)
        {
            Destroy(previousCards[i].gameObject);
            previousCards.RemoveAt(i);
        }

        // wait until previous cards are officially destroyed
        yield return new WaitUntil(() => FindObjectsOfType<Card>().Count() == 0);

        // instantiate new cards
        foreach (Sprite sprite in sprites)
        {
            InstantiateCard(sprite);
        }

        // show each cards
        List<Card> cards = FindObjectsOfType<Card>().ToList();
        for(int i = cards.Count() - 1; i >= 0; i--)
        {
            AudioManager.instance?.PlaySoundEffect(EnumSoundName.CardFlip);
            cards[i].cardAnimation.EnableCard(true);
            yield return new WaitForSeconds(0.4f);
        }

        yield return new WaitForSeconds(1.75f);

        foreach (Card card in cards)
        {
            card.FlipCardToBack();
        }

        // play level
        OnPlayLevel();
    }

    IEnumerator CheckForMatchingCards()
    {
        Card card1 = SelectedCards[0];
        Card card2 = SelectedCards[1];

        // wait for any card flipping animations to start
        yield return new WaitUntil(() => card1.cardAnimation.mCardState == CardState.Front && card2.cardAnimation.mCardState == CardState.Front);

        yield return new WaitForSeconds(0.4f);

        // check if they're matching
        if (card1.Image.sprite.name == card2.Image.sprite.name)
        {
            AudioManager.instance?.PlaySoundEffect(EnumSoundName.CardMatch);

            yield return new WaitForSeconds(0.25f);

            card1.HideCard();
            card2.HideCard();

            yield return new WaitForSeconds(0.25f);

            numberOfSetsMatched++;
        }
        else
        {
            AudioManager.instance?.PlaySoundEffect(EnumSoundName.CardMismatch);

            yield return new WaitForSeconds(0.5f);

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