using UnityEngine;

public class S_Kitchen : MonoBehaviour
{
    //キッチンUIを入れておく変数
    private GameObject KitchenUI;
    //キッチンUIのスクリプトを入れる
    //private 
    //使用したベースの食材
    private GameObject UsedBaseIngredients;
    //使用した夢の食材
    private GameObject UsedDreamIngredients;
    //使用した夢食材のイーナムを入れとく変数
    private DreamFood UsedDreamFood;
    //完成した料理
    private FoodType FinishedFood;
    //ベースの食材を入れるイーナム
    public enum BaseFood
    {
        None,
        A,
        B,
        C,
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ゲームマネージャからキッチンフェーズに変わったことを報告された
    public void OnKitchenPhase(DreamFood dreamFood)
    {
        //イーナムを受け止める
        UsedDreamFood = dreamFood;

        //UIマネージャにキッチンUIの生成を依頼する
        //まだないので一回空白処理はきた

        //ここまで
        //ゲームオブジェクトからスクリプトを見れるようにキャストするなど



    }
    public void OnKitchenPhaseEnd(FoodType foodType)
    {

    }
}
