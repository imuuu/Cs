using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager _instance;
    private RectTransform _ownGraveYardDeckButton;
    private RectTransform _opponentGraveYardDeckButton;
  
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Debug.Log("Instance already exists, destroy object!");
            Destroy(this);
        }
      
        _ownGraveYardDeckButton = GameObject.FindGameObjectWithTag("DeckGraveyard").transform.GetChild(0).GetComponent<RectTransform>();
        _opponentGraveYardDeckButton = GameObject.FindGameObjectWithTag("EnemyDeckGraveyard").transform.GetChild(0).GetComponent<RectTransform>();
        //Instantiate(new GameObject("Test objecti"), _graveYardDeckButton, Quaternion.identity);
    }
    public void PlayBattleHitAnimation(GameObject attacker, GameObject victim, Card aCard, Card vCard)
    {
        //attack
        attacker.transform.LeanMove(victim.transform.position, 0.5f).setEaseInExpo().setOnComplete(x => 
        {
            attacker.transform.LeanMove(attacker.GetComponent<CardDragDrop>().GetLastRectAnchorPoint(), 0.2f).setEaseInExpo();
            CheckDeath(attacker, aCard);
            CheckDeath(victim, vCard);

        });
    }

    public bool CheckDeath(GameObject cardObj, Card c)
    {
        bool attacker = c._ownerID == Client._instance._myId;
        if(c._defence <= 0)
        {
            float delay = 0.3f;
            if(attacker)
            {
                cardObj.transform.LeanMove(_ownGraveYardDeckButton.position, 2f).setEaseInOutExpo().delay = delay;
            }
            else
            {
                cardObj.transform.LeanMove(_opponentGraveYardDeckButton.position, 2f).setEaseInOutExpo().delay = delay;
            }
                
            cardObj.transform.LeanScale(new Vector3(0, 0, 0), 2f).setDestroyOnComplete(true).delay = delay;
            cardObj.GetComponent<CardUI>().GetTableHolder().GetComponent<TableCardHolder>().SetDropEnable(true);
            return true;
        }
        return false;
    }

   
}
