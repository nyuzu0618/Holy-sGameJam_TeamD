using UnityEngine;
using System;

public class H_CustomerDeramBUIButton : MonoBehaviour
{
    public event Action<DreamFood> ButtonClicked;

    public void OnClick()
    {
        Debug.Log("DreamFood B Clicked");

        ButtonClicked?.Invoke(DreamFood.B);
    }
}