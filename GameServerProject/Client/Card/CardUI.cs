using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour, ICardUI
{
    public TextMeshProUGUI _nameText { get; set; }
    public TextMeshProUGUI _descriptionText { get; set; }
    public TextMeshProUGUI _manaCostText { get; set; }
    public TextMeshProUGUI _cardClassTypeText { get; set; }
    public Image _indicatorBg { get; set; }
    public Card _card { get; set; }
    public Sprite _sprite { get; set; }
    public GameObject _tableHolder { get; set; }
    public GameManager _gameManager { get; set; }

    private void Awake()
    {
        GetTextMeshTexts();
        DisableIndicatorBg();
        if (_card != null)
        {
            UpdateCardUI();
        }
        _gameManager = GameManager._instance;
    }
    virtual protected void GetTextMeshTexts()
    {
        _indicatorBg = transform.GetChild(0).GetComponent<Image>();
        _nameText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        _descriptionText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        _manaCostText = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        _cardClassTypeText = transform.GetChild(5).GetComponent<TextMeshProUGUI>();

    }
    //public void SetGameManager(GameManager gManager)
    //{
    //    _gameManager = gManager;
    //    //GetComponent<CardDragDrop>().SetGameManager(gManager);
    //}
    public Card GetCard()
    {
        return _card;
    }
    public void SetTableHolder(GameObject o)
    {
        _tableHolder = o;
    }

    public GameObject GetTableHolder()
    {
        return _tableHolder;
    }

    public void SetCardUIData(Card card)
    {
        if (card == null)
        {
            Debug.Log("card is null");
            return;
        }
        _card = card;
        if (card._artWork != null)
            _sprite = card._artWork;

        UpdateCardUI();
    }

    virtual protected void UpdateCardUI()
    {
        //if (_card == null)
        //{
        //    _nameText.text = "null";
        //    _descriptionText.text = "null";
        //    //_attackValueText.text = "null";
        //    //_defenceValueText.text = "null";
        //    _manaCostText.text = "null";
        //    _cardClassTypeText.text = "null";
        //    //_goldCostText.text = "null";
        //    return;
        //}
        _nameText.text = _card._name;
        _descriptionText.text = _card._description;
        //_attackValueText.text = _card._attack.ToString();
        //_defenceValueText.text = _card._defence.ToString();
        _manaCostText.text = _card._manaCost.ToString();
        _cardClassTypeText.text = _card._classType.ToString();
        //_goldCostText.text = _card._goldCost.ToString();
        //Debug.Log("Card has been updated: " + _card._name+ "health: "+_card._defence);
    }
    virtual public void SetAsCover()
    {
        _nameText.text = "";
        _descriptionText.text = "";
        //_attackValueText.text = "";
        //_defenceValueText.text = "";
        _manaCostText.text = "";
        _cardClassTypeText.text = "";
        //_goldCostText.text = "";

        Image img = this.gameObject.GetComponentInChildren<Image>();
        img.color = Color.black;
    }
    public void SetIndicatorBg(Color color)
    {
        _indicatorBg.color = color;
    }

    public void EnableIndicatorBg()
    {
        _indicatorBg.enabled = true;
    }

    public void DisableIndicatorBg()
    {
        _indicatorBg.enabled = false;
    }
}
