using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    private Player _owner;
    public int _id = -1;
    public Guid _uuid = Guid.Empty;
    public string _name;
    public string _description;

    public CardType _type;
    public CardFaction _faction;
    public CardClassType _classType = CardClassType.NONE;
    private CardState _state;

    //public int _goldCost;
    public int _manaCost;
    public int _attack;
    public int _defence;
    //public int _health;
    
    public Sprite _artWork;
    public int _ownerID;

    public void SetState(CardState state)
    {
        _state = state;
    }

    public CardState GetState()
    {
        return _state;
    }

    public void SetOwner(Player player)
    {
        _owner = player;
    }

    public Player GetOwner()
    {
        return _owner;
    }

}
