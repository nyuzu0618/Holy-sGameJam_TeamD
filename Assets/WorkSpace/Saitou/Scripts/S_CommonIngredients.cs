using System;
using UnityEngine;
using UnityEngine.UI;
using static S_Kitchen;

public class S_CommonIngredients : MonoBehaviour
{
    //インスペクターからイーナムの番号をふる
    [SerializeField] private BaseFood MyFoodID;
    //情報を送る際に使うものを定義
    public event Action<BaseFood> OnClickBaseFood;
    //UIのコンポーネントのがたを使ったUIを生成
    private Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
