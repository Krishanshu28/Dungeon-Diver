using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingMenu : MonoBehaviour
{
    public GameObject[] Scheme;
    public GameObject EscCanvas;
    public GameObject BindingCanvas;
    private int current = 0;

    public void Back()
    {
        EscCanvas.SetActive(true);
        BindingCanvas.SetActive(false);
    }
    public void Next()
    {
        if (current == 2) 
        {
            return;
        }
        Scheme[current].SetActive(false);
        current++;
        Scheme[current].SetActive(true);
    }
    public void Previous()
    {
        if (current == 0)
        {
            return;
        }
        Scheme[current].SetActive(false);
        current--;
        Scheme[current].SetActive(true);
    }
}

