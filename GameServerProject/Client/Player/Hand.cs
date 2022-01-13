using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] GameObject _cardPrefab;
    [SerializeField] GameObject _handTwo;
    [SerializeField] GameObject _being_dragged;
    float _cardWitdh = 0;
    //float _cardHeight = 0;
    float _cap  = 120;
    float _defcap;

    private void Awake()
    {
        _cardWitdh = _cardPrefab.GetComponent<RectTransform>().sizeDelta.x;
        _cardWitdh *= _cardPrefab.GetComponent<RectTransform>().localScale.x;
        _defcap = _cap;
    }

    public void SetBeingDraggedCard(GameObject cardGameobject)
    {
        _being_dragged = cardGameobject;
    }

    public GameObject GetBeingDraggedCard()
    {
        return _being_dragged;
    }

    public Vector2[] GetCardPositions(float scaleFactor, int cardCount, float cap)
    {       
        _cap = _defcap * scaleFactor;
        //Debug.Log($"_cap: {_cap} defCap : {_defcap} , sclae: {scaleFactor}");
        //Debug.Log($"Card witdh: {_cardWitdh} local scale : {_cardPrefab.transform.localScale} , cap: {_cap}");
        float cardWith = _cardWitdh;
        if (cardCount > 1)
            cardWith += cap * 0.5f;

        float total_leight = cardWith * cardCount;
        Vector2[] cardPositions = new Vector2[cardCount];
        for (int i = 0; i < cardCount; ++i)
        {
            cardPositions[i] = new Vector2((total_leight * -0.5f) + (cardWith * 0.5f) + (cardWith * i), 0);
        }
        return cardPositions;
    }


    public void UpdateHandUI(GameObject[] cards, int totalCardsInHand)
    {
        SpawnCards(this.transform, cards, totalCardsInHand);
        //float cardWith = _cardWitdh;

        //if (totalCardsInHand <= 0)
        //{
        //    return;
        //}

        //GameObject[] cs = new GameObject[totalCardsInHand];
        //int idx = 0;
        //for(int i = 0; i < cards.Length; ++i)
        //{
        //    if(cards[i] != null)
        //    {
        //        cs[idx] = cards[i];
        //        idx++;
        //    }              
        //}
        //if (totalCardsInHand > 1)
        //    cardWith += _cap * 0.5f;

        //float total_leight = cardWith * totalCardsInHand;

        //for(int i = 0; i < totalCardsInHand; ++i)
        //{
        //    RectTransform rectTransform = cs[i].GetComponent<RectTransform>();
        //    rectTransform.SetParent(this.transform);
        //    rectTransform.anchoredPosition = new Vector2 ((total_leight * -0.5f) + (cardWith * 0.5f) + (cardWith * i), 0);
        //    cs[i].GetComponent<CardDragDrop>().setLastRectAnchorPoint(rectTransform.position);
        //}
        
    }

    public float MakeScaleFactor(GameObject obj)
    {
        
        GameObject fakeScaleObj = new GameObject();
        fakeScaleObj.transform.SetParent(obj.transform);
        float scaleFactor = fakeScaleObj.transform.localScale.x;
        //Debug.Log($"Scale factor: {scaleFactor}");
        Destroy(fakeScaleObj);
        return scaleFactor;
    }
    void SpawnCards(Transform offsetTransform, GameObject[] cards, int totalCardCount)
    {
        if (cards.Length == 0)
            return;

        //GameObject[] cs = new GameObject[cards.Length];
        Vector2[] cardPositions = GetCardPositions(MakeScaleFactor(this.gameObject),totalCardCount, 120);
        int realIndex = 0;
        for (int i = 0; i < cards.Length; ++i)
        {
            if (cards[i] == null)
                continue;

            RectTransform rectTransform = cards[i].GetComponent<RectTransform>();
            rectTransform.SetParent(offsetTransform);
            rectTransform.anchoredPosition = cardPositions[realIndex];
            cards[i].GetComponent<CardDragDrop>().setLastRectAnchorPoint(rectTransform.position);
            realIndex++;
        }
    }

    public void UpdateHandUIOpponent(GameObject[] cards, int totalCardsInHand)
    {
        //SpawnCards(_handTwo.transform, cards, totalCardsInHand);
        
        
        //float cardWith = _cardWitdh;

        //if (totalCardsInHand <= 0)
        //{
        //    return;
        //}

        //GameObject[] cs = new GameObject[totalCardsInHand];
        //int idx = 0;
        //for (int i = 0; i < cards.Length; ++i)
        //{
        //    if (cards[i] != null)
        //    {
        //        cs[idx] = cards[i];
        //        idx++;
        //    }
        //}
        //if (totalCardsInHand > 1)
        //    cardWith += _cap * 0.5f;

        //float total_leight = cardWith * totalCardsInHand;

        //for (int i = 0; i < totalCardsInHand; ++i)
        //{

        //    RectTransform rectTransform = cs[i].GetComponent<RectTransform>();
        //    rectTransform.SetParent(_handTwo.transform);
        //    rectTransform.anchoredPosition = new Vector2((total_leight * -0.5f) + (cardWith * 0.5f) + (cardWith * i), 0);
        //    cs[i].GetComponent<CardDragDrop>().setLastRectAnchorPoint(rectTransform.position);

        //}
    }

    
}
