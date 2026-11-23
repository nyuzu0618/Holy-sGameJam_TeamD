
using System;
using UnityEngine;

public class H_Customer : MonoBehaviour
{
    // =========================
    // Customer Data
    // =========================

    private string _name;
    private int _id;


    // =========================
    // UI
    // =========================

    private GameObject customerUI;

    private Canvas customerCanvas;

    private GameObject dreamFoodAUI;
    private GameObject dreamFoodBUI;


    // =========================
    // Events
    // =========================

    public event Action<DreamFood> CustomerPhaseEnded;

    public event Action<int> CustomerEatPhaseEnded;


    // =========================
    // Customer Visit Phase
    // =========================

    /// <summary>
    /// 客が来た時のフェーズ開始
    /// </summary>
    public void OnCustomerPhaseStart()
    {
        Debug.Log("Customer Phase Start");

        // Customer UI生成
        CreateCustomerUI();

        // DreamFood UI生成
        CreateDreamFoodUI();
    }


    // =========================
    // Create Customer UI
    // =========================

    /// <summary>
    /// Customer UIを生成してCanvasを取得
    /// </summary>
    private void CreateCustomerUI()
    {
        if (R_UIManager.Instance == null)
        {
            Debug.LogError("R_UIManagerが存在しません。");
            return;
        }

        // Customer UI生成
        customerUI = R_UIManager.Instance.CreateCustomerAUI();

        if (customerUI == null)
        {
            Debug.LogError("Customer UIの生成に失敗しました。");
            return;
        }

        // Customer UI内のCanvas取得
        customerCanvas = customerUI.GetComponentInChildren<Canvas>(true);

        if (customerCanvas == null)
        {
            Debug.LogError("Customer UI内にCanvasがありません。");
            return;
        }
    }


    // =========================
    // Create DreamFood UI
    // =========================

    /// <summary>
    /// Customer UIのCanvas内にDreamFood UIを生成
    /// </summary>
    private void CreateDreamFoodUI()
    {
        dreamFoodAUI = R_UIManager.Instance.CreateDreamFoodAUI(
            customerCanvas.transform
        );

        dreamFoodBUI = R_UIManager.Instance.CreateDreamFoodBUI(
            customerCanvas.transform
        );
    }


    // =========================
    // Customer Visit Phase End
    // =========================

    /// <summary>
    /// 客の夢を選んだ時
    /// </summary>
    public void OnCustomerPhaseEnd(DreamFood food)
    {
        Debug.Log("Customer Phase End : " + food);

        CustomerPhaseEnded?.Invoke(food);
    }


    // =========================
    // Customer Eat Phase
    // =========================

    /// <summary>
    /// 食事フェーズ開始
    /// </summary>
    public void OnCustomerEatPhaseStart(FoodType foodType)
    {
        Debug.Log("Customer Eat Phase Start : " + foodType);
    }


    /// <summary>
    /// 食事終了・評価
    /// </summary>
    public void OnCustomerEatPhaseEnd(int star)
    {
        Debug.Log("Customer Eat Phase End : Star " + star);

        CustomerEatPhaseEnded?.Invoke(star);
    }
}
