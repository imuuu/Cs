using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UICanvasManager : MonoBehaviour
{
    public static UICanvasManager _instance;
    [SerializeField] private TextMeshProUGUI _textMoney;
    [SerializeField] private TextMeshProUGUI _textIncomeMoney;
    [SerializeField] private TextMeshProUGUI _textActionPoints;
    [SerializeField] private TextMeshProUGUI _textVictoryPoints;
    [SerializeField] private ImageFillerController _manaController;


    private void Awake()
    {
        if(_instance != null)
        {
            Debug.Log("UI canvas already exist");
            Destroy(this);
        }
        _instance = this;
    }

    public void SetMoneyText(int amount)
    {
        _textMoney.text = amount.ToString() + ": Money";
    }
    public void SetIncomeMoneyText(int amount)
    {
        _textIncomeMoney.text = amount.ToString() + ": Income";
    }

    public void SetManaText(int amount)
    {
        print("Mana setted");
        //_textMana.text = $"({_playerManager.GetManaCap()}) {amount} : mana";
       
    }

    public void SetManaCap(int amount)
    {
        if(_manaController.GetFillersSize() != amount)
            _manaController.SetAmount(amount);
    }

    public void ReFillMana()
    {
        _manaController.FillAll();
    }

    public void DrainMana(int amount)
    {
        _manaController.Drain(amount);
    }

    public void SetActionPointText(int amount)
    {
        _textActionPoints.text = $"{amount} : Actions";
    }

    public void SetVictoryPoints(int amount)
    {
        _textVictoryPoints.text = $"{amount} : VictoryCards";
    }


}
