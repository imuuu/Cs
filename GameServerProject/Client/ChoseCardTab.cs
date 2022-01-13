using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChoseCardTab : MonoBehaviour
{

    [SerializeField] GameObject _cardPrefab;
    CardBuilder _cardBuilder;

    private GameObject[] _cards;
    private bool isChosen = false;
    private CanvasGroup _backGround;
    private GameManager _gameManager;
    private Hand _hand;
    private void Awake()
    {

        _cardBuilder = GameObject.FindObjectOfType<CardBuilder>();
        _gameManager = GameObject.FindObjectOfType<GameManager>();

        _backGround = transform.GetChild(0).GetComponent<CanvasGroup>();

       
    }

    private void Start()
    {
        _hand = _gameManager.GetHand();
    }
    void OnChosenCard()
    {
        isChosen = true;
        LTDescr ltd = _backGround.LeanAlpha(0, 0.5f).setOnComplete(x =>
        {
            transform.GetChild(0).gameObject.SetActive(false);
        });
        
        for(int i = 0; i < _cards.Length; ++i)
        {
            _cards[i].transform.LeanMoveLocalX(Screen.width, 0.5f).setEaseInExpo().setDestroyOnComplete(true);
        }

        _cards = null;
       
      
    }
    void SpawnCards(int deckID,Card[] cards)
    {
        _cards = new GameObject[cards.Length];

        Vector2[] cardPositions = _hand.GetCardPositions(_hand.MakeScaleFactor(this.gameObject), cards.Length, 250);
        for (int i = 0; i < cards.Length; ++i)
        {
            _cards[i] = _cardBuilder.SpawnCard(cards[i],_gameManager);

            _cards[i].AddComponent<Button>();
            string uuid = _cards[i].GetComponent<CardUI>().GetCard()._uuid.ToString();
            _cards[i].GetComponent<Button>().onClick.AddListener(delegate { Clicked(deckID, uuid); });

            Transform trans = _cards[i].GetComponent<Transform>();
            trans.SetParent(this.transform);
            trans.localPosition = new Vector2(Screen.width, 0);
            trans.LeanMoveLocalX(cardPositions[i].x, 0.5f).setEaseInOutExpo().delay = 0.1f;
            _cards[i].transform.localScale *= 2;

        }

    }
 
    

    public void InitShowCardsUI(int deckID, Card[] cards)
    {
        isChosen = false;
        transform.GetChild(0).gameObject.SetActive(true);
        _backGround.alpha = 0;
        _backGround.LeanAlpha(1, 0.5f);
        SpawnCards(deckID, cards);
    }

    void Clicked(int deckID, string uuid)
    {
        if (isChosen)
            return;

        OnChosenCard();
        ClientSend.SendChosenCard(deckID, uuid);
    }
}
