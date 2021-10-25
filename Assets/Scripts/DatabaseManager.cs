using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各種データベース用のスクリプタブルオブジェクト
/// </summary>
public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [SerializeField]
    private StagePathDataSO stagePathDataSO;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    /// <summary>
    /// ステージパス番号から分岐先のRailPathData情報を取得
    /// </summary>
    /// <param name="nextStagePathDataNo"></param>
    /// <param name="serchBranchDirectionType"></param>
    /// <returns></returns>
    public RailPathData GetRailPathDatasFromBranchNo(int nextStagePathDataNo,BranchDirectionType serchBranchDirectionType)
    {
        return stagePathDataSO.stagePathDatasList[nextStagePathDataNo].branchDatasList.Find(x => x.branchDirectionType == serchBranchDirectionType).railPathData;
    }


    /// <summary>
    /// ステージあいのルートの数の取得
    /// </summary>
    /// <returns></returns>
    public int GetStagePathDatasListCount()
    {
        return stagePathDataSO.stagePathDatasList.Count;
    }


    /// <summary>
    /// ブランチの管理している分岐数の取得
    /// </summary>
    /// <param name="branchNo"></param>
    /// <returns></returns>
    public int GetBranchDatasListCount(int branchNo)
    {
        return stagePathDataSO.stagePathDatasList[branchNo].branchDatasList.Count;
    }
}
