using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TableAreaTrigger : AreaTrigger
{
    public override void OnCardDrop(AreaTriggerDrop areaTriggerDrop)
    {
        ClientSend.OnAreaTableTriggerDrop(areaTriggerDrop);
       //ClientSend.DeckDropCardToHolder(this, areaTriggerDrop._card);
    }
}
