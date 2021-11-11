using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 指定したクラスをstring型のJson形式でPlayerPrefsクラスにセーブ・ロードするためのヘルパークラス
/// </summary>
public class PlayerPrefsHelper : MonoBehaviour
{
    /// <summary>
    /// 指定したキーのデータが存在しているか確認
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool ExistsData(string key)
    {
        //指定したキーのデータが存在しているか確認して、存在している場合はtrue、存在していない場合はfalsを戻す
        return PlayerPrefs.HasKey(key);
    }


    /// <summary>
    /// 指定されたオブジェクトのデータをセーブ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="obj"></param>
    public static void SaveSetObjectData<T>(string key, T obj)
    {
        //オブジェクトのデータをJson形式に変換
        string json = JsonUtility.ToJson(obj);

        //セット
        PlayerPrefs.SetString(key, json);

        //セットしたKeyとjsonをセーブ
        PlayerPrefs.Save();

    }


    /// <summary>
    /// 指定されたオブジェクトのデータをロード
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public static T LoadGetObjectDate<T>(string key)
    {
        //セーブされているデータをロード
        string json = PlayerPrefs.GetString(key);

        //読み込む型を指定して変換して取得
        return JsonUtility.FromJson<T>(json);
    }


    /// <summary>
    /// 指定されたキーのデータを削除
    /// </summary>
    /// <param name="key"></param>
    public static void RemoveObjectData(string key)
    {
        //指定されたキーのデータを削除
        PlayerPrefs.DeleteKey(key);

        Debug.Log("セーブデータを削除 実行 :" + key);

    }


    /// <summary>
    /// 全てのセーブデータを削除
    /// </summary>
    public static void AllClearSaveData()
    {
        //全てのセーブデータを削除
        PlayerPrefs.DeleteAll();

        Debug.Log("全セーブデータを削除 実行");
    }


    /// <summary>
    /// 整数データのセーブ
    /// </summary>
    /// <param name="key"></param>
    /// <param name="saveValue"></param>
    public static void SaveIntData(string key,int saveValue)
    {
        //整数データのセットとセーブ
        PlayerPrefs.SetInt(key, saveValue);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// 整数データのロード
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static int LoadInData(string key)
    {
        //整数データのロード
        return PlayerPrefs.GetInt(key);
    }


    /// <summary>
    /// 文字列データのセーブ(主にDateTime 構造体のセーブに使う)
    /// </summary>
    /// <param name="key"></param>
    /// <param name="saveValue"></param>
    public static void SaveStringData(string key,string saveValue)
    {
        PlayerPrefs.SetString(key, saveValue);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// 文字列データのロード(主に DateTime 構造体のロードに使う)
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string LoadStringData(string key)
    {
        return PlayerPrefs.GetString(key);
    }


}
