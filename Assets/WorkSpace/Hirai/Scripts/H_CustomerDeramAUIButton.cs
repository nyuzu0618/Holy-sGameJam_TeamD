using System;
using UnityEngine;


public class H_CustomerDeramAUIButton : MonoBehaviour
{
    public event Action<DreamFood> ButtonClicked;

    public void OnClick()
    {
        Debug.Log("DreamFood A Clicked");

        ButtonClicked?.Invoke(DreamFood.A);
    }
}