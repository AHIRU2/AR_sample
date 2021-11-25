using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class AvatarController : MonoBehaviourPunCallbacks, IPunObservable
{
    private const float MaxStamina = 6f;

    [SerializeField]
    private Image staminaBar = default;

    private float currentStamina = MaxStamina;

    private void Start()
    {
        // 自身が管理者かどうかを判定する
        if (photonView.IsMine)
        {
            // 所有者を取得する
            Player owner = photonView.Owner;
            // 所有者のプレイヤー名とIDをコンソールに出力する
            Debug.Log($"{owner.NickName}({photonView.OwnerActorNr})");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine)
        {
            var input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            if (input.sqrMagnitude > 0f)

                // 入力があったら、スタミナを減少させる
                currentStamina = Mathf.Max(0f, currentStamina - Time.deltaTime);
            transform.Translate(6f * Time.deltaTime * input.normalized);
        }
        else
        {
            // 入力がなかったら、スタミナを回復させる
            currentStamina = Mathf.Min(currentStamina + Time.deltaTime * 2, MaxStamina);
        }

        //スタミナをゲージに反映させる
        staminaBar.fillAmount = currentStamina / MaxStamina;

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 自身のアバターのスタミナを送信する
            stream.SendNext(currentStamina);
        }
        else
        {
            // 他プレイヤーのアバターのスタミナを受信する
            currentStamina = (float)stream.ReceiveNext();
        }

    }
}
