using System;
using UnityEngine;

public class R_ResultBUI : MonoBehaviour
{
    // =========================
    // Events
    // =========================

    public event Action ButtonClicked;


    // =========================
    // Button
    // =========================

    /// <summary>
    /// ボタン押下時
    /// </summary>
    public void OnButtonClick()
    {
        Debug.Log("Result B Button Clicked");

        ButtonClicked?.Invoke();
    }
}