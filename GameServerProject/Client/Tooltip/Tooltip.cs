using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//[ExecuteInEditMode]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI _headerField;
    public TextMeshProUGUI _headerField_2;
    public TextMeshProUGUI _contentField;
    public TextMeshProUGUI _contentField_2;
    public LayoutElement _layoutElement;

    public int _characterWrapLimit;

    public RectTransform _rectTranform;

    private void Awake()
    {
        _rectTranform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "", string content2 = "", string header2 = "")
    {       
        ContentChecker(_headerField, header);
        ContentChecker(_headerField_2, header2);
        ContentChecker(_contentField_2, content2);

        _contentField.text = content;

        this.gameObject.transform.localScale = Vector3.zero;
        this.gameObject.transform.LeanScale(new Vector3(1, 1, 1), 0.3f);
        //int headerLenght = _headerField.text.Length;
        //int contentLenght = _contentField.text.Length;
        //_layoutElement.enabled = (headerLenght > _characterWrapLimit || contentLenght > _characterWrapLimit);
    }

    void ContentChecker(TextMeshProUGUI obj, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            obj.gameObject.SetActive(false);
        }
        else
        {
            obj.gameObject.SetActive(true);
            obj.text = content;
        }
    }

    private void Update()
    {
        Vector2 pos = Input.mousePosition;
       

        float pivoX = pos.x / Screen.width;
        float pivotY = pos.y / Screen.height;

        _rectTranform.pivot = new Vector2(pivoX, pivotY);
        transform.position = pos;
    }

}
