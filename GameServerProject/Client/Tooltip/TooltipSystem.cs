using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem _current;

    public Tooltip _tooltip;

    private void Awake()
    {
        if(_current == null)
        {
            _current = this;
          
        }
        else
        {
            Debug.Log("Instance already exists, destroy object!");
            Destroy(this);
            return;
        }
       

    }

    

    public static void Show(string content, string header="", string content2 = "",string header2 = "")
    {
        _current._tooltip.SetText(content, header, content2, header2);
        _current._tooltip.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        _current._tooltip.gameObject.SetActive(false);
    }
}
