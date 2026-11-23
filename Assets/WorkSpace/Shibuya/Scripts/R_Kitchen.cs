
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Collections;

public class R_Kitchen : MonoBehaviour
{
    // =========================
    // Events
    // =========================

    public event Action<FoodType> KitchenPhaseEnded;


    // =========================
    // Cooking Tool
    // =========================

    public enum CookingTool
    {
        None,
        Pot,
        Pan
    }


    // =========================
    // UI References
    // =========================

    [SerializeField] private GameObject kitchenUIPrefab;

    [Header("Ingredient Buttons")]
    [SerializeField] private Button baseFoodAUI;
    [SerializeField] private Button baseFoodBUI;
    [SerializeField] private Button baseFoodCUI;

    [Header("Cooking Areas")]
    [SerializeField] private RectTransform potUI;
    [SerializeField] private RectTransform panUI;

    [Header("Canvas")]
    [SerializeField] private Canvas canvas;


    // =========================
    // Kitchen Data
    // =========================

    private DreamFood currentDreamFood;

    private BaseFood currentBaseFood = BaseFood.None;

    private CookingTool currentCookingTool = CookingTool.None;

    private FoodType currentFoodType;

    private bool isDragging = false;

    private bool isKitchenEnded = false;

    // 料理完成後の待機中か
    private bool isKitchenEnding = false;

    private Image dragImage;

    private RectTransform dragImageRect;


    // =========================
    // Kitchen Phase Start
    // =========================

    /// <summary>
    /// 調理フェーズ開始
    /// </summary>
    public void OnKitchenPhaseStart(DreamFood dreamFood)
    {
        Debug.Log("Kitchen Start : " + dreamFood);

        currentDreamFood = dreamFood;

        currentBaseFood = BaseFood.None;
        currentCookingTool = CookingTool.None;

        isKitchenEnded = false;
        isKitchenEnding = false;
        isDragging = false;

        DestroyDragImage();

        // UI登録チェック
        if (baseFoodAUI == null ||
            baseFoodBUI == null ||
            baseFoodCUI == null ||
            potUI == null ||
            panUI == null ||
            canvas == null)
        {
            Debug.LogError("Kitchen UIの登録が不足しています。");
        }
    }


    // =========================
    // Update
    // =========================

    private void Update()
    {
        if (isKitchenEnded || isKitchenEnding)
            return;

        if (Mouse.current == null)
            return;

        // ドラッグ開始
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckDragStart();
        }

        // ドラッグ中
        if (isDragging && Mouse.current.leftButton.isPressed)
        {
            UpdateDragPosition();
        }

        // ドラッグ終了
        if (isDragging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            CheckDrop();
        }
    }


    // =========================
    // Drag Start
    // =========================

    /// <summary>
    /// 食材ボタンの押下判定
    /// </summary>
    private void CheckDragStart()
    {
        if (canvas == null)
            return;

        if (TryStartDrag(baseFoodAUI, BaseFood.A))
            return;

        if (TryStartDrag(baseFoodBUI, BaseFood.B))
            return;

        TryStartDrag(baseFoodCUI, BaseFood.C);
    }


    /// <summary>
    /// ドラッグ開始処理
    /// </summary>
    private bool TryStartDrag(Button button, BaseFood food)
    {
        if (button == null || button.image == null)
            return false;

        RectTransform buttonRect = button.GetComponent<RectTransform>();

        Camera camera = GetCanvasCamera();

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (!RectTransformUtility.RectangleContainsScreenPoint(
            buttonRect,
            mousePosition,
            camera))
        {
            return false;
        }

        currentBaseFood = food;

        CreateDragImage(button.image);

        isDragging = true;

        return true;
    }


    // =========================
    // Drag Image
    // =========================

    /// <summary>
    /// ドラッグ用画像生成
    /// </summary>
    private void CreateDragImage(Image sourceImage)
    {
        GameObject imageObject = new GameObject("DragImage");

        imageObject.transform.SetParent(canvas.transform, false);

        dragImage = imageObject.AddComponent<Image>();

        dragImage.sprite = sourceImage.sprite;
        dragImage.preserveAspect = true;
        dragImage.raycastTarget = false;

        dragImageRect = dragImage.GetComponent<RectTransform>();

        dragImageRect.sizeDelta = sourceImage.rectTransform.rect.size;

        dragImageRect.SetAsLastSibling();

        UpdateDragPosition();
    }


    /// <summary>
    /// ドラッグ画像をマウスに追従
    /// </summary>
    private void UpdateDragPosition()
    {
        if (dragImageRect == null)
            return;

        RectTransform canvasRect = canvas.transform as RectTransform;

        Camera camera = GetCanvasCamera();

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            mousePosition,
            camera,
            out localPoint))
        {
            dragImageRect.anchoredPosition = localPoint;
        }
    }


    // =========================
    // Drag End / Drop
    // =========================

    /// <summary>
    /// ドロップ先判定
    /// </summary>
    private void CheckDrop()
    {
        isDragging = false;

        Camera camera = GetCanvasCamera();

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (potUI != null &&
            RectTransformUtility.RectangleContainsScreenPoint(
                potUI,
                mousePosition,
                camera))
        {
            currentCookingTool = CookingTool.Pot;
        }
        else if (panUI != null &&
            RectTransformUtility.RectangleContainsScreenPoint(
                panUI,
                mousePosition,
                camera))
        {
            currentCookingTool = CookingTool.Pan;
        }
        else
        {
            // ドロップ失敗
            Debug.Log("Drop Failed");

            DestroyDragImage();

            currentBaseFood = BaseFood.None;
            currentCookingTool = CookingTool.None;

            return;
        }

        // ドロップ成功
        Debug.Log(
            "Drop Success : " +
            currentBaseFood + " / " +
            currentCookingTool
        );

        // 料理判定
        CheckFoodType();

        DestroyDragImage();
    }


    // =========================
    // Food Type Check
    // =========================

    /// <summary>
    /// 食材・希望料理・調理器具からFoodTypeを決定
    /// </summary>
    private void CheckFoodType()
    {
        if (!TryGetFoodType(
            currentBaseFood,
            currentDreamFood,
            currentCookingTool,
            out currentFoodType))
        {
            Debug.LogError("FoodTypeの判定に失敗しました。");
            return;
        }

        Debug.Log("Completed FoodType : " + currentFoodType);

        // 料理完成後の処理を開始
        isKitchenEnding = true;

        // FoodTypeに対応したFoodUIを生成
        if (R_UIManager.Instance != null)
        {
            R_UIManager.Instance.CreateFoodUI(currentFoodType);
        }
        else
        {
            Debug.LogError("R_UIManagerが存在しません。");
        }

        // 1秒後にKitchenPhase終了
        StartCoroutine(DelayedKitchenPhaseEnd(currentFoodType));
    }


    /// <summary>
    /// FoodUI表示後、1秒待ってKitchenPhaseを終了
    /// </summary>
    private IEnumerator DelayedKitchenPhaseEnd(FoodType foodType)
    {
        yield return new WaitForSeconds(1f);

        OnKitchenPhaseEnd(foodType);
    }


    /// <summary>
    /// 12種類の料理分岐
    /// </summary>
    private bool TryGetFoodType(
        BaseFood baseFood,
        DreamFood dreamFood,
        CookingTool cookingTool,
        out FoodType foodType)
    {
        foodType = default;

        switch (baseFood)
        {
            case BaseFood.A:

                switch (dreamFood)
                {
                    case DreamFood.A:
                        if (cookingTool == CookingTool.Pot)
                            foodType = FoodType.A;
                        else if (cookingTool == CookingTool.Pan)
                            foodType = FoodType.B;
                        else
                            return false;
                        break;

                    case DreamFood.B:
                        if (cookingTool == CookingTool.Pot)
                            foodType = FoodType.C;
                        else if (cookingTool == CookingTool.Pan)
                            foodType = FoodType.D;
                        else
                            return false;
                        break;

                    default:
                        return false;
                }
                break;


            case BaseFood.B:

                switch (dreamFood)
                {
                    case DreamFood.A:
                        if (cookingTool == CookingTool.Pot)
                            foodType = FoodType.E;
                        else if (cookingTool == CookingTool.Pan)
                            foodType = FoodType.F;
                        else
                            return false;
                        break;

                    case DreamFood.B:
                        if (cookingTool == CookingTool.Pot)
                            foodType = FoodType.G;
                        else if (cookingTool == CookingTool.Pan)
                            foodType = FoodType.H;
                        else
                            return false;
                        break;

                    default:
                        return false;
                }
                break;


            case BaseFood.C:

                switch (dreamFood)
                {
                    case DreamFood.A:
                        if (cookingTool == CookingTool.Pot)
                            foodType = FoodType.I;
                        else if (cookingTool == CookingTool.Pan)
                            foodType = FoodType.J;
                        else
                            return false;
                        break;

                    case DreamFood.B:
                        if (cookingTool == CookingTool.Pot)
                            foodType = FoodType.K;
                        else if (cookingTool == CookingTool.Pan)
                            foodType = FoodType.L;
                        else
                            return false;
                        break;

                    default:
                        return false;
                }
                break;


            default:
                return false;
        }

        return true;
    }


    // =========================
    // Drag Image Destroy
    // =========================

    private void DestroyDragImage()
    {
        if (dragImage != null)
        {
            Destroy(dragImage.gameObject);
        }

        dragImage = null;
        dragImageRect = null;
    }


    // =========================
    // Canvas Camera
    // =========================

    private Camera GetCanvasCamera()
    {
        if (canvas == null)
            return null;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            return null;

        return canvas.worldCamera;
    }


    // =========================
    // Kitchen Phase End
    // =========================

    /// <summary>
    /// 調理フェーズ終了
    /// </summary>
    public void OnKitchenPhaseEnd(FoodType foodType)
    {
        if (isKitchenEnded)
            return;

        isKitchenEnded = true;

        KitchenPhaseEnded?.Invoke(foodType);

        Destroy(gameObject);
    }
}
