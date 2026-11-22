
using UnityEngine;
using System;

public class CustomerA : MonoBehaviour
{
    // =========================
    // Events
    // =========================

    // 客フェーズ終了イベント
    public event Action<DreamFood> CustomerPhaseEnded;

    // 客の食事フェーズ終了イベント
    public event Action<int> CustomerEatPhaseEnded;


    // =========================
    // Customer Phase
    // =========================

    /// <summary>
    /// 客フェーズ開始
    /// </summary>
    public void OnCustomerPhaseStart()
    {
        Debug.Log("Customer Phase Start");
    }

    /// <summary>
    /// 客フェーズ終了通知
    /// </summary>
    public void OnCustomerPhaseEnd(DreamFood food)
    {
        CustomerPhaseEnded?.Invoke(food);
    }


    // =========================
    // Customer Eat Phase
    // =========================

    /// <summary>
    /// 客の食事フェーズ開始
    /// </summary>
    public void OnCustomerEatPhaseStart(FoodType foodType)
    {
        Debug.Log("Customer Eat Phase Start : " + foodType);
    }

    /// <summary>
    /// 客の食事フェーズ終了
    /// </summary>
    public void OnCustomerEatPhaseEnd(int Star)
    {
        Debug.Log("Customer Eat Phase End");

        CustomerEatPhaseEnded?.Invoke(Star);
    }
}