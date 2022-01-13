using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    public static LevelManager _instance;
    [SerializeField] private GameObject _loadCanvas;
    [SerializeField] private Image _progressBar;
    private float _target;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(this.gameObject);
        }
    }

    public async void LoadScene(Scene_enum sceneName)
    {
        _progressBar.fillAmount = 0;
        _target = 0;

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName.ToString());
        scene.allowSceneActivation = false;
        _loadCanvas.SetActive(true);
        StartCoroutine(StartFilling());
        do
        {
            await Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);
        //print("loaded!");
        _progressBar.fillAmount = 1;
        await Task.Delay(1000);
        scene.allowSceneActivation = true;
        _loadCanvas.SetActive(false);
        StopAllCoroutines();
        ClientSend.SceneLoaded(sceneName);
    }

    IEnumerator StartFilling()
    {
        while(_progressBar.fillAmount < 0.90)
        {
            _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 2 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
}
