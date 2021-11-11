using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エネミーの基幹クラス
/// </summary>
public class EnemyBase : EventBase<int>
{

    [SerializeField]
    protected GameObject lookTarget;

    [SerializeField]
    protected int enemyNo;

    [SerializeField]
    protected int hp;

    [SerializeField]
    protected int attackPower;

    [SerializeField]
    protected float moveSpeed;

    protected Animator anim;

    protected bool isAttack;

    protected float attackInterval = 3.0f;

    protected PlayerController player;

    protected GameManager gameManager;

    protected IEnumerator attackCoroutine;

    protected int point = 100;

    protected bool isDead;

    public EnemyMoveType enemyMoveType;

    // TODO 部位のListを持たせる
    [SerializeField, Header("部位の情報を登録するリスト")]
    protected List<BodyRegionPartsController> partsControllersList = new List<BodyRegionPartsController>();

    // TODO 敵のデータクラスを持たせる

    // Start is called before the first frame update
    void Start()
    {
        //デバッグ用
        SetUpEnemy(lookTarget);
    }


    /// <summary>
    /// エネミーの設定。外部クラスから呼び出す設計
    /// </summary>
    /// <param name="playerObj"></param>
    /// <param name="gameManager"></param>
    public virtual void SetUpEnemy(GameObject playerObj,GameManager gameManager = null)
    {
        lookTarget = playerObj;
        this.gameManager = gameManager;

        //敵のデータを敵の番号から検索してセット
        GetEnemyData();

        TryGetComponent(out anim);
    }


    /// <summary>
    /// 敵の情報をデータベースより取得して設定
    /// </summary>
    protected virtual void GetEnemyData()
    {
        // TODO データベースからデータを取得してセット
    }



    // Update is called once per frame
    protected virtual void Update()
    {
        //エネミーを対象（カメラ）の方向をむける
        if (lookTarget)
        {
            Vector3 direction = lookTarget.transform.position - transform.position;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (isAttack)
        {
            return;
        }

        //プレイヤーの情報を保持しており、攻撃中でないなら
        if (player != null)
        {
            //攻撃用のメソッドを登録
            SetAttackCoroutine();

            Debug.Log("プレイヤー　感知済");

            //プレイヤーの情報がないなら
        }
        else
        {
            if(other.transform.parent.TryGetComponent(out player))
            {
                //攻撃用のメソッドを登録
                SetAttackCoroutine();

                Debug.Log("攻撃範囲内にプレイヤー　初感知");
            }
        }


        //ローカル関数を定義
        void SetAttackCoroutine()
        {
            //IEnumerator型の変数に攻撃用のメソッドを登録
            attackCoroutine = Attack(player);

            //登録したメソッドを実行
            StartCoroutine(attackCoroutine);

            Debug.Log("攻撃開始");
        }
    }


    protected virtual void OnTriggerExit(Collider other)
    {
        //プレイヤーを感知済みの時に、攻撃範囲内にプレイヤーがいなくなったら
        if (player != null)
        {
            //初期化
            player = null;

            //攻撃処理を止める
            isAttack = false;
            StopCoroutine(attackCoroutine);

            Debug.Log("攻撃範囲外");
        }
    }


    /// <summary>
    /// 攻撃
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    protected IEnumerator Attack(PlayerController player = null)
    {
        isAttack = true;

        player.CalcHp(-attackPower);

        if (anim)
        {
            anim.SetTrigger("Attack");
        }

        yield return new WaitForSeconds(attackInterval);

        isAttack = false;
    }


    /// <summary>
    /// 攻撃力取得用
    /// </summary>
    /// <returns></returns>
    public int GetAttackPower()
    {
        return attackPower;
    }


    /// <summary>
    /// 抽象クラスのメソッドを実装
    /// </summary>
    /// <param name="value"></param>
    /// <param name="hitBodyRegionType"></param>
    public override void TriggerEvent(int value, BodyRegionType hitBodyRegionType)
    {
        //ダメージ計算
        CalcDamage(value, hitBodyRegionType);
    }


    /// <summary>
    /// ダメージ計算
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="hitParts"></param>
    public virtual void CalcDamage(int damage,BodyRegionType hitParts)
    {
        if (isDead)
        {
            return;
        }

        hp -= damage;

        if (anim) anim.ResetTrigger("Attack");

        if (hp <= 0)
        {
            isDead = true;

            if (anim)
            {
                anim.SetBool("Walk", false);
                anim.SetBool("Down", true);
            }

            // TODO エネミーの情報を外部クラスのListで管理している場合には、Listから削除

            // TODO 部位による判定があり、かつ、頭を打って倒した場合
            if (hitParts == BodyRegionType.Head)
            {
                //頭を消す
                BodyRegionPartsController parts = partsControllersList.Find(x => x.GetBodyPartType() == hitParts);
                parts.gameObject.SetActive(false);

                //スコアにボーナス(任意)
                point *= 3;
            }

            // TODO スコア加算

            Destroy(gameObject, 1.5f);
        }
        else
        {
            if (anim) anim.SetTrigger("Damage");
        }
    }


    /// <summary>
    /// 移動を一時停止
    /// </summary>
    public virtual void PauseMove()
    {

    }


    /// <summary>
    /// 移動を再開
    /// </summary>
    public virtual void ResumeMove()
    {

    }
}
