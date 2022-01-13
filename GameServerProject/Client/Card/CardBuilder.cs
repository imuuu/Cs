using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardBuilder : MonoBehaviour
{
    [SerializeField] GameObject _cardMinionPrefab;
    [SerializeField] GameObject _cardVictoryPrefab;
    [SerializeField] GameObject _cardSpellPrefab;
    //[SerializeField] Dictionary<int, Scriptable_Card> _allCards = new Dictionary<int, Scriptable_Card>();

    //void LoadCards()
    //{
    //    List<Scriptable_Card> sCards = Resources.LoadAll<Scriptable_Card>("").ToList();
    //
    //    for (int i = 0; i < sCards.Count; ++i)
    //    {        
    //        if(sCards[i]._id < 0)
    //        {
    //            Debug.Log("Card named: " + sCards[i]._name+" has not id set, please add id");
    //        }
    //        _allCards.Add(sCards[i]._id, sCards[i]);
    //    }
    //}
    public GameObject SpawnCard(Card card, GameManager gameManager)
    {

        GameObject o;
        switch (card._classType)
        {
            case CardClassType.NONE:
                o = Instantiate(_cardMinionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case CardClassType.Minion:
                o = Instantiate(_cardMinionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case CardClassType.Spell:
                o = Instantiate(_cardSpellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            case CardClassType.Victory:
                o = Instantiate(_cardVictoryPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                break;
            default:
                o = Instantiate(_cardMinionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                break;
        }
        o.GetComponent<CardUI>().SetCardUIData(card);
        //o.GetComponent<CardUI>().SetGameManager(gameManager);
        //SetCardData(sCard, o);
        //UpdateCard(o);

        return o;
    }
    public GameObject SpawnCard()
    {
        GameObject o = Instantiate(_cardMinionPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        return o;
    }




    //public void SetCardData(Scriptable_Card sCard, GameObject o)
    //{
    //    CardData data = o.GetComponent<CardData>();
    //    data._id = sCard._id;
    //    data._name = sCard._name;
    //    data._description = sCard._description;
    //    data._type = sCard._type;
    //    data._faction = sCard._faction;
    //    data._goldCost = sCard._goldCost;
    //    data._manaCost = sCard._manaCost;
    //    data._attack = sCard._attack;
    //    data._defence = sCard._defence;
    //    data._health = sCard._health;
    //    data._artWork = sCard._artWork;
    //}

    //public void UpdateCard(GameObject o)
    //{
    //    o.GetComponent<CardUI>().updateCardUI();
    //}

    //public Scriptable_Card GetScriptableCard(int id)
    //{
    //    return _allCards[id];
    //}
}
