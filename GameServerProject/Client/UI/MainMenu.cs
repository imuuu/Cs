using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject oCardCollection;
    GameManager _gameManager;
    UIManager _uiManager;
    [SerializeField] InputField _usernameField;
    private void Awake()
    {
        _gameManager = GameObject.FindObjectOfType<GameManager>();
        _uiManager = UIManager._instance;
    }
    public void PlayGame() 
    {
        int[] cardIDs = _gameManager.GetCardIds();
        if (cardIDs == null)
        {
            print("CARD IDS NULL");
            return;
        }
        //ClientSend.SendUsername(_usernameField.text);
        ClientSend.SendPlayerDecks(cardIDs);
        ClientSend.OnJoinAndLeaveQueue(true);
        Debug.Log("Play the game");
        EnableMainMenu(false);
        _uiManager.StartWaitingForOpponent();
        
    }

    void EnableMainMenu(bool b)
    {
        for (int i = 0; i < this.transform.childCount - 1; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(b);
        }
        this.transform.GetChild(this.transform.childCount - 1).gameObject.SetActive(b == false ? true : false);
    }

    public void CancelPlayGame()
    {
        ClientSend.OnJoinAndLeaveQueue(false);
        EnableMainMenu(true);
        _uiManager.StopWaitingForOpponent();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

   

    public void CardCollection()
    {
        if(oCardCollection.activeSelf)
        {
            oCardCollection.SetActive(false);
        }else
        {
            oCardCollection.SetActive(true);
            
        }
    }





}