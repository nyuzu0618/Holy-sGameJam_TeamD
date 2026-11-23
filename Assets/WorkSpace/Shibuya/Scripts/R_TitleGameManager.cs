using UnityEngine;

public class R_TitleGameManager : R_GameManagerBase
{
    private GameObject titleUIObject;
    private R_TitleUI titleUI;

    private void Start()
    {
        CreateTitleUI();
    }

    private void CreateTitleUI()
    {
        // UIManagerからGameObjectを取得
        titleUIObject = R_UIManager.Instance.CreateTitleUI();

        if (titleUIObject == null)
        {
            Debug.LogError("TitleUIの生成に失敗しました。");
            return;
        }

        // R_TitleUIにキャスト
        titleUI = titleUIObject.GetComponent<R_TitleUI>();

        if (titleUI == null)
        {
            Debug.LogError("R_TitleUIが見つかりません。");
            return;
        }

        // イベント購読
        titleUI.ButtonClicked += OnButtonClick;
    }

    // スタートボタン押下時
    private void OnButtonClick()
    {
        Debug.Log("Title : Game Start");

        // ゲーム開始処理
        RequestSceneChange("InGame");
    }

    private void OnDestroy()
    {
        if (titleUI != null)
        {
            titleUI.ButtonClicked -= OnButtonClick;
        }

        if (titleUIObject != null)
        {
            R_UIManager.Instance.DestroyUI(titleUIObject);
        }
    }
}