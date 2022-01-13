using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public GameObject _startMenu;

    public TextMeshProUGUI _connectINFO;
    public GameObject BattlefieldArea;
    private bool _findingOpponent = false;
    Coroutine _waitingForOpponent;
    [SerializeField] GameObject _mainMenu;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Debug.Log("Instance already exists, destroy object!");
            Destroy(this);
        }

        if (BattlefieldArea != null) BattlefieldArea.SetActive(false);


    }
    private void Start()
    {
        StartCoroutine(Connecting());
    }

    private void OnLevelWasLoaded(int level)
    {
        if(!Client._instance.IsConnected())
        {
            StartCoroutine(Connecting());
        }else
        {
            ConnectedToServer();
        }
        BattlefieldArea = GameObject.FindGameObjectWithTag("BattlefieldArea");
    }
    IEnumerator Connecting()
    {
        _mainMenu.SetActive(false);
        int i = 0;
        while(!Client._instance.IsConnected())
        {
            //Debug.Log("Waiting for connection...");

            Client._instance.ConnectedToServer();
            while (i < 4 )
            {
                string str = "Waiting for connection";
                str += string.Concat(Enumerable.Repeat(".", i++));
                _connectINFO.text = str;
                if (Client._instance.IsConnected()) break;
                yield return new WaitForSeconds(1f);
            }
            i = 0;
            //if (i > 3)
            //    i = 0;
           
            yield return new WaitForSeconds(1);
        }
        _connectINFO.text = "Connected!";
        Client._instance.InitializeClientData();
        ConnectedToServer();
        _mainMenu.SetActive(true);
        yield return null;
    }

    public void StartWaitingForOpponent()
    {
        _findingOpponent = true;
        _connectINFO.transform.parent.gameObject.SetActive(true);
        _waitingForOpponent = StartCoroutine(WaitingOpponent());
    }

    public void StopWaitingForOpponent()
    {
        _findingOpponent = false;
       
        StopCoroutine(_waitingForOpponent);
        _connectINFO.transform.parent.gameObject.SetActive(false);
    }

    IEnumerator WaitingOpponent()
    {
        int i = 0;
        while (_findingOpponent)
        {
            while (i < 4)
            {
                string str = "Waiting for Opponent";
                str += string.Concat(Enumerable.Repeat(".", i++));
                _connectINFO.text = str;
                //if (Client._instance.IsConnected()) break;
                yield return new WaitForSeconds(1f);
            }
            i = 0;

            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    public void ConnectedToServer()
    {
        _startMenu.SetActive(false);
        //_usernameField.interactable = false;
        _connectINFO.transform.parent.gameObject.SetActive(false);
        //Client._instance.ConnectedToServer();
       //his.gameObject.SetActive(false);
        
        if (BattlefieldArea != null) BattlefieldArea.SetActive(true);
    }

    
   
}
