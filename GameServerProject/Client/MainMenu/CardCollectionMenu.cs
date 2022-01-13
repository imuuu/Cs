using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
public class CardCollectionMenu : MonoBehaviour
{
    public Card[] _cards;
    private int _page = -1;
    public CollectionCardPlate[] _colCardPlates;
    private int[] _cardCounts;
    private CardBuilder _cardBuilder;
    private GameManager _gameManager;

    [SerializeField] TextMeshProUGUI _tLowCost; int _lowCount = 0;
    [SerializeField] TextMeshProUGUI _tMidCost; int _midCount = 0;
    [SerializeField] TextMeshProUGUI _tHighCost; int _highCount = 0;

    private void Awake()
    {
        _tLowCost.text = "";
        _tMidCost.text = "";
        _tHighCost.text = "";
        _cardBuilder = GameObject.FindObjectOfType<CardBuilder>();
        _gameManager = FindObjectOfType<GameManager>();

        foreach (CollectionCardPlate ccp in _colCardPlates) { ccp.SetClickAction(SetCardCount); };

    }

    public void SetCards(Card[] cards, Dictionary<int, int> counts)
    {
        _cards = cards.OrderBy(x => x._manaCost).ToArray();
        _cardCounts = new int[_cards.Length];
        for(int i = 0; i < _cards.Length; ++i)
        {
            //_cardCounts[i] = counts[i];
            SetCardCount(i, counts[_cards[i]._id]);
        }
       
        _page = -1;
        ChangePage(0);
    }

    public void SetCardCount(int slot, int amount)
    {
        //print($"slot: {slot}, amount {amount}");
        int dif = amount - _cardCounts[slot];
        _cardCounts[slot] = amount;
        Counter(_cards[slot], dif);
    }

    void SendCards()
    {
        List<int> cardIds = new List<int>();
        for(int i = 0; i < _cardCounts.Length; i++)
        {
            for(int count = 0; count < _cardCounts[i]; count++)
            {
                cardIds.Add(_cards[i]._id);
            }
        }
        _gameManager.SetCardIDs(cardIds.ToArray());
    }

    void Counter(Card card, int amount)
    {
        //if (card == null) { print($"card:null amount: {amount}"); return; }
        //print($"card: {card._name} amount: {amount}");

        if(card._manaCost > 0 && card._manaCost < 4)
        {
            _lowCount += amount;
            _tLowCost.text = _lowCount == 0 ? "" : _lowCount.ToString();
            return;
        }

        if (card._manaCost > 3 && card._manaCost < 7)
        {
            _midCount += amount;
            _tMidCost.text = _midCount == 0 ? "" : _midCount.ToString();
            return;
        }

        if (card._manaCost > 6)
        {
            _highCount += amount;
            _tHighCost.text = _highCount == 0 ? "" : _highCount.ToString();
            return;
        }
    }

    public void ButtonBack()
    {
        SendCards();
        this.gameObject.SetActive(false);
    }
    public void ChangePage(int amount)
    {
        int lastPage = _page;
        _page += amount;
        
        int totalPages = 0;
        if (_cards != null && _cards.Length != 0)
            totalPages = Mathf.CeilToInt(_cards.Length / _colCardPlates.Length);

        //print($"totalPages: {totalPages}, cards {_cards.Length}");
        if (_page < 0) _page = 0;
        if (_page > totalPages) _page = totalPages;

        if (_cards == null) return;

        if (lastPage == _page) return;

        for(int i = 0; i < _colCardPlates.Length; ++i)
        {
          
            int idx = i + ((_colCardPlates.Length) * _page);
            if (idx >= _cards.Length)
            {
                _colCardPlates[i].ChangeCard(new GameObject(), -1, amount, 0);
                continue;
            }
            Card card = _cards[idx];
            //if (card == null) continue;

            GameObject cardGameobject = _cardBuilder.SpawnCard(card, _gameManager);
            Destroy(cardGameobject.GetComponent<CardDragDrop>());
           
            _colCardPlates[i].ChangeCard(cardGameobject,idx, amount, _cardCounts[idx]);
        }
    }

}
