using UnityEngine;
using System;

public class R_TitleUI : MonoBehaviour
{
    public event Action ButtonClicked;

    // スタートボタンのOnClickから呼び出す
    public void OnButtonClick()
    {
        ButtonClicked?.Invoke();
    }
}