
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class R_UIManager : MonoBehaviour
{
    // Singleton
    public static R_UIManager Instance { get; private set; }

    // 生成したUIを管理
    private List<GameObject> uiObjects = new List<GameObject>();

    [Header("UI Prefabs")]
    [SerializeField] private GameObject sampleUIPrefab;
    [SerializeField] private GameObject titleUIPrefab;
    [SerializeField] private GameObject dreamFoodAPrefab;
    [SerializeField] private GameObject dreamFoodBPrefab;

    [Header("Fade UI Prefabs")]
    [SerializeField] private GameObject fadeInUIPrefab;
    [SerializeField] private GameObject fadeOutUIPrefab;

    private void Awake()
    {
        // Singletonチェック
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        // シーン移動時のイベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 初回チェック
        CheckEventSystem();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    /// <summary>
    /// シーン読み込み完了時
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckEventSystem();
    }

    /// <summary>
    /// EventSystemの重複チェック
    /// </summary>
    private void CheckEventSystem()
    {
        EventSystem[] eventSystems = FindObjectsByType<EventSystem>(
            FindObjectsSortMode.None
        );

        foreach (EventSystem eventSystem in eventSystems)
        {
            // UIManagerの子にあるEventSystemは残す
            if (eventSystem.transform.IsChildOf(transform))
                continue;

            // それ以外は破棄
            Destroy(eventSystem.gameObject);
        }
    }

    /// <summary>
    /// UI生成・管理リスト登録
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
    /// UI破棄・管理リスト削除
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
    // UI生成関数
    // =========================

    public GameObject CreateSampleUI()
    {
        return CreateUI(sampleUIPrefab);
    }

    public GameObject CreateTitleUI()
    {
        GameObject titleObject = Instantiate(titleUIPrefab);

        return titleObject;
    }

    // =========================
    // Fade Functions
    // =========================

    /// <summary>
    /// FadeIn UI生成
    /// </summary>
    public void PlayFadeIn()
    {
        GameObject fadeUI = CreateUI(fadeInUIPrefab);

        if (fadeUI != null)
        {
            StartCoroutine(DestroyUIAfterDelay(fadeUI));
        }
    }

    /// <summary>
    /// FadeOut UI生成
    /// </summary>
    public void PlayFadeOut()
    {
        GameObject fadeUI = CreateUI(fadeOutUIPrefab);

        if (fadeUI != null)
        {
            StartCoroutine(DestroyUIAfterDelay(fadeUI));
        }
    }

    /// <summary>
    /// 0.5秒後にUIを自動破棄
    /// </summary>
    private IEnumerator DestroyUIAfterDelay(GameObject uiObject)
    {
        yield return new WaitForSecondsRealtime(0.5f);

        DestroyUI(uiObject);
    }
}
