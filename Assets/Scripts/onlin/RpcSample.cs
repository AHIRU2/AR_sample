using Photon.Pun;
using UnityEngine;

public class RpcSample : MonoBehaviourPunCallbacks
{
    private void Update()
    {
        // マウスクリック毎に、ルーム内のプレイヤー全員にメッセージを送信する
        if (Input.GetMouseButtonDown(0))
        {
            photonView.RPC(nameof(RpcSendMessage), RpcTarget.All, "こんにちは");
        }
    }

    [PunRPC]
    private void RpcSendMessage(string message, PhotonMessageInfo info)
    {
        // メッセージを送信したプレイヤー名も表示する
        Debug.Log($"{info.Sender.NickName}: {message}");
    }
}