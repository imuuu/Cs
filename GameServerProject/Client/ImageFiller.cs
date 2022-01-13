using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageFiller : MonoBehaviour
{
    public Image filler_image;
    float _fillRate = 0.1f;

    public IEnumerator _next;


    //public void Fill()
    //{
    //    filler_image.fillAmount = 0;
    //    StopAllCoroutines();
    //    StartCoroutine(FillImage());
    //}

    //public void Drain()
    //{
    //    filler_image.fillAmount = 1;
    //    StopAllCoroutines();
    //    StartCoroutine(DrainImage());
    //}
    public void SetFillRate(float rate)
    {
        _fillRate = rate;
    }
    public void FillFull()
    {
        filler_image.fillAmount = 1.0f;
    }

    public void SetEmpty()
    {
        filler_image.fillAmount = 0.0f;
    }

    public IEnumerator FillImage()
    {
        filler_image.fillAmount = 0.0f;
        StopAllCoroutines();

        while (filler_image.fillAmount < 1.0f)
        {
            filler_image.fillAmount += _fillRate;
            yield return new WaitForFixedUpdate();
        }

        if(_next != null)
            StartCoroutine(_next);

        filler_image.fillAmount = 1.0f;
        _next = null;
    }
    public IEnumerator DrainImage()
    {
        filler_image.fillAmount = 1.0f;
        StopAllCoroutines();

        while (filler_image.fillAmount > 0.0f)
        {
            filler_image.fillAmount -= _fillRate;
            yield return new WaitForFixedUpdate();
        }

        if (_next != null)
            StartCoroutine(_next);
        _next = null;
        filler_image.fillAmount = 0.0f;

    }
}
