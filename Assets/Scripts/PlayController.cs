using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class CardSelection
{
    public CardMachine card1;
    public CardMachine card2;
    public int MaxSelected;

    public void Add(CardMachine newSelect) { 
    
        if(card1 == null)
            card1 = newSelect;
        else if(card2 == null && card1 != newSelect)
            card2 = newSelect;

        
    
    
    }


    public int Count()
    {
        if(card1 == null && card2 == null) return 0;
        else if (card1 == null || card2 == null) return 1;
        else return 2;
    }

    public bool IsFull()=> Count() >= MaxSelected;

    public void Reset()
    {
        card1 = null;
        card2 = null;
    }


    public bool TryMatch() => card1.GetType().Equals(card2.GetType());

}


public class CardDeck
{

    CardMachine[] deck;
    CardSelection selector;
    int cardAmount;

    int selectedCount;
    public bool IsInactive = false;
    

    public CardDeck(List<CardMachine> cards)
    {

        this.cardAmount = cards.Count;
        Transform deckParent = findDeckParent();
        initializeDeckWithPrefab(deckParent , cards);
        selector = new CardSelection();
        selector.MaxSelected = 2;
        initializeListener();
       

    }

    public void SwitchAll(CardMode mode)
    {
        if (deck == null)
            throw new System.Exception("DeckNull");

        for(int i = 0; i < deck.Length; i++)
            deck[i].ChangeState(mode);
    }


    public bool IsAllMatched()
    {
        for(int i = 0;i < deck.Length;i++)
            if (deck[i].current != CardMode.MATCHED)
                return false;

        return true;
    }


    Transform findDeckParent()
    {
        Transform deckParent = GameObject.Find("deckParent").transform;
        if (deckParent == null)
            throw new System.Exception("You must nickname an empty object as 'deckParent'");
    
        return deckParent;
    }


    void initializeDeckWithPrefab(Transform deckParent , List<CardMachine> prefabs)
    {
        deck = new CardMachine[cardAmount];
        if (deck.Length != prefabs.Count)
            throw new System.Exception("Deck and prefabs must be same size.");
        for (int i = 0; i < cardAmount; i++)
            deck[i] = Object.Instantiate<CardMachine>(prefabs[i], deckParent);
    }


    void initializeListener()
    {
        for (int i = 0; i < deck.Length; i++)
        {
            CardMachine cardSM = deck[i];
            cardSM.clickable.onClick.AddListener(() => { OnCardClick(cardSM); });
        }
    }


    void OnCardClick(CardMachine clicked)
    {

        if(IsInactive) { return; }

        if (!selector.IsFull())
        {
            selector.Add(clicked);
            clicked.ChangeState(CardMode.SELECTED);
         
        }

        if (selector.IsFull())
        {
            if (selector.TryMatch())
            {
                selector.card1.ChangeState(CardMode.MATCHED);
                selector.card2.ChangeState(CardMode.MATCHED);
            }
            else
            {
                selector.card1.ChangeState(CardMode.IDLE);
                selector.card2.ChangeState(CardMode.IDLE);
            }


            selector.Reset();
        }
            
                
    }

    




}


public class PlayController : MonoBehaviour
{
    CardDeck deck;

    [SerializeField] private GameObject CardParent;
    [SerializeField] private LevelScriptable LevelData;

    private PlayMenu menu;
    private Timer GOTimer;
    private Timer PreviewTimer;
    private int PreviewDuration;
    private int LosingDuration;



    // Start is called before the first frame update
    void Start()
    {

        menu = GameObject.Find("PlayMenu").GetComponent<PlayMenu>();
        initializeLevelData();

    }

    void initializeLevelData()
    {
        PreviewDuration = LevelData.PreviewDuration;
        LosingDuration = LevelData.GameOverDuration;
        deck = new CardDeck(LevelData.cards);


    }

    void cardPreview()
    {
        if(PreviewTimer == null)
        {
            deck.IsInactive = true;
            PreviewTimer = new Timer(PreviewDuration);
            deck.SwitchAll(CardMode.SELECTED);
            return;
        }

        menu.UpdatePreviewDuration(PreviewTimer.GetRemainingSecs());

        if (PreviewTimer.IsFinished() && GOTimer == null)
        {
            deck.IsInactive = false;
            GOTimer = new Timer(LosingDuration);
            deck.SwitchAll(CardMode.IDLE);
            menu.DisablePreviewDurationText();
            menu.EnableLosingDurationText();
        }





    }


    void gameplay()
    {
        
        if (deck.IsAllMatched())
            menu.PlayerWon();
        else if (GOTimer.IsFinished())
            menu.PlayerLost();

        menu.UpdateLosingDuration(GOTimer.GetRemainingSecs());



    }



    // Update is called once per frame
    void Update()
    {

        cardPreview();
        if(PreviewTimer.IsFinished())
            gameplay();

    }
}
