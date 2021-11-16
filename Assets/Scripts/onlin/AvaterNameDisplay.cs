using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class AvaterNameDisplay : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        var nameLabel = GetComponent<TextMeshPro>();

        //プレイヤー名とプレイヤーIDを表示する
        nameLabel.text = $"{photonView.Owner.NickName}({photonView.OwnerActorNr})";
    }

}
