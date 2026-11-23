using System;
using UnityEngine;


public class H_Customer : MonoBehaviour
{
    //テキスト
    private GameObject CustomerDreamUI; //
    private string name;                //名前
    private int id;                     //キャラクターのid
   

    //選んだ食材を渡すイベント？
    public event Action<DreamFood> CustomerPhaseEnded;

   
    //客の話を聞くフェーズ
    public void OnCustomerPhaseStart()
    {
        Debug.Log("Customer Phase Start");
        //客が来た時になんか話す。



        //夢のUIを表示する。

        //  R_UIManager.Instance.CreateDreamFoodAUI();
        //  R_UIManager.Instance.CreateDreamFoodBUI();

        //夢の食材を選ぶ 


    }

    //客の夢を選んだと伝える。
    public void OnCustomerPhaseEnd(DreamFood food)
    {
        
        CustomerPhaseEnded?.Invoke(food);
    }

    //できた料理を食べるフェーズ
    public void OnCustomerEatPhaseStart(FoodType foodType)
    {
        Debug.Log("Customer Eat Phase Start : " + foodType);
    }

    //食べ終わって、感想を言う。
    public void OnCustomerEatPhaseEnd(int Star)
    {
        Debug.Log("Customer Eat Phase End");

    }
}
