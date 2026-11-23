
using UnityEngine;
using System;

public class KitchenA : MonoBehaviour
{
    // =========================
    // Events
    // =========================

    public event Action<FoodType> KitchenPhaseEnded;

    // =========================
    // Kitchen Phase
    // =========================

    /// <summary>
    /// 調理フェーズ開始
    /// </summary>
    public void OnKitchenPhaseStart(DreamFood dreamFood)
    {
        Debug.Log("Kitchen Start : " + dreamFood);

        // 今後、調理処理を実装
    }

    /// <summary>
    /// 調理フェーズ終了
    /// </summary>
    public void OnKitchenPhaseEnd(FoodType foodType)
    {
        KitchenPhaseEnded?.Invoke(foodType);
    }
}
