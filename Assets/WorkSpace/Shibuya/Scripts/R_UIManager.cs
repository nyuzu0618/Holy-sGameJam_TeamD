
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

    [Header("Result UI Prefabs")]
    [SerializeField] private GameObject resultSPrefab;
    [SerializeField] private GameObject resultAPrefab;
    [SerializeField] private GameObject resultBPrefab;


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

    private GameObject CreateUI(GameObject uiPrefab, Transform parent)
    {
        if (uiPrefab == null)
        {
            Debug.LogError("UI Prefabが設定されていません。");
            return null;
        }

        GameObject uiObject = Instantiate(uiPrefab, parent, false);

        uiObjects.Add(uiObject);

        return uiObject;
    }

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

    public GameObject CreateCustomerAUI()
    {
        return CreateUI(customerAUIprefab);
    }


    // =========================
    // DreamFood UI
    // =========================

    public GameObject CreateDreamFoodAUI()
    {
        return CreateUI(dreamFoodAPrefab);
    }

    public GameObject CreateDreamFoodAUI(Transform parent)
    {
        return CreateUI(dreamFoodAPrefab, parent);
    }

    public GameObject CreateDreamFoodBUI()
    {
        return CreateUI(dreamFoodBPrefab);
    }

    public GameObject CreateDreamFoodBUI(Transform parent)
    {
        return CreateUI(dreamFoodBPrefab, parent);
    }


    // =========================
    // Food UI
    // =========================

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
                Debug.LogError("対応するFood UIがありません : " + foodType);
                return null;
        }
    }


    // =========================
    // Result UI
    // =========================

    public GameObject CreateResultSUI()
    {
        return CreateUI(resultSPrefab);
    }

    public GameObject CreateResultAUI()
    {
        return CreateUI(resultAPrefab);
    }

    public GameObject CreateResultBUI()
    {
        return CreateUI(resultBPrefab);
    }


    // =========================
    // Fade Functions
    // =========================

    /// <summary>
    /// FadeInを生成する。
    /// 破棄はシーン遷移に任せる。
    /// </summary>
    public void PlayFadeIn()
    {
        CreateUI(fadeInUIPrefab);
    }

    /// <summary>
    /// FadeOutを生成し、1秒後に破棄する。
    /// </summary>
    public void PlayFadeOut()
    {
        GameObject fadeUI = CreateUI(fadeOutUIPrefab);

        if (fadeUI != null)
        {
            StartCoroutine(DestroyFadeOutAfterDelay(fadeUI));
        }
    }

    /// <summary>
    /// 1秒後にFadeOutの破棄を依頼する
    /// </summary>
    private IEnumerator DestroyFadeOutAfterDelay(GameObject fadeUI)
    {
        yield return new WaitForSecondsRealtime(1f);

        DestroyUI(fadeUI);
    }
}
