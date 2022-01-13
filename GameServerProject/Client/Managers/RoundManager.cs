using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
public class RoundManager : MonoBehaviour
{
    public RoundInfoPanel _infoPanel;
    public TextMeshProUGUI _text_roundTime;
    public GameObject _buttonEndTurn;
    private float _roundTime = 0;
    private RoundState _state;
    public RoundInfoPanel GetInfoPanel()
    {
        return _infoPanel;
    }

    public void ShowTextInfoPanel(string text)
    {
        _infoPanel.gameObject.SetActive(true);
        _infoPanel.ShowText(text);
    }

    public void SetRoundTime(int roundTime)
    {
        _roundTime =(float) roundTime;
        _text_roundTime.gameObject.SetActive(true);
    }
    public void ShowStateButton(bool b)
    {
        _buttonEndTurn.SetActive(b);
    }
    public void SetState(RoundState state)
    {
        switch (state)
        {
            case RoundState.PURCHACE:
                {
                    _buttonEndTurn.GetComponentInChildren<TextMeshProUGUI>().text = "End Purchace State";
                    
                    break;
                }
            case RoundState.FIGHT:
                {
                    _buttonEndTurn.GetComponentInChildren<TextMeshProUGUI>().text = "End Your Turn";
                    break;
                }
        }
        _state = state;
    }
    public void ButtonPressEND_TURN()
    {
        ClientSend.SendMiniPacket(MiniPackets.ROUND_STATE, 0);
    }

    private void Update()
    {
        if(_roundTime <= 0)
        {
            _roundTime = 0;
            _text_roundTime.gameObject.SetActive(false);
            return;
        }
        int value;
        if (_roundTime > 0)
        {
            _roundTime -= Time.deltaTime;
            
            value = Mathf.CeilToInt(_roundTime);

            _text_roundTime.text = value.ToString();
        }

    }

    private async void ShowRoundTime(bool b)
    {
        if(!b)
        {
            _text_roundTime.gameObject.SetActive(false);
            _roundTime = 0;
            return;
        }

        float tester = 0;
        int value;
        while(_roundTime > 0)
        {
            _roundTime -= Time.deltaTime;
            if (_roundTime < 0)
                _roundTime = 0;

            value = Mathf.CeilToInt(_roundTime);

            if (tester == value)
                await Task.Yield();

            _text_roundTime.text = value.ToString();
            await Task.Yield();
           
        }
        _text_roundTime.gameObject.SetActive(false);
    }

}
