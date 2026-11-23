
using UnityEngine;
using UnityEngine.UI;

public class R_BackgroundAnimator : MonoBehaviour
{
    [Header("Background UI")]
    [SerializeField] private Image backgroundImage;

    [Header("Animation Settings")]
    [SerializeField] private float frameInterval = 0.2f;
    [SerializeField] private bool loop = true;

    [Header("Background Sprites")]
    [SerializeField] private Sprite[] morningSprites;
    [SerializeField] private Sprite[] lunchSprites;
    [SerializeField] private Sprite[] dinnerSprites;

    private Sprite[] currentSprites;

    private int currentFrame;
    private float timer;

    private TimeLine currentTimeLine = TimeLine.None;

    private void Start()
    {
        SetBackTime(TimeLine.Morning);
    }

    private void Update()
    {
        if (currentSprites == null || currentSprites.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= frameInterval)
        {
            timer -= frameInterval;
            NextFrame();
        }
    }

    public void SetBackTime(TimeLine timeLine)
    {
        currentTimeLine = timeLine;

        switch (timeLine)
        {
            case TimeLine.None:
                currentSprites = null;
                backgroundImage.enabled = false;
                return;

            case TimeLine.Morning:
                currentSprites = morningSprites;
                break;

            case TimeLine.Lunch:
                currentSprites = lunchSprites;
                break;

            case TimeLine.Dinner:
                currentSprites = dinnerSprites;
                break;
        }

        if (currentSprites == null || currentSprites.Length == 0)
        {
            Debug.LogWarning(
                "Background Spriteが設定されていません: " + timeLine
            );

            backgroundImage.enabled = false;
            return;
        }

        backgroundImage.enabled = true;

        // アニメーションを先頭から再生
        currentFrame = 0;
        timer = 0f;

        backgroundImage.sprite = currentSprites[currentFrame];
    }

    private void NextFrame()
    {
        currentFrame++;

        if (currentFrame >= currentSprites.Length)
        {
            if (loop)
            {
                currentFrame = 0;
            }
            else
            {
                currentFrame = currentSprites.Length - 1;
                return;
            }
        }

        backgroundImage.sprite = currentSprites[currentFrame];
    }
}
