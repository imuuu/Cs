using System.Collections;
using UnityEngine;
public abstract class Deck : MonoBehaviour , IDeck
{
    public int _id = -1;
    public int _cost_gold = 0;
    public int _cost_mana = 0;
    public int _cost_health = 0;
    public int _cost_actionPoint = 0;
    public virtual void Clicked()
    {
        Debug.Log("virtaul");
        throw new System.NotImplementedException();
    }

    public bool IsOneBuyType()
    {
        int[] ar = new int[]{_cost_gold, _cost_health, _cost_mana};

        int count = 0;
        foreach(int i in ar)
        {
            if (i > 0)
                count++;
        }
        if (count > 1)
            return false;

        return true;
    }

    public DeckBuyType GetFirstBuyType()
    {
        if (_cost_gold > -1)
            return DeckBuyType.GOLD;

        if (_cost_health > -1)
            return DeckBuyType.HEALTH;

        if (_cost_mana > -1)
            return DeckBuyType.MANA;

        if (_cost_actionPoint > -1)
            return DeckBuyType.ACTION_POINT;

        return DeckBuyType.NONE;
    }

    
}