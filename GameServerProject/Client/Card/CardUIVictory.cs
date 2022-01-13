using UnityEngine;
using TMPro;
public class CardUIVictory : CardUI
{
    TextMeshProUGUI _increaseIncome;
    protected override void UpdateCardUI()
    {
        base.UpdateCardUI();
        if (_increaseIncome != null && _card._attack > 0)
            _increaseIncome.text = _card._attack.ToString();
    }

    public override void SetAsCover()
    {
        base.SetAsCover();

        if (_increaseIncome != null)
        {

            _increaseIncome.text = _card._attack > 0 ? _card._attack.ToString() : "";
        }
           
    }

    protected override void GetTextMeshTexts()
    {
        base.GetTextMeshTexts();
        _increaseIncome = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        
    }
}
