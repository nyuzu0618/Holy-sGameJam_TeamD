using System;
using UnityEngine;

public class R_ResultAUI : MonoBehaviour
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
        Debug.Log("Result A Button Clicked");

        ButtonClicked?.Invoke();
    }
}