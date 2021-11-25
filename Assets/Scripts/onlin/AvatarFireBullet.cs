using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarFireBullet : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Bullet bulletPrefab = default;

    private int nextBulletId = 0;

    private void Update()
    {
        if (photonView.IsMine)
        {
            // 左クリックでカーソルの方向に弾を発射する
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var direction = mousePosition - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x);

                // 弾を発射するたびに弾のIDを1ずつ増やしていく
                photonView.RPC(nameof(FireBullet), RpcTarget.All, nextBulletId++, angle);
            }
        }
    }

    // 弾を発射するメソッド
    [PunRPC]
    private void FireBullet(int id, float angle)
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.Init(id, photonView.OwnerActorNr, transform.position, angle);
    }
}
