using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField, Tooltip("AR空間に召喚する豆腐")] GameObject tohu;

    [SerializeField]
    private GameObject stageObj;

    //[SerializeField]
    //private CameraController cameraController;

    private GameObject obj;

    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> raycastHitList = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            TrackingPlane();

            //Vector2 touchPosition = Input.GetTouch(0).position;
            //if (raycastManager.Raycast(touchPosition, raycastHitList, TrackableType.Planes))
            //{
            //    // Raycastの衝突情報は距離によってソートされるため、0番目が最も近い場所でヒットした情報となります
            //    var hitPose = raycastHitList[0].pose;

            //    if (spawnedObject)
            //    {
            //        spawnedObject.transform.position = hitPose.position;
            //    }
            //    else
            //    {
            //        spawnedObject = Instantiate(tohu, hitPose.position, Quaternion.identity);
            //    }
            //}
        }
    }



    /// <summary>
    /// 平面感知とfeild表示
    /// </summary>
    private void TrackingPlane()
    {
        Touch touch = Input.GetTouch(0);

        //タッチを開始していなければ
        if (touch.phase != TouchPhase.Ended)
        {
            //処理を行わない
            return;
        }

        //ARCameraからタップした地点に向かってRayを発射し、平面感知している部分にRayが侵入したか判定
        if (raycastManager.Raycast(touch.position, raycastHitList, TrackableType.PlaneWithinPolygon))
        {
            //Rayが平面感知した最新の地点を取得
            Pose hitPose = raycastHitList[0].pose;

            if (obj = null)
            {
                LogDebugger.instance.Displaylog("Raycast 成功");

                // TODO 後で生成する処理に変更
                obj = Instantiate(tohu, hitPose.position, Quaternion.identity);

                // TODO フィールドを表示（後で生成する処理に変更）
                //stageObj.SetActive(true);

                // TODO 平面感知を終了

            }
            else
            {
                LogDebugger.instance.Displaylog("Raycast 済");

                obj.transform.position = hitPose.position;
            }

        }
        else
        {
            LogDebugger.instance.Displaylog("RayCast 失敗");
        }
    }


}