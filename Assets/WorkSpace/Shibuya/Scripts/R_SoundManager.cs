
using UnityEngine;

public class R_SoundManager : MonoBehaviour
{
    // =========================
    // Singleton
    // =========================

    public static R_SoundManager Instance { get; private set; }

    // =========================
    // AudioSource
    // =========================

    private AudioSource bgmSource;
    private AudioSource seSource;

    // =========================
    // BGM
    // =========================

    [Header("BGM Clips")]
    [SerializeField] private AudioClip titleBGM;
    [SerializeField] private AudioClip gameBGM;
    [SerializeField] private AudioClip resultBGM;

    [Header("BGM Volume")]
    [SerializeField, Range(0f, 3f)]
    private float bgmVolumeMultiplier = 1f;

    [SerializeField, Range(0f, 1f)]
    private float bgmVolume = 1f;

    // =========================
    // SE
    // =========================

    [Header("SE Clips")]
    [SerializeField] private AudioClip sheepBleatingSE
;
    [SerializeField] private AudioClip messageDisplaySE
;
    [SerializeField] private AudioClip decisionSE;

    [Header("SE Volume")]
    [SerializeField, Range(0f, 3f)]
    private float seVolumeMultiplier = 1f;

    [SerializeField, Range(0f, 1f)]
    private float seVolume = 1f;

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

        // AudioSource生成
        bgmSource = gameObject.AddComponent<AudioSource>();
        seSource = gameObject.AddComponent<AudioSource>();

        // BGM設定
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        // SE設定
        seSource.loop = false;
        seSource.playOnAwake = false;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // =========================
    // Volume
    // =========================

    /// <summary>
    /// BGM音量更新
    /// </summary>
    private void UpdateBGMVolume()
    {
        bgmSource.volume = bgmVolume * bgmVolumeMultiplier;
    }

    /// <summary>
    /// SE音量取得
    /// </summary>
    private float GetSEVolume()
    {
        return Mathf.Clamp01(seVolume * seVolumeMultiplier);
    }

    /// <summary>
    /// BGM一律音量設定
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp01(volume);
        UpdateBGMVolume();
    }

    /// <summary>
    /// SE一律音量設定
    /// </summary>
    public void SetSEVolume(float volume)
    {
        seVolume = Mathf.Clamp01(volume);
    }

    /// <summary>
    /// BGM音量倍率設定
    /// </summary>
    public void SetBGMVolumeMultiplier(float multiplier)
    {
        bgmVolumeMultiplier = Mathf.Clamp(multiplier, 0f, 3f);
        UpdateBGMVolume();
    }

    /// <summary>
    /// SE音量倍率設定
    /// </summary>
    public void SetSEVolumeMultiplier(float multiplier)
    {
        seVolumeMultiplier = Mathf.Clamp(multiplier, 0f, 3f);
    }

    // =========================
    // Play BGM
    // =========================

    /// <summary>
    /// BGM再生
    /// 再生中のBGMと異なる場合のみ切り替える
    /// </summary>
    public void PlayBGM(AudioClip bgm)
    {
        if (bgm == null)
            return;

        // 同じBGMなら何もしない
        if (bgmSource.clip == bgm && bgmSource.isPlaying)
            return;

        // 現在のBGM停止
        bgmSource.Stop();

        // 新しいBGM再生
        bgmSource.clip = bgm;
        bgmSource.loop = true;

        UpdateBGMVolume();

        bgmSource.Play();
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // =========================
    // Play SE
    // =========================

    /// <summary>
    /// SE再生
    /// 重複再生可能
    /// </summary>
    public void PlaySE(AudioClip se)
    {
        if (se == null)
            return;

        seSource.PlayOneShot(se, GetSEVolume());
    }

    // =========================
    // BGM Functions
    // =========================

    public void PlayTitleBGM()
    {
        PlayBGM(titleBGM);
    }

    public void PlayGameBGM()
    {
        PlayBGM(gameBGM);
    }

    public void PlayResultBGM()
    {
        PlayBGM(resultBGM);
    }

    // =========================
    // SE Functions
    // =========================

    public void PlaySheepBleatingSE()
    {
        PlaySE(sheepBleatingSE);
    }

    public void PlayMessageDisplaySE()
    {
        PlaySE(messageDisplaySE);
    }

    public void PlayDecisionSE()
    {
        PlaySE(decisionSE);
    }
}
