
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class R_GameManagerBase : MonoBehaviour
{
    // =========================
    // Scene Change
    // =========================

    private bool isChangingScene = false;

    // 次のシーンでFadeOutを再生するか
    private static bool playFadeOutOnNextScene = false;

    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // =========================
    // Scene Loaded
    // =========================

    /// <summary>
    /// シーン読み込み完了時
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!playFadeOutOnNextScene)
            return;

        playFadeOutOnNextScene = false;

        if (R_UIManager.Instance != null)
        {
            R_UIManager.Instance.PlayFadeOut();
        }
    }

    // =========================
    // Scene Change
    // =========================

    /// <summary>
    /// シーン移動リクエスト
    /// </summary>
    public void RequestSceneChange(string sceneName)
    {
        if (isChangingScene)
            return;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("シーン名が指定されていません。");
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("移動先のシーンが登録されていません: " + sceneName);
            return;
        }

        StartCoroutine(SceneChangeCoroutine(sceneName));
    }

    /// <summary>
    /// FadeIn後にシーン移動
    /// </summary>
    private IEnumerator SceneChangeCoroutine(string sceneName)
    {
        isChangingScene = true;

        // FadeIn再生
        if (R_UIManager.Instance != null)
        {
            R_UIManager.Instance.PlayFadeIn();
        }

        // 0.5秒待機
        yield return new WaitForSecondsRealtime(0.5f);

        // 次のシーンでFadeOutを再生する
        playFadeOutOnNextScene = true;

        // シーン移動
        SceneManager.LoadScene(sceneName);
    }
}
