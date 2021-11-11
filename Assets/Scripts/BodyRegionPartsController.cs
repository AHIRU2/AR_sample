using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRegionPartsController : MonoBehaviour
{
    [SerializeField, Header("部位の設定")]
    private BodyRegionType bodyPartType;


    /// <summary>
    /// 部位の情報の取得用。プロパティでも可
    /// </summary>
    /// <returns></returns>
    public BodyRegionType GetBodyPartType()
    {
        return bodyPartType;
    }


    /// <summary>
    /// 部位ごとにダメージの値を計算
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public (int,BodyRegionType) CalcDamageParts(int damage)
    {
        //部位の情報を利用してダメージの計算をする
        int lastDamage = bodyPartType switch
        {
            //頭部の場合はダメージ5倍
            BodyRegionType.Head => damage * 5,

            // TODO 他の部位を追加

            //上記以外
            _ => damage,
        };

        //処理結果をタプル型で戻す
        return (lastDamage, bodyPartType);
    }




}
