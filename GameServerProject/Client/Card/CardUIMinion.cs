using UnityEngine;
using TMPro;


public class CardUIMinion : CardUI, ICardUI
{

    TextMeshProUGUI _attackValueText;
    TextMeshProUGUI _defenceValueText;

    protected override void UpdateCardUI()
    {
        base.UpdateCardUI();
        
        if (_attackValueText != null)
            _attackValueText.text = _card._attack.ToString();
        if (_defenceValueText != null)
            _defenceValueText.text = _card._defence.ToString();
    }

    public override void SetAsCover()
    {
        base.SetAsCover();

        if (_attackValueText != null)
            _attackValueText.text = _card._attack.ToString();
        if (_defenceValueText != null)
            _defenceValueText.text = _card._defence.ToString();
    }

    protected override void GetTextMeshTexts()
    {
        base.GetTextMeshTexts();
        _attackValueText = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        _defenceValueText = transform.GetChild(7).GetComponent<TextMeshProUGUI>();
    }



}
