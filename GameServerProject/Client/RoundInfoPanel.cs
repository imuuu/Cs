using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoundInfoPanel : MonoBehaviour
{
    private CanvasGroup _background;
    private TextMeshProUGUI _infoText;

    private void Awake()
    {
        _background = transform.GetChild(0).GetComponent<CanvasGroup>();
        _infoText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void ShowText(string text)
    {
        _infoText.text = text;
        float time = 0.3f;
        _background.alpha = 0;
        _background.LeanAlpha(1, time);

        transform.localScale = new Vector3(0, 0, 0);
        transform.LeanScale(new Vector3(1, 1, 1), time).setEaseInOutQuart();
        LeanTween.delayedCall(0.8f, Close);
    }

    private void Close()
    {
        _background.LeanAlpha(0, 0.5f);
        transform.LeanScale(new Vector3(0, 0, 0), 0.5f).setEaseInOutExpo().setOnComplete(x =>
        {
            gameObject.SetActive(false);
        });
    }
}
