using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static LTDescr _delay;
    public string _header;
    public string _header_2;
    
    [Multiline()]
    public string _content;
    [Multiline()]
    public string _content2;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _delay = LeanTween.delayedCall(0.5f, () =>
        {
            TooltipSystem.Show(_content, _header, _content2, _header_2);
        });
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(_delay.uniqueId);
        TooltipSystem.Hide();
    }
}
