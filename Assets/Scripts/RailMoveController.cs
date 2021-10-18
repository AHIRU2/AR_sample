using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class RailMoveController : MonoBehaviour
{
    [SerializeField]
    private Transform railMoveTarget; //レールを移動させる対象。カメラ、あるいはAR用のカメラ

    [SerializeField]
    private RailPathData currentRailPathData; //RailPathData クラスにアタッチされているRailPathDataゲームオブジェクトをアサイン。後で自動アサインに変更

    private Tween tween;

    private GameManager gameManager;



    /// <summary>
    /// RailMoveControllerの初期設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpRailMoveController(GameManager gameManager)
    {
        this.gameManager = gameManager;

        // TODO 他にもある場合には追記。必要に応じて引数を通じて外部から情報をもらうようにする
    }


    public void SetNextRailPathData(RailPathData nextRailPathData)
    {
        //目的地取得
        currentRailPathData = nextRailPathData;

        //移動開始
        StartCoroutine(StartRailMove());
    }


    //private void Start()
    //{
    //    //Debug用レールの移動の開始
    //    StartCoroutine(StartRailMove());

    //}


    public IEnumerator StartRailMove()
    {
        yield return null;

        ////移動する地点を取得するための配列の初期化
        //Vector3[] paths = new Vector3[currentRailPathData.GetPathTrans().Length];
        //float totalTime = 0;

        ////移動する位置情と時間を順番に配列に取得
        //for(int i = 0; i < currentRailPathData.GetPathTrans().Length; i++)
        //{
        //    paths[i] = currentRailPathData.GetPathTrans()[i].position;
        //    totalTime += currentRailPathData.GetRailMoveDurations()[i];
        //}

        //ラムダ式
        //移動する地点を取得するための配列の初期化
        Vector3[] paths = currentRailPathData.GetPathTrans().Select(x => x.position).ToArray();
        //移動する位置情と時間を順番に配列に取得
        float totalTime = currentRailPathData.GetRailMoveDurations().Sum();

        Debug.Log(totalTime);

        //パスによる移動開始
        tween = railMoveTarget.DOPath(paths, totalTime).SetEase(Ease.Linear).OnWaypointChange((waypointIndex) => CheckArrivalDsetination(waypointIndex));

        // TODO 他に必要な処理を追記

    }


    /// <summary>
    /// レール移動の一時停止
    /// </summary>
    public void PauseMove()
    {
        //一時停止
        tween.Pause();
    }


    /// <summary>
    /// レール移動の再開
    /// </summary>
    public void ResumeMove()
    {
        //移動再開
        tween.Play();
    }



    /// <summary>
    /// パスの目標地点に到達するたびに実行される
    /// </summary>
    /// <param name="waypointIndex"></param>
    private void CheckArrivalDsetination(int waypointIndex)
    {

        Debug.Log("目的地　到着：" + waypointIndex + "番目");

        //移動の一時停止
        PauseMove();

        //移動先のパスがまだ残っているが確認
        if (waypointIndex < currentRailPathData.GetPathTrans().Length)
        {
            // TODO ミッションが発生するかゲームマネージャー側で確認

            //ミッションが発生するかゲームマネジャー側で確認
            gameManager.CheckMissionTrigger(waypointIndex++);

            //Debug用　次のパスへの移動開始
            //ResumeMove();
        }
        else
        {
            //DOTweenを停止
            tween.Kill();

            // TODO 移動先が残っていない場合には、ゲームマネージャー側で分岐の確認(次のルートの選定、移動先の分岐、ボス、クリアのいずれか)
            Debug.Log("分岐確認");
        }
    }
}
