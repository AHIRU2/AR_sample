using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private RailMoveController railMoveController;

    [SerializeField,Header("経路用のパス群の元データ")]
    private RailPathData originRailPathData;　// 後程 List 化して複数のルートを管理できるようにします

    [SerializeField, Header("パスにおけるミッションの発生有無")] //Debug用
    private bool[] isMissionTriggers;


    // Start is called before the first frame update
    void Start()
    {
        // TODO ゲームの状態を準備中にする

        // TODO ルート用の経路情報を設定

        //RailMoveControllerの初期設定
        railMoveController.SetUpRailMoveController(this);

        //パスデータよりミッションの発生有無情報取得
        SetMissionTriggers();

        //次に再生するレール移動の目的地と経路のパスを設定
        railMoveController.SetNextRailPathData(originRailPathData);

        // TODO 経路の準備が完了するのを待つ（Start メソッドの戻り値をIEnumeratorに変更してコルーチンメソッドに変える)


        // TODO ゲームの状態をプレイ中に変更する
    }


    /// <summary>
    /// パスデータよりミッションの発生有無情報取得
    /// </summary>
    private void SetMissionTriggers()
    {
        //配列の初期化
        isMissionTriggers = new bool[originRailPathData.GetIsMissionTriggers().Length];

        //ミッション発生有無の情報を登録
        isMissionTriggers = originRailPathData.GetIsMissionTriggers();
    }


    public void CheckMissionTrigger(int index)
    {
        if (isMissionTriggers[index])
        {
            // TODOミッション発生
            Debug.Log("ミッション発生");

            //Debug用今はそのまま進行
            railMoveController.ResumeMove();

        }
        else
        {
            //ミッションなし、次のパスへ移動を再開
            railMoveController.ResumeMove();
        }
    }
}
