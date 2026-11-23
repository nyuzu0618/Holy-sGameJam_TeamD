using System;
using UnityEngine;
using UnityEngine.UI;

// このスクリプトを付けたオブジェクトに Image が必須であることを保証する
[RequireComponent(typeof(Image))]
public class SevenFrameAnimator : MonoBehaviour
{
    [Header("アニメーション設定")]
    [SerializeField] private float frameInterval = 0.15f; // 1コマあたりの秒数
    [SerializeField] private bool loop = true;            // ループ再生するか
    [SerializeField] private bool playOnStart = true;     // 開始と同時に自動再生するか

    [Header("アニメーション画像（7枚設定）")]
    [SerializeField] private Sprite[] frames = new Sprite[7];

    // ループしない場合、7枚目まで再生し終わった瞬間に通知するイベント
    public event Action OnAnimationFinished;

    private Image targetImage; // インスペクター欄から消して内部専用にする
    private int currentFrame = 0;
    private float timer = 0f;
    private bool isPlaying = false;

    private void Awake()
    {
        // 自分自身についている Image コンポーネントを自動で取得する
        targetImage = GetComponent<Image>();
    }

    private void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    private void Update()
    {
        if (!isPlaying || frames == null || frames.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= frameInterval)
        {
            timer -= frameInterval;
            NextFrame();
        }
    }

    // アニメーションを1枚目から再生開始する
    public void Play()
    {
        if (frames == null || frames.Length == 0)
        {
            Debug.LogWarning("アニメーション用の画像（frames）が設定されていません。");
            return;
        }

        currentFrame = 0;
        timer = 0f;
        isPlaying = true;

        targetImage.enabled = true;
        targetImage.sprite = frames[currentFrame];
    }

    // アニメーションを一時停止する
    public void Stop()
    {
        isPlaying = false;
    }

    // コマを1つ進める処理
    private void NextFrame()
    {
        currentFrame++;

        if (currentFrame >= frames.Length)
        {
            if (loop)
            {
                currentFrame = 0;
            }
            else
            {
                currentFrame = frames.Length - 1;
                isPlaying = false;
                OnAnimationFinished?.Invoke();
                return;
            }
        }

        targetImage.sprite = frames[currentFrame];
    }

    public void AnimationStart()
    {
        Play();
    }
}