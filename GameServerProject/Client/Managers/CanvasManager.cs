using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class CanvasManager : MonoBehaviour
{
    public TextMeshProUGUI _textUsername1;
    public TextMeshProUGUI _textUsername2;
   
    public TextMeshProUGUI _textEndTurn;

    public ChoseCardTab _choseCardTab;
    public RoundManager _roundManager;
    public PlayerManager _playerManager;

    private void Awake()
    {
        _choseCardTab = transform.GetComponentInChildren<ChoseCardTab>();
        _roundManager = transform.GetComponentInChildren<RoundManager>();
    }

    public void ReloadScene()
    {
        Client._instance.Disconnect();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
       
    }

    public void SetPlayerManager(PlayerManager pManager)
    {

        _playerManager = pManager;
    }

    //public void SetManaCapText(int amount)
    //{
    //    _textManaCap.text = "("+amount.ToString() + ")";
    //}
   
}
