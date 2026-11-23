
using UnityEngine;

public class R_InGameManager : R_GameManagerBase
{
    // =========================
    // Game Phase
    // =========================

    public enum GamePhase
    {
        None,
        CustomerVisit,
        Kitchen,
        CustomerEat,
        Result
    }

    [Header("Game Phase")]
    [SerializeField] private GamePhase currentPhase = GamePhase.None;

    public GamePhase CurrentPhase => currentPhase;


    // =========================
    // Customer
    // =========================

    [Header("Customer Prefabs")]
    [SerializeField] private H_Customer[] customerPrefabs;

    [Header("Spawn Position")]
    [SerializeField]
    private Vector3 customerSpawnPosition =
        new Vector3(-3f, 0f, 0f);

    private H_Customer currentCustomer;

    private DreamFood currentDreamFood;

    public DreamFood CurrentDreamFood => currentDreamFood;

    public H_Customer CurrentCustomer => currentCustomer;


    // =========================
    // Kitchen
    // =========================

    [Header("Kitchen Prefab")]
    [SerializeField] private R_Kitchen kitchenPrefab;

    private R_Kitchen currentKitchen;

    private FoodType currentFoodType;

    public FoodType CurrentFoodType => currentFoodType;

    public R_Kitchen CurrentKitchen => currentKitchen;


    // =========================
    // Result
    // =========================

    private int currentStar;

    public int CurrentStar => currentStar;

    private GameObject currentResultUI;

    private R_ResultSUI currentResultSUI;
    private R_ResultAUI currentResultAUI;
    private R_ResultBUI currentResultBUI;


    // =========================
    // Start
    // =========================

    public void Start()
    {
        PhaseChange(GamePhase.CustomerVisit);
    }


    // =========================
    // Spawn Customer
    // =========================

    /// <summary>
    /// 客をランダムに1体スポーン
    /// </summary>
    public H_Customer SpawnCustomer()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0)
        {
            Debug.LogError("Customer Prefabが登録されていません。");
            return null;
        }

        int randomIndex = Random.Range(0, customerPrefabs.Length);

        H_Customer selectedCustomer = customerPrefabs[randomIndex];

        if (selectedCustomer == null)
        {
            Debug.LogError("選択されたCustomer PrefabがNullです。");
            return null;
        }

        // 客を生成
        currentCustomer = Instantiate(
            selectedCustomer,
            customerSpawnPosition,
            Quaternion.identity
        );

        // Customerイベント購読
        currentCustomer.CustomerPhaseEnded += CustomerVisitPhaseEnd;
        currentCustomer.CustomerEatPhaseEnded += CustomerEatPhaseEnd;

        return currentCustomer;
    }


    // =========================
    // Phase Change
    // =========================

    /// <summary>
    /// ゲームフェーズ変更
    /// </summary>
    public void PhaseChange(GamePhase nextPhase)
    {
        if (currentPhase == nextPhase)
            return;

        currentPhase = nextPhase;

        Debug.Log("GamePhase Changed : " + currentPhase);

        switch (currentPhase)
        {
            case GamePhase.None:
                OnNonePhaseStart();
                break;

            case GamePhase.CustomerVisit:
                OnCustomerVisitPhaseStart();
                break;

            case GamePhase.Kitchen:
                OnKitchenPhaseStart();
                break;

            case GamePhase.CustomerEat:
                OnCustomerEatPhaseStart();
                break;

            case GamePhase.Result:
                OnResultPhaseStart();
                break;
        }
    }


    // =========================
    // Phase Start Functions
    // =========================

    private void OnNonePhaseStart()
    {
        Debug.Log("None Phase Start");
    }

    private void OnCustomerVisitPhaseStart()
    {
        Debug.Log("CustomerVisit Phase Start");

        // Customer生成
        H_Customer customer = SpawnCustomer();

        // Customerフェーズ開始
        if (customer != null)
        {
            customer.OnCustomerPhaseStart();
        }
    }

    private void OnKitchenPhaseStart()
    {
        Debug.Log("Kitchen Phase Start");

        // Prefabチェック
        if (kitchenPrefab == null)
        {
            Debug.LogError("Kitchen Prefabが登録されていません。");
            return;
        }

        // Kitchen生成（原点）
        currentKitchen = Instantiate(
            kitchenPrefab,
            Vector3.zero,
            Quaternion.identity
        );

        // イベント購読
        currentKitchen.KitchenPhaseEnded += KitchenPhaseEnd;

        // 希望料理を渡してKitchen開始
        currentKitchen.OnKitchenPhaseStart(currentDreamFood);
    }

    private void OnCustomerEatPhaseStart()
    {
        Debug.Log("CustomerEat Phase Start");

        // Customer食事フェーズ開始
        if (currentCustomer != null)
        {
            currentCustomer.OnCustomerEatPhaseStart(currentFoodType);
        }
        else
        {
            Debug.LogError("CurrentCustomerが存在しません。");
        }
    }

    private void OnResultPhaseStart()
    {
        Debug.Log("Result Phase Start");

        Debug.Log("Earned Star : " + currentStar);

        if (R_UIManager.Instance == null)
        {
            Debug.LogError("R_UIManagerが存在しません。");
            return;
        }

        // Star数に応じてResult UI生成
        switch (currentStar)
        {
            case 3:
                currentResultUI = R_UIManager.Instance.CreateResultSUI();

                if (currentResultUI != null)
                {
                    currentResultSUI =
                        currentResultUI.GetComponent<R_ResultSUI>();

                    if (currentResultSUI != null)
                    {
                        currentResultSUI.ButtonClicked += OnResultButtonClicked;
                    }
                    else
                    {
                        Debug.LogError("Result S UIにR_ResultSUIがありません。");
                    }
                }
                break;

            case 2:
                currentResultUI = R_UIManager.Instance.CreateResultAUI();

                if (currentResultUI != null)
                {
                    currentResultAUI =
                        currentResultUI.GetComponent<R_ResultAUI>();

                    if (currentResultAUI != null)
                    {
                        currentResultAUI.ButtonClicked += OnResultButtonClicked;
                    }
                    else
                    {
                        Debug.LogError("Result A UIにR_ResultAUIがありません。");
                    }
                }
                break;

            case 1:
                currentResultUI = R_UIManager.Instance.CreateResultBUI();

                if (currentResultUI != null)
                {
                    currentResultBUI =
                        currentResultUI.GetComponent<R_ResultBUI>();

                    if (currentResultBUI != null)
                    {
                        currentResultBUI.ButtonClicked += OnResultButtonClicked;
                    }
                    else
                    {
                        Debug.LogError("Result B UIにR_ResultBUIがありません。");
                    }
                }
                break;

            case 0:
                Debug.Log("Star 0 : Result UIなし");
                break;

            default:
                Debug.LogWarning("想定外のStar数です : " + currentStar);
                break;
        }
    }


    // =========================
    // Phase End Functions
    // =========================

    /// <summary>
    /// 客フェーズ終了イベント受信
    /// </summary>
    private void CustomerVisitPhaseEnd(DreamFood dreamFood)
    {
        // イベント購読解除
        if (currentCustomer != null)
        {
            currentCustomer.CustomerPhaseEnded -= CustomerVisitPhaseEnd;
        }

        // 希望料理を保存
        currentDreamFood = dreamFood;

        // Kitchenフェーズへ移行
        PhaseChange(GamePhase.Kitchen);
    }

    /// <summary>
    /// Kitchenフェーズ終了イベント受信
    /// </summary>
    private void KitchenPhaseEnd(FoodType foodType)
    {
        // イベント購読解除
        if (currentKitchen != null)
        {
            currentKitchen.KitchenPhaseEnded -= KitchenPhaseEnd;
        }

        // 完成料理を保存
        currentFoodType = foodType;

        Debug.Log("Kitchen Phase End : " + currentFoodType);

        // CustomerEatフェーズへ移行
        PhaseChange(GamePhase.CustomerEat);
    }

    /// <summary>
    /// Customer食事フェーズ終了イベント受信
    /// </summary>
    private void CustomerEatPhaseEnd(int star)
    {
        // イベント購読解除
        if (currentCustomer != null)
        {
            currentCustomer.CustomerEatPhaseEnded -= CustomerEatPhaseEnd;
        }

        // 獲得Starを保存
        currentStar = star;

        Debug.Log("Customer Eat Phase End : Star " + currentStar);

        // Resultフェーズへ移行
        PhaseChange(GamePhase.Result);
    }


    // =========================
    // Result Button
    // =========================

    /// <summary>
    /// Result UIのボタン押下イベント受信
    /// </summary>
    private void OnResultButtonClicked()
    {
        Debug.Log("Result Button Clicked");

        // イベント購読解除
        if (currentResultSUI != null)
        {
            currentResultSUI.ButtonClicked -= OnResultButtonClicked;
        }

        if (currentResultAUI != null)
        {
            currentResultAUI.ButtonClicked -= OnResultButtonClicked;
        }

        if (currentResultBUI != null)
        {
            currentResultBUI.ButtonClicked -= OnResultButtonClicked;
        }

        // タイトルシーンへ移動
        RequestSceneChange("Title");
    }
}
