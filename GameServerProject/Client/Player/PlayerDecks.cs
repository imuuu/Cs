using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecks : MonoBehaviour
{
    Dictionary<int, int[]> _decks = new Dictionary<int, int[]>();
    public int[] deck_Monster = new int[20];
    public int[] deck_LowCost = new int[20];
    public int[] deck_MidCost = new int[20];
    public int[] deck_HighCost = new int[20];
    public int[] deck_Class = new int[20];
    public int[] deck_Spell = new int[20];
    //public int[] deck_Mana = new int[20];

    private void Awake()
    {
        _decks[0] = deck_Monster;
        _decks[1] = deck_LowCost;
        _decks[2] = deck_MidCost;
        _decks[3] = deck_HighCost;
        _decks[4] = deck_Class;
        _decks[5] = deck_Spell;
    }
    public int[] GetALLDeckCardsArray()
    {
        int[] all_decks = new int[5 * 20];
        int state = 0;
        int deckcount = 0;
        for(int i = 0; i < all_decks.Length; ++i)
        {
            if (i != 0 && i % 20 == 0)
            {
                deckcount = 0;
                state++;
            }

            int[] deck = _decks[state];
            all_decks[i] = deck[deckcount];
            deckcount++;
        }
        return all_decks;
    }
}
