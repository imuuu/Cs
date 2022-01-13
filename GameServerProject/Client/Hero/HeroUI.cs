using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class HeroUI: MonoBehaviour, IDropHandler
{
    public int id = -1;
  
    private TextMeshProUGUI _textName;
    private TextMeshProUGUI _textHp;
    Hero _hero;

    public GameObject _abilityGameobject;
    TooltipTrigger _abilityTooltipTrigger;
    private void Awake()
    {
        _textName = this.gameObject.transform.Find("T_Name").GetComponent<TextMeshProUGUI>();
        _textHp = this.gameObject.transform.Find("T_Hp").GetComponent<TextMeshProUGUI>();
        if (id == 1)
        {
            _abilityGameobject.GetComponentInChildren<Button>().onClick.AddListener(TriggerClassAbility);
        }  
        _abilityTooltipTrigger = _abilityGameobject.GetComponentInChildren<TooltipTrigger>();

    }

    public Hero GetHero()
    {
        return _hero;
    }

    public void SetData(Hero hero, int id)
    {
        Debug.Log("data: " + id);
        SetName(hero._name);
        SetHp(hero._hp);

        if(hero._ability != null)
        {

            SetAbility(hero._ability);
        }
            

        _hero = hero;
    }
    public void SetName(string name)
    {
        _textName.text = name;
    }

    public void SetHp(int hp)
    {
        _textHp.text = hp.ToString();
    }


    public void SetAbility(HeroAbility ability)
    {
        _abilityTooltipTrigger._header = ability.GetTitle();
        _abilityTooltipTrigger._content = ability.GetDesc();
        _abilityTooltipTrigger._header_2 = ability.GetPassive()._title;
        _abilityTooltipTrigger._content2 = ability.GetPassive()._description;
    }

    public void TriggerClassAbility()
    {
        ClientSend.TriggerClassAbility();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            GameObject o = eventData.pointerDrag;
            CardUI cardUI = o.GetComponent<CardUI>();
            if (cardUI == null)
                return;

            Card card = cardUI.GetCard();

            if (card.GetState() == CardState.FRONTLINE || card.GetState() == CardState.BACKLINE)
            {
                ClientSend.HitHero(_hero._uuid,card);
                
            }

        }
    }
}
