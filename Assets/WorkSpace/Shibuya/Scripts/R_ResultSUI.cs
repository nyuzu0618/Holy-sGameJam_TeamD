using System;
using UnityEngine;

public class R_ResultSUI : MonoBehaviour
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
        Debug.Log("Result S Button Clicked");

        ButtonClicked?.Invoke();
    }
}