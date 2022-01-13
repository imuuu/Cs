using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    CanvasManager _canvas;
    private PlayerData _playerData;
    private void Awake()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasManager>();
    }
    public void SetPlayerData(PlayerData pData)
    {
        _canvas.SetPlayerManager(this);
        _playerData = pData;

        //UICanvasManager._instance.SetMoneyText(pData._money);
        //UICanvasManager._instance.SetIncomeMoneyText(pData._moneyIncome);
        ////UICanvasManager._instance.SetManaText(pData._mana);//will refresh manaCap too
        //UICanvasManager._instance.SetVictoryPoints(pData._victoryPoints);
        //UICanvasManager._instance.SetActionPointText(pData._round_actionsPoints);
        _canvas._textUsername1.text = pData._username;


        SetMoney(pData._money);
        SetMoneyIncome(pData._moneyIncome);
        //SetMana(pData._mana);
        SetUsername(pData._username);
        SetManaCap(pData._manaCap);
        SetRoundActionPoints(pData._round_actionsPoints);
        SetVictoryPoints(pData._victoryPoints);

    }
    public PlayerData GetPlayerData()
    {
        return _playerData;
    }
    public void SetUsername(string username)
    {
        _playerData._username = username;
        _canvas._textUsername1.text = username;
    }

    public void SetOpponetUsername(string username)
    {
        _canvas._textUsername2.text = username;
    }
   
    public void SetMana(int mana)
    {
        _playerData._mana = mana;
        UICanvasManager._instance.SetManaText(_playerData._mana);
    }

    public int GetMana()
    {
        return _playerData._mana;
    }

    public void AddMoney(int amount)
    {
        SetMoney(_playerData._money + amount);
    }
    public void SetMoney(int money)
    {
        _playerData._money = money;
        UICanvasManager._instance.SetMoneyText(money);
    }
    public int GetMoney()
    {
        return _playerData._money;
    }

    public void AddMoneyIncome(int amount)
    {
        SetMoneyIncome(_playerData._moneyIncome + amount);
    }

    public void SetMoneyIncome(int income)
    {
        _playerData._moneyIncome = income;
        UICanvasManager._instance.SetIncomeMoneyText(income);
    }

    public int GetMoneyIncome()
    {
        return _playerData._moneyIncome;
    }

    public int GetManaCap()
    {
        return _playerData._manaCap;
    }

    internal void SetManaCap(int manaCap)
    {
        _playerData._manaCap = manaCap;
        UICanvasManager._instance.SetManaCap(_playerData._manaCap); //will refresh manaCap too
    }

    public void SetRoundActionPoints(int amount)
    {
        _playerData._round_actionsPoints = amount;
        UICanvasManager._instance.SetActionPointText(amount);
    }
    public void SetVictoryPoints(int amount)
    {
        UICanvasManager._instance.SetVictoryPoints(amount);
    }
    public int GetVictoryPoints()
    {
        return _playerData._victoryPoints;
    }
    public int GetRoundActionPoints()
    {
        return _playerData._round_actionsPoints;
    }

    public int GetHeroHealth()
    {
        return _playerData._heroHealth;
    }
}
