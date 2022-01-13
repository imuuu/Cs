using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public interface ICardUI
{
    TextMeshProUGUI _nameText { get; set; }
    TextMeshProUGUI _descriptionText { get; set; }
    TextMeshProUGUI _manaCostText { get; set; }
    TextMeshProUGUI _cardClassTypeText { get; set; }
    Image _indicatorBg { get; set; }
    Card _card { get; set; }
    Sprite _sprite { get; set; }

    GameObject _tableHolder { get; set; }
    GameManager _gameManager { get; set; }
}

  