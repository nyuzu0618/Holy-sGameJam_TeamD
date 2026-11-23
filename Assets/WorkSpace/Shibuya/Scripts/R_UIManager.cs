
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class R_UIManager : MonoBehaviour
{
    // =========================
    // Singleton
    // =========================

    public static R_UIManager Instance { get; private set; }


    // =========================
    // UI Management
    // =========================

    private List<GameObject> uiObjects = new List<GameObject>();


    // =========================
    // UI Prefabs
    // =========================

    [Header("Basic UI Prefabs")]
    [SerializeField] private GameObject sampleUIPrefab;
    [SerializeField] private GameObject titleUIPrefab;

    [Header("Customer UI Prefabs")]
    [SerializeField] private GameObject customerAUIprefab;

    [Header("DreamFood UI Prefabs")]
    [SerializeField] private GameObject dreamFoodAPrefab;
    [SerializeField] private GameObject dreamFoodBPrefab;

    [Header("Food UI Prefabs")]
    [SerializeField] private GameObject foodAPrefab;
    [SerializeField] private GameObject foodBPrefab;
    [SerializeField] private GameObject foodCPrefab;
    [SerializeField] private GameObject foodDPrefab;
    [SerializeField] private GameObject foodEPrefab;
    [SerializeField] private GameObject foodFPrefab;
    [SerializeField] private GameObject foodGPrefab;
    [SerializeField] private GameObject foodHPrefab;
    [SerializeField] private GameObject foodIPrefab;
    [SerializeField] private GameObject foodJPrefab;
    [SerializeField] private GameObject foodKPrefab;
    [SerializeField] private GameObject foodLPrefab;

    [Header("Fade UI Prefabs")]
    [SerializeField] private GameObject fadeInUIPrefab;
    [SerializeField] private GameObject fadeOutUIPrefab;


    // =========================
    // Awake
    // =========================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        CheckEventSystem();
    }


    // =========================
    // OnDestroy
    // =========================

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }


    // =========================
    // Scene Loaded
    // =========================

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckEventSystem();
    }


    // =========================
    // EventSystem
    // =========================

    private void CheckEventSystem()
    {
        EventSystem[] eventSystems =
            FindObjectsByType<EventSystem>(FindObjectsSortMode.None);

        foreach (EventSystem eventSystem in eventSystems)
        {
            if (eventSystem.transform.IsChildOf(transform))
                continue;

            Destroy(eventSystem.gameObject);
        }
    }


    // =========================
    // Create / Destroy UI
    // =========================

    /// <summary>
    /// UIを生成して管理リストに登録する
    /// </summary>
    private GameObject CreateUI(GameObject uiPrefab)
    {
        if (uiPrefab == null)
        {
            Debug.LogError("UI Prefabが設定されていません。");
            return null;
        }

        GameObject uiObject = Instantiate(uiPrefab);

        uiObjects.Add(uiObject);

        return uiObject;
    }

    /// <summary>
    /// UIを破棄して管理リストから削除する
    /// </summary>
    public void DestroyUI(GameObject uiObject)
    {
        if (uiObject == null)
            return;

        if (uiObjects.Remove(uiObject))
        {
            Destroy(uiObject);
        }
    }


    // =========================
    // Basic UI
    // =========================

    public GameObject CreateSampleUI()
    {
        return CreateUI(sampleUIPrefab);
    }

    public GameObject CreateTitleUI()
    {
        return Instantiate(titleUIPrefab);
    }


    // =========================
    // Customer UI
    // =========================

    /// <summary>
    /// CustomerAのUIを生成する
    /// </summary>
    public GameObject CreateCustomerAUI()
    {
        return CreateUI(customerAUIprefab);
    }


    // =========================
    // DreamFood UI
    // =========================

    /// <summary>
    /// DreamFoodに対応したUIを生成する
    /// </summary>
    public GameObject CreateDreamFoodUI(DreamFood dreamFood)
    {
        switch (dreamFood)
        {
            case DreamFood.A:
                return CreateUI(dreamFoodAPrefab);

            case DreamFood.B:
                return CreateUI(dreamFoodBPrefab);

            default:
                Debug.LogError(
                    "対応するDreamFood UIがありません : " + dreamFood
                );
                return null;
        }
    }


    // =========================
    // Food UI
    // =========================

    /// <summary>
    /// FoodTypeに対応したUIを生成する
    /// </summary>
    public GameObject CreateFoodUI(FoodType foodType)
    {
        switch (foodType)
        {
            case FoodType.A:
                return CreateUI(foodAPrefab);

            case FoodType.B:
                return CreateUI(foodBPrefab);

            case FoodType.C:
                return CreateUI(foodCPrefab);

            case FoodType.D:
                return CreateUI(foodDPrefab);

            case FoodType.E:
                return CreateUI(foodEPrefab);

            case FoodType.F:
                return CreateUI(foodFPrefab);

            case FoodType.G:
                return CreateUI(foodGPrefab);

            case FoodType.H:
                return CreateUI(foodHPrefab);

            case FoodType.I:
                return CreateUI(foodIPrefab);

            case FoodType.J:
                return CreateUI(foodJPrefab);

            case FoodType.K:
                return CreateUI(foodKPrefab);

            case FoodType.L:
                return CreateUI(foodLPrefab);

            default:
                Debug.LogError(
                    "対応するFood UIがありません : " + foodType
                );
                return null;
        }
    }


    // =========================
    // Fade Functions
    // =========================

    public void PlayFadeIn()
    {
        GameObject fadeUI = CreateUI(fadeInUIPrefab);

        if (fadeUI != null)
        {
            StartCoroutine(DestroyUIAfterDelay(fadeUI));
        }
    }

    public void PlayFadeOut()
    {
        GameObject fadeUI = CreateUI(fadeOutUIPrefab);

        if (fadeUI != null)
        {
            StartCoroutine(DestroyUIAfterDelay(fadeUI));
        }
    }

    /// <summary>
    /// 0.5秒後にUIを自動破棄する
    /// </summary>
    private IEnumerator DestroyUIAfterDelay(GameObject uiObject)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        DestroyUI(uiObject);
    }
}
