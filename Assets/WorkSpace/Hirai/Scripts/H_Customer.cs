
using System;
using System.Collections;
using UnityEngine;

public class H_Customer : MonoBehaviour
{
    // =========================
    // Customer Data
    // =========================

    private string _name;
    private int _id;

    // 好きな料理
    [SerializeField]
    private FoodType[] favoriteFoods;

    // 嫌いな料理
    [SerializeField]
    private FoodType[] dislikedFoods;


    // =========================
    // UI
    // =========================

    private GameObject customerUI;
    private Canvas customerCanvas;

    private GameObject dreamFoodAUI;
    private GameObject dreamFoodBUI;

    private H_CustomerDeramAUIButton dreamFoodAButton;
    private H_CustomerDeramBUIButton dreamFoodBButton;


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

        CreateCustomerUI();
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

        customerUI = R_UIManager.Instance.CreateCustomerAUI();

        if (customerUI == null)
        {
            Debug.LogError("Customer UIの生成に失敗しました。");
            return;
        }

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
        if (customerCanvas == null)
        {
            Debug.LogError("Customer Canvasが取得されていません。");
            return;
        }

        // DreamFood A生成
        dreamFoodAUI = R_UIManager.Instance.CreateDreamFoodAUI(
            customerCanvas.transform
        );

        // DreamFood B生成
        dreamFoodBUI = R_UIManager.Instance.CreateDreamFoodBUI(
            customerCanvas.transform
        );

        // DreamFood Aボタン取得
        if (dreamFoodAUI != null)
        {
            dreamFoodAButton =
                dreamFoodAUI.GetComponent<H_CustomerDeramAUIButton>();

            if (dreamFoodAButton != null)
            {
                dreamFoodAButton.ButtonClicked += OnDreamFoodSelected;
            }
            else
            {
                Debug.LogError(
                    "DreamFood A UIにH_CustomerDeramAUIButtonがありません。"
                );
            }
        }

        // DreamFood Bボタン取得
        if (dreamFoodBUI != null)
        {
            dreamFoodBButton =
                dreamFoodBUI.GetComponent<H_CustomerDeramBUIButton>();

            if (dreamFoodBButton != null)
            {
                dreamFoodBButton.ButtonClicked += OnDreamFoodSelected;
            }
            else
            {
                Debug.LogError(
                    "DreamFood B UIにH_CustomerDeramBUIButtonがありません。"
                );
            }
        }
    }


    // =========================
    // DreamFood Selection
    // =========================

    /// <summary>
    /// DreamFood A/Bの選択イベント受信
    /// </summary>
    private void OnDreamFoodSelected(DreamFood food)
    {
        Debug.Log("DreamFood Selected : " + food);

        OnCustomerPhaseEnd(food);
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

        // ボタンイベント購読解除
        if (dreamFoodAButton != null)
        {
            dreamFoodAButton.ButtonClicked -= OnDreamFoodSelected;
        }

        if (dreamFoodBButton != null)
        {
            dreamFoodBButton.ButtonClicked -= OnDreamFoodSelected;
        }

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

        // 料理を受け取った直後に評価判定
        int star = GetStar(foodType);

        Debug.Log("Customer Evaluation : Star " + star);

        // CustomerUIのH_CustomerUIを取得
        if (customerUI != null)
        {
            H_CustomerUI customerUIComponent =
                customerUI.GetComponent<H_CustomerUI>();

            if (customerUIComponent != null)
            {
                // 評価画像を設定
                customerUIComponent.SetImage(star);
            }
            else
            {
                Debug.LogError(
                    "Customer UIにH_CustomerUIがありません。"
                );
            }
        }
        else
        {
            Debug.LogError("Customer UIが存在しません。");
        }

        // DreamFood A UI削除
        if (dreamFoodAUI != null)
        {
            if (R_UIManager.Instance != null)
            {
                R_UIManager.Instance.DestroyUI(dreamFoodAUI);
            }

            dreamFoodAUI = null;
        }

        // DreamFood B UI削除
        if (dreamFoodBUI != null)
        {
            if (R_UIManager.Instance != null)
            {
                R_UIManager.Instance.DestroyUI(dreamFoodBUI);
            }

            dreamFoodBUI = null;
        }

        // ボタン参照をクリア
        dreamFoodAButton = null;
        dreamFoodBButton = null;

        // 1秒後にEatPhase終了通知
        StartCoroutine(DelayedEatPhaseEnd(star));
    }


    /// <summary>
    /// 料理の評価を判定する
    /// </summary>
    private int GetStar(FoodType foodType)
    {
        // 好きな料理なら3
        if (favoriteFoods != null)
        {
            foreach (FoodType favoriteFood in favoriteFoods)
            {
                if (favoriteFood == foodType)
                {
                    return 3;
                }
            }
        }

        // 嫌いな料理なら1
        if (dislikedFoods != null)
        {
            foreach (FoodType dislikedFood in dislikedFoods)
            {
                if (dislikedFood == foodType)
                {
                    return 1;
                }
            }
        }

        // それ以外なら2
        return 2;
    }


    /// <summary>
    /// 1秒待ってから食事フェーズを終了する
    /// </summary>
    private IEnumerator DelayedEatPhaseEnd(int star)
    {
        yield return new WaitForSeconds(1f);

        OnCustomerEatPhaseEnd(star);
    }


    /// <summary>
    /// 食事終了・評価
    /// </summary>
    public void OnCustomerEatPhaseEnd(int star)
    {
        Debug.Log("Customer Eat Phase End : Star " + star);

        CustomerEatPhaseEnded?.Invoke(star);
    }


    // =========================
    // OnDestroy
    // =========================

    private void OnDestroy()
    {
        // イベント購読解除
        if (dreamFoodAButton != null)
        {
            dreamFoodAButton.ButtonClicked -= OnDreamFoodSelected;
        }

        if (dreamFoodBButton != null)
        {
            dreamFoodBButton.ButtonClicked -= OnDreamFoodSelected;
        }
    }
}
