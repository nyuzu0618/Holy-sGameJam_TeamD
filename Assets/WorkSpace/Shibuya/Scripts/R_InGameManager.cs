
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
    [SerializeField] private CustomerA[] customerPrefabs;

    [Header("Spawn Position")]
    [SerializeField]
    private Vector3 customerSpawnPosition =
        new Vector3(-3f, 0f, 0f);

    private CustomerA currentCustomer;

    private DreamFood currentDreamFood;

    public DreamFood CurrentDreamFood => currentDreamFood;

    public CustomerA CurrentCustomer => currentCustomer;


    // =========================
    // Kitchen
    // =========================

    [Header("Kitchen Prefab")]
    [SerializeField] private KitchenA kitchenPrefab;

    private KitchenA currentKitchen;

    private FoodType currentFoodType;

    public FoodType CurrentFoodType => currentFoodType;

    public KitchenA CurrentKitchen => currentKitchen;


    // =========================
    // Result
    // =========================

    private int currentStar;

    public int CurrentStar => currentStar;


    // =========================
    // Spawn Customer
    // =========================

    /// <summary>
    /// 客をランダムに1体スポーン
    /// </summary>
    public CustomerA SpawnCustomer()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0)
        {
            Debug.LogError("Customer Prefabが登録されていません。");
            return null;
        }

        int randomIndex = Random.Range(0, customerPrefabs.Length);

        CustomerA selectedCustomer = customerPrefabs[randomIndex];

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
        CustomerA customer = SpawnCustomer();

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
            // Kitchenで完成した料理をCustomerに渡す
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
}
