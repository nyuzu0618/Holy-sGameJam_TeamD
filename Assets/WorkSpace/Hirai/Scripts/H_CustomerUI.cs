
using UnityEngine;
using UnityEngine.UI;

public class H_CustomerUI : MonoBehaviour
{
    [Header("Star Sprites")]
    [SerializeField] private Sprite star3Sprite;
    [SerializeField] private Sprite star2Sprite;
    [SerializeField] private Sprite star1Sprite;

    [Header("Display Image")]
    [SerializeField] private Image displayImage;

    /// <summary>
    /// Starの値に応じてSpriteを切り替える
    /// </summary>
    public void SetImage(int Star)
    {
        switch (Star)
        {
            case 3:
                displayImage.sprite = star3Sprite;
                break;

            case 2:
                displayImage.sprite = star2Sprite;
                break;

            case 1:
                displayImage.sprite = star1Sprite;
                break;

            default:
                Debug.LogWarning("不正なStar値です : " + Star);
                break;
        }
    }
}
