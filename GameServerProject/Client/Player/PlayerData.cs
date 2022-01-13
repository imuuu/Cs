using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int _id;

    public string _username;

    public int _money = 0;
    public int _moneyIncome = 0;
    public int _mana = 0;
    public int _manaCap = 1;
    private int _actionPoints = 5;
    public int _round_actionsPoints = 0;
    public int _handSize = 0;
    public int _heroHealth;
    public int _victoryPoints = 0;

    public void PrintData()
    {
        Debug.Log($"id: {_id}, " +
            $"username: {_username}, " +
            $"money: {_money}, " +
            $"moneyIncome: {_moneyIncome}, " +
            $"mana: {_mana}, " +
            $"manaCap: {_manaCap}, " +
            $"actionPoints: {_actionPoints}, " +
            $"victoryPoints: {_victoryPoints}, " +
            $"roundACpoints: {_round_actionsPoints}, " +
            $"heroHealth: {_heroHealth}");
    }

    public void SetActionPoints(int points)
    {
        _actionPoints = points;
    }
}
