using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageFillerController : MonoBehaviour
{
    public GameObject _prefab_with_imageFiller;
    public ImageFiller[] _fillers = new ImageFiller[1];
    public bool _go_left = true;
    public float _cap = 50;
    public float _fillRate = 100;
    int _current = 1;

    float _with;
    private void Awake()
    {
        _current = _fillers.Length-1;
        _with = _prefab_with_imageFiller.GetComponent<RectTransform>().rect.width;
    }
  
    public void SetCurrent(int amount)
    {
        _current = amount;
    }

    public int GetFillersSize()
    {
        return _fillers.Length;
    }

    public void SetAmount(int amount)
    {
        ImageFiller[] fillers = new ImageFiller[amount];
        int size = _fillers.Length;
        if(_fillers != null)
        {
            if (_fillers.Length > fillers.Length)
            {
                size = fillers.Length;
                for (int i = size; i < _fillers.Length; ++i)
                {
                    Destroy(_fillers[i].gameObject);
                }
                if (_current > size)
                    _current = size - 1;
            }


            for (int i = 0; i < size; ++i)
            {
                fillers[i] = _fillers[i];
            }
        }
        

        for(int i = 0; i < fillers.Length; ++i)
        {
            GameObject obj;
            if(i >= _fillers.Length)
            {
                obj = Instantiate(_prefab_with_imageFiller, this.transform.position, Quaternion.identity, this.transform);
                fillers[i] = obj.GetComponent<ImageFiller>();
                fillers[i].SetEmpty();
            }
            else
            {
                obj = fillers[i].gameObject;
                obj.transform.parent = this.transform;               
            }
           
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector3((_go_left ? -i : i) *( _with * 0.5f + _cap), 0, 1);
            fillers[i].SetFillRate(_fillRate);
        }

        _fillers = fillers;
        
    }

    public void FillAll()
    {
        for(int i = 0; i < _fillers.Length; ++i)
        {
            _fillers[i].FillFull();
        }
        _current = _fillers.Length - 1;
    }

    public void Drain(int amount)
    {
        if (_current < 0)
        {
            _current = 0;
        }

        int start = _current;
        int looping = _current;
        for(int i = looping; i > looping - amount; --i)
        {
            int nextID = i - 1;
            _current = nextID;
            if (nextID < 0)
            {
                break;
            }
               
            if(i != (looping - amount + 1))
                _fillers[i]._next = _fillers[nextID].DrainImage();

            ;
        }
        StartCoroutine(_fillers[start].DrainImage());
    }

    public void Fill(int amount)
    {
        if(_current >= _fillers.Length-1)
        {
            _current--;
        }

        int start = _current+1;
        int looping = _current+1;
        for (int i = looping; i < looping + amount; ++i)
        {
            _current = i;
            int nextID = i + 1;
            if (nextID > _fillers.Length-1)
            {
                break;
            }

            if (i != (looping + amount - 1))
                _fillers[i]._next = _fillers[nextID].FillImage();

        }
        if (_current > _fillers.Length - 1)
            _current = _fillers.Length - 1;

        StartCoroutine(_fillers[start].FillImage());
    }

    
}
