using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;




public class ServerState : NetworkBehaviour
{
    [SerializeField]
    [Tooltip("Make sure this is included in the NetworkManager's list of prefabs!")]
    private NetworkObject m_PlayerPrefab;

    public override void OnNetworkSpawn()
    {
        //NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
        // すべての接続されたクライアントのIDを出力する


        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            Debug.Log("======== サーバーステート動いてます！！  ==============");
            Debug.Log(client.Key);
            SpwanPlayerServerRpc(client.Key);
        }
    }

    void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        // すべての接続されたクライアントのIDを出力する
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            Debug.Log(client.Key);
            SpwanPlayerServerRpc(client.Key);
        }


    }

    [ServerRpc(RequireOwnership = false)]   
    public void SpwanPlayerServerRpc(ulong clientId) {
        // クライアントIDに対応するプレイヤーネットワークオブジェクトを取得
        var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);

        var newPlayer = Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);
        // プレイヤーキャラクターをシーンとともに破棄するようにしてスポーン
        newPlayer.SpawnWithOwnership(clientId, true);
    }
}
