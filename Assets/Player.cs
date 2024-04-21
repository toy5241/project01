using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;
public class Player : NetworkBehaviour
{
    [SerializeField]
    public GameObject playerAvaterObj;

    [SerializeField]
    public NetworkAvatarGuidState networkAvatarGuidState;

    //[SerializeField]
    //NetworkAvatarGuidState m_NetworkAvatarGuidState;

    //public NetworkAvatarGuidState NetworkAvatarGuidState => m_NetworkAvatarGuidState;

    //[SerializeField]
    //private List<GameObject> respawnPointsList = new List<GameObject>();
    //[SerializeField]
    //private NetworkObject CharacterAvatar;

    //public Vector3 respawnPoint;

    //////ネットワーク同期変数
    //public NetworkVariable<int> countClient = new NetworkVariable<int>(
    //        0,
    //        NetworkVariableReadPermission.Everyone,     // 読み込み管理者
    //        NetworkVariableWritePermission.Server       // 書き換え管理者
    //    );

    public override void OnNetworkSpawn()
    {
        gameObject.name = "PersistentPlayer" + OwnerClientId;

        //if (respawnPointsList.Count == 0)
        //{
        //    Debug.Log("リスポーン位置がなくなった。");
        //    return;
        //}

        ////// ネットワーク同期変数の値が変わったら呼ばれる処理
        //countClient.OnValueChanged += (int oldCountClient, int newCountClient) =>
        //{
        //    Debug.Log("oldCountClient " + oldCountClient);
        //    Debug.Log("newCountClient " + newCountClient);

        //    //スポーン地点を取得
        //    //newCountClientだと[1]からなので、oldCountClientを使用する。
        //    Vector3 respawnPoint = respawnPointsList[oldCountClient].transform.position;
        //    //PlayerAvatarを生成
        //    Debug.Log("==== PlayerAvaterSpawnRPC 実行  ====");
        //    if (IsServer)
        //    {
        //        PlayerAvaterSpwanRpc(OwnerClientId, respawnPoint);
        //    }
        //};

        //if (IsServer)
        //{
        //    GetClientCountRpc();
        //}



    }

    ///// <summary>
    ///// 現在接続している人数を取得
    ///// </summary>
    //[Rpc(SendTo.Server, RequireOwnership = false)]
    //void GetClientCountRpc()
    //{
    //    countClient.Value = NetworkManager.Singleton.ConnectedClientsList.Count;
    //}


    ///// <summary>
    ///// PlayerAvatarを生成
    ///// </summary>
    ///// <param name="clientId"></param>
    //[Rpc(SendTo.Server, RequireOwnership = false)]
    //void PlayerAvaterSpwanRpc(ulong clientId, Vector3 respawnPoint)
    //{
    //    ////// サーバー上でオブジェクトをインスタンスしスポーンさせる。
    //    NetworkManager.SpawnManager.InstantiateAndSpawn(CharacterAvatar, clientId, false, true, true, respawnPoint);
    //}

    //private Player m_Player;
    //[SerializeField]
    //PersistentPlayerRuntimeCollection m_PersistentPlayerRuntimeCollection;
    //public override void OnNetworkSpawn()
    //{
    //    gameObject.name = "Player" + OwnerClientId; // ゲームオブジェクトの名前を設定
    //    m_Player = (Player)this;
    //    m_PersistentPlayerRuntimeCollection.Add(m_Player);
    //}
    //private void Update()
    //{
    //    if (IsOwner == false)
    //    {
    //        return;
    //    }


    //    Vector2 direction = new Vector2()
    //    {
    //        x = Input.GetAxisRaw("Horizontal"),
    //        y = Input.GetAxisRaw("Vertical")
    //    };
    //    float moveSpeed = 3f;
    //    transform.Translate(direction * moveSpeed * Time.deltaTime);
    //}

    // オブジェクトがスポーンしたら発生する処理
    //public override void OnNetworkSpawn()
    //{
    //    if (IsClient)
    //    {
    //        Debug.Log("クライアントが接続しました。");
    //    }
    //}
}
