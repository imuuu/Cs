using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HeroButton : MonoBehaviour
{
    public int _heroID = 0;
    private TextMeshProUGUI _heroName;
    Hero _hero;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => ClickHero(_heroID));
        _heroName = this.transform.GetComponentInChildren<TextMeshProUGUI>();  
    }

    private void Start()
    {
        Hero hero = GameManager._instance.GetHeroCollection().GetHero(_heroID);
        _hero = hero;
        if (_hero != null)
            _heroName.text = _hero._name;

        ClickHero(_heroID);
    }
    public void ClickHero(int id)
    {
        //print("ClickHero icon");
        ClientSend.AskChosenHeroCards(id);
    }
}
