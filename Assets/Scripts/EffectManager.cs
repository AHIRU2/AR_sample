using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;

    //Player用
    [Header("発射口用のエフェクト")]
    public GameObject hitEffectPrefab;

    // TODO 適宜追加

    //適用

    //共通

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

    //enumにエフェクトの種類を登録しておいて、それを引数とし、対象となるエフェクトの情報を取得するゲッターメソッドを準備
}
