using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionScaling : MonoBehaviour
{
    public int width;
    public int heigth;

    public void SetWidth(int newWidth)
    {
        width = newWidth;
    }

    public void SetHeight(int newHeight)
    {
        heigth = newHeight;
    }

    public void SetRes()
    {
        Screen.SetResolution(width, heigth, false);
    }
}
