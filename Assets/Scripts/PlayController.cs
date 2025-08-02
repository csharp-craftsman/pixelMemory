using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CardContainer
{

    public List<Card> container;
    public int MaxSelectedNumber;


    public CardContainer(Card[] cardArr , int maxCardSelected)
    {
        container = new List<Card>(cardArr);
        MaxSelectedNumber = maxCardSelected;
    }

    public int SelectedLength()
    {
        int count = 0;
        for (int i = 0; i < container.Count; i++)
            if (container[i].IsSelected)
                count++;
        return count;
    }


    public bool IsSelectionOver() => MaxSelectedNumber <= SelectedLength();

    public List<Card> GetSelectedCards()
    {
        List<Card> selected = new List<Card>();
        for (int i = 0;i < container.Count; i++)
        {
            Card card = container[i];
            if (card.IsSelected && selected.Count <= MaxSelectedNumber)
                selected.Add(card);

        }

        return selected;

    }


    public void MatchCards(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            card.Match();
        }
    }

    public void ClearSelected()
    {
        for(int i = 0;i < container.Count;i++)
            container[i].IsSelected = false;
        

    }

    public void SwitchInteraction(bool mode)
    {
        for(int i = 0;i<container.Count;i++)
            container[i].IsDisabled = !mode;
    }




}




public class PlayController : MonoBehaviour
{
    [SerializeField] private GameObject CardContainerObj;
    CardContainer cards;


    // Start is called before the first frame update
    void Start()
    {
        Card[] cardArr = CardContainerObj.GetComponentsInChildren<Card>();
        cards = new CardContainer(cardArr , 2);
        if (cardArr.Length % cards.MaxSelectedNumber != 0)
            throw new System.Exception("Card Count Is Not Divisible");
        cards.SwitchInteraction(false);
        StartCoroutine(waitForStart());


    }


    IEnumerator waitForStart()
    {
        yield return new WaitForSeconds(2f);
        cards.SwitchInteraction(true);
    }




    // Update is called once per frame
    void Update()
    {

        bool matchMode = cards.IsSelectionOver();
        if (matchMode)
        {
            List<Card> list = cards.GetSelectedCards();
            cards.MatchCards(list);
            cards.ClearSelected();

        }

        
            


        

    }
}
