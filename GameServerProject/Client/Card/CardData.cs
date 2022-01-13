using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public int _id { get; set; } = -1;
    public string _name { get; set; }
    public string _description { get; set; }

    public CardType _type { get; set; }
    public CardFaction _faction { get; set; }

    public int _goldCost { get; set; }
    public int _manaCost { get; set; }
    public int _attack { get; set; }
    public int _defence { get; set; }
    public int _health { get; set; }

    public Sprite _artWork { get; set; }


}
