using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// イベント共通用の抽象クラス
/// </summary>
public abstract class EventBase : MonoBehaviour
{
    public EventType eventType;

    /// <summary>
    /// イベント発火用
    /// </summary>
    /// <param name="value"></param>
    /// <param name="hitBodyRegionType"></param>
    public abstract void TriggerEvent(int value, BodyRegionType hitBodyRegionType);
}
