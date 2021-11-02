using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rayによる弾の発射処理の制御クラス
/// </summary>
public class RayController : MonoBehaviour
{
    [Header("発射口用のエフェクトサイズ調整")]
    public Vector3 muzzleFlashScale;

    public GameObject hitEffectObj;

    private bool isShooting;

    private GameObject muzzleFlasjObj;  //生成したエフェクトの代入用

    private GameObject target; //Rayで補足した対象の代入

    [SerializeField, Header("Ray用のレイヤー設定")]
    private int[] layerMasks;

    [SerializeField] //Debug用、確認できたらSerializeField属性を削除してprivateにしておく
    private string[] layerMasksStr;

    [SerializeField]
    private PlayerController playerController;

    private EventBase eventBase;


    // Start is called before the first frame update
    void Start()
    {
        //Layerの情報を文字列に変換し、Raycastメソッドで利用しやすい情報を変数として作成しておく
        layerMasksStr = new string[layerMasks.Length];
        for(int i = 0; i < layerMasks.Length; i++)
        {
            layerMasksStr[i] = LayerMask.LayerToName(layerMasks[i]);
        }


    }

    // Update is called once per frame
    void Update()
    {
        // TODO ゲーム状態がプレイ中でない場合には処理は行わない制御をする

        //リロード判定(弾数0でリロード機能ありの場合)
        if (playerController.BulletCount == 0 && playerController.isReloadModeOn && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(playerController.ReloadBullet());
        }

        //発射判定(弾数が残っており、リロード実行中でない場合)押しっぱなしで発射できる


        if (playerController.BulletCount > 0 && !playerController.isReloading && Input.GetMouseButton(0)){
            //発射時間の計測
            StartCoroutine(ShootTimer());
        }
    }


    /// <summary>
    /// 継続的な弾の発射処理
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootTimer()
    {
        if (!isShooting)
        {
            isShooting = true;

            //発射エフェクトの表示、初回のみ生成し、2回目はオンオフで切り替える
            if (muzzleFlasjObj == null)
            {

                //発射口の位置にRayControllerゲームオブジェクトを配置する
                muzzleFlasjObj = Instantiate(EffectManager.instance.muzzleFlashPrefab, transform.position, transform.rotation);
                muzzleFlasjObj.transform.SetParent(gameObject.transform);
                muzzleFlasjObj.transform.localScale = muzzleFlashScale;

            }
            else
            {
                muzzleFlasjObj.SetActive(true);
            }

            //発射
            Shoot();

            yield return new WaitForSeconds(playerController.shootInterval);

            muzzleFlasjObj.SetActive(false);

            //if (hitEffectObj != null)
            //{
            //    hitEffectObj.SetActive(false);
            //}
            //else{
            //    yield return null;
            //}
        }

    }



    /// <summary>
    /// 弾の発射
    /// </summary>
    private void Shoot()
    {
        //カメラの位置からRayを投射
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, playerController.shootRange, LayerMask.GetMask(layerMasksStr)))
        {
            Debug.Log(hit.collider.gameObject.name);

            //同じ対象を攻撃しているか確認、対象外なかったか、同じ対象でない場合
            if (target == null || target != hit.collider.gameObject)
            {
                target = hit.collider.gameObject;

                Debug.Log(target.name);

                // TODO TryGetComponentの処理で敵や障害物などの情報を取得しつつ、判定をする
                //ゲームオブジェクトにアタッチされている親クラスを取得できるか判定
                if(target.TryGetComponent(out eventBase))
                {
                    //取得した親クラスにある抽象メソッドを実行する＝＞子クラスで実装しているメソッドの振る舞いになる
                    eventBase.TriggerEvent(playerController.bulletPower, BodyRegionType.Not_Available);

                    //演出
                    PlayHitEffect(hit.point, hit.normal);
                }

                //同じ対象の場合
            }
            else if(target==hit.collider.gameObject)
            {
                // TODO すでに情報があるので再取得はせずに判定のみする
                eventBase.TriggerEvent(playerController.bulletPower,BodyRegionType.Not_Available);

                // TODO 演出
                PlayHitEffect(hit.point, hit.normal);
            }
        }

        //弾数を減らす
        playerController.CalcBulletCount(-1);

    }


    private void PlayHitEffect(Vector3 effectPos,Vector3 surfacePos)
    {
        if (hitEffectObj == null)
        {
            hitEffectObj = Instantiate(EffectManager.instance.hitEffectPrefab, effectPos, Quaternion.identity);
        }
        else
        {
            hitEffectObj.transform.position = effectPos;
            hitEffectObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, surfacePos);

            hitEffectObj.SetActive(true);
        }
    }


}
