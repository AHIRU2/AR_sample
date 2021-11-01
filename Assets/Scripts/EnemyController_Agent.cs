using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class EnemyController_Agent : EnemyBase
{
    protected NavMeshAgent agent;  //NavMeshAgentコンポーネントの代入用、子クラスには変数の宣言もできる

    private float originMoveSpeed; //初期の移動速度の保持用、この情報はprivate修飾子なのでこのクラス内でのみ利用できる



    /// <summary>
    /// エネミーの設定
    /// </summary>
    /// <param name="playerObj"></param>
    /// <param name="gameManager"></param>
    public override void SetUpEnemy(GameObject playerObj, GameManager gameManager = null)
    {
        base.SetUpEnemy(playerObj, gameManager);

        //NavMeshを利用しているか判定
        if(TryGetComponent(out agent))
        {
            originMoveSpeed = moveSpeed;

            //利用している場合には目標地点をセット
            agent.destination = lookTarget.transform.position;

            //移動速度をNavMeshに設定
            agent.speed = moveSpeed;

            //アニメがある場合には再生
            if (anim)
            {
                anim.SetBool("Walk", true);
            }
        }
    }


    protected override void Update()
    {
        base.Update();

        //目的地を更新
        if (lookTarget != null && agent != null)
        {
            agent.destination = lookTarget.transform.position;
        }
    }


    /// <summary>
    /// 移動を一時停止
    /// </summary>
    public override void PauseMove()
    {
        agent.speed = 0;
    }


    /// <summary>
    /// 移動を再開
    /// </summary>
    public override void ResumeMove()
    {
        agent.speed = originMoveSpeed;
    }


    /// <summary>
    /// 目的地を消去
    /// </summary>
    public void ClearPath()
    {
        agent.ResetPath();
    }


}
