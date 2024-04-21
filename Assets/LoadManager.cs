
using System;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(NetcodeHooks), typeof(NetworkCharaSelection))]
public class LoadManager : MonoBehaviour
{
    [SerializeField]
    NetcodeHooks m_NetcodeHooks;

    [SerializeField]
    NetworkCharaSelection m_NetworkCharSelection;
    //private List<GameObject> respawnPointsList = new List<GameObject>();
    //[SerializeField]
    //private NetworkObject Player;
    //[SerializeField]
    //private ClientList clientListData;

    ////public NetworkVariable<List> respawnPointsList = new NetworkVariable<List>(new List());
    ////public NetworkVariable<List<Transform>> respawnPointsList = new NetworkVariable<List<Transform>>(
    ////         new List<Transform>(),
    ////         NetworkVariableReadPermission.Everyone,     // 読み込み管理者
    ////         NetworkVariableWritePermission.Server       // 書き換え管理者
    ////    );


    //public Vector3 respawnPoint;

    ////ネットワーク同期変数
    //public NetworkVariable<int> countClient = new NetworkVariable<int>(
    //        0,
    //        NetworkVariableReadPermission.Everyone,     // 読み込み管理者
    //        NetworkVariableWritePermission.Server       // 書き換え管理者
    //    );

    //public NetworkVariable<ulong> addClientId = new NetworkVariable<ulong>(
    //    0,
    //    NetworkVariableReadPermission.Everyone,     // 読み込み管理者
    //    NetworkVariableWritePermission.Server       // 書き換え管理者
    //);

    //public ulong hoge;
    public bool isHost = false;
    public NetworkCharaSelection networkCharSelection { get; private set; }
    protected void Awake()
    {
        networkCharSelection = GetComponent<NetworkCharaSelection>();
        m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
    }

    public void OnNetworkSpawn()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            enabled = false;
        }
        else
        {
            //NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
            networkCharSelection.OnClientChangedSeat += OnClientChangedSeat;
            networkCharSelection.OnClientChangedReadyState += OnClientChangedReadyState;
            networkCharSelection.OnStartGame += OnStartGame;
        }
    }

    void OnSceneEvent(SceneEvent sceneEvent)
    {
        // We need to filter out the event that are not a client has finished loading the scene
        if (sceneEvent.SceneEventType != SceneEventType.LoadComplete) return;
        // When the client finishes loading the Lobby Map, we will need to Seat it


        Debug.Log("=== ロビーシーンが読み込まれました ===");
        Debug.Log("クライアントID " + sceneEvent.ClientId);
        //Debug.Log("ロビープレイヤーカウント " + networkCharSelection.LobbyPlayers.Count);

        if (networkCharSelection.LobbyPlayers.Count == 0)
        {
            isHost = true;
        }
        else
        {
            isHost = false;
        }

        networkCharSelection.LobbyPlayers.Add(new NetworkCharaSelection.LobbyPlayerState(sceneEvent.ClientId,0, networkCharSelection.LobbyPlayers.Count,isHost));

    }

    void OnClientChangedSeat(ulong clientId, int newSeatIdx)
    {
        int idx = FindLobbyPlayerIdx(clientId);

        if (idx == -1)
        {
            // プレイヤーが存在しない場合
            throw new Exception($"OnClientChangedSeat: client ID {clientId} is not a lobby player and cannot change seats! Shouldn't be here!");
        }

        if(newSeatIdx != -1)
        {
            for(int i=0;i < networkCharSelection.LobbyPlayers.Count; i++)
            {
                if (networkCharSelection.LobbyPlayers[i].ClientId != clientId)
                {
                    networkCharSelection.LobbyPlayers[i] = new NetworkCharaSelection.LobbyPlayerState(
                        networkCharSelection.LobbyPlayers[i].ClientId,
                        networkCharSelection.LobbyPlayers[i].PlayerNumber,
                        networkCharSelection.LobbyPlayers[i].RespawnNumber,
                        networkCharSelection.LobbyPlayers[i].IsHost,
                        0,
                        networkCharSelection.LobbyPlayers[i].SeatIdx,
                        networkCharSelection.LobbyPlayers[i].ReadyState
                        );
                }
            }

        }

        networkCharSelection.LobbyPlayers[idx] = new NetworkCharaSelection.LobbyPlayerState(
            networkCharSelection.LobbyPlayers[idx].ClientId,
            networkCharSelection.LobbyPlayers[idx].PlayerNumber,
            networkCharSelection.LobbyPlayers[idx].RespawnNumber,
            networkCharSelection.LobbyPlayers[idx].IsHost,
            0,
            newSeatIdx,
            networkCharSelection.LobbyPlayers[idx].ReadyState
            );
    }

    void OnClientChangedReadyState(ulong clientId, bool isReady)
    {

        int idx = FindLobbyPlayerIdx(clientId);

        if (idx == -1)
        {
            // プレイヤーが存在しない場合
            throw new Exception($"OnClientChangedSeat: client ID {clientId} is not a lobby player and cannot change seats! Shouldn't be here!");
        }

        if (!isReady)
        {
            for (int i = 0; i < networkCharSelection.LobbyPlayers.Count; i++)
            {
                if (networkCharSelection.LobbyPlayers[i].ClientId != clientId)
                {
                    networkCharSelection.LobbyPlayers[i] = new NetworkCharaSelection.LobbyPlayerState(
                        networkCharSelection.LobbyPlayers[i].ClientId,
                        networkCharSelection.LobbyPlayers[i].PlayerNumber,
                        networkCharSelection.LobbyPlayers[i].RespawnNumber,
                        networkCharSelection.LobbyPlayers[i].IsHost,
                        0,
                        networkCharSelection.LobbyPlayers[i].SeatIdx,
                        networkCharSelection.LobbyPlayers[i].ReadyState
                        );
                }
            }

        }

        networkCharSelection.LobbyPlayers[idx] = new NetworkCharaSelection.LobbyPlayerState(
            networkCharSelection.LobbyPlayers[idx].ClientId,
            networkCharSelection.LobbyPlayers[idx].PlayerNumber,
            networkCharSelection.LobbyPlayers[idx].RespawnNumber,
            networkCharSelection.LobbyPlayers[idx].IsHost,
            0,
            networkCharSelection.LobbyPlayers[idx].SeatIdx,
            isReady
            );
    }


    /// <summary>
    /// Returns the index of a client in the master LobbyPlayer list, or -1 if not found
    /// </summary>
    int FindLobbyPlayerIdx(ulong clientId)
    {
        for (int i = 0; i < networkCharSelection.LobbyPlayers.Count; ++i)
        {
            if (networkCharSelection.LobbyPlayers[i].ClientId == clientId)
                return i;
        }
        return -1;
    }

    void OnStartGame()
    {
        foreach (NetworkCharaSelection.LobbyPlayerState playerInfo in m_NetworkCharSelection.LobbyPlayers)
        {
            var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(playerInfo.ClientId);

            if (playerNetworkObject && playerNetworkObject.TryGetComponent(out Player persistentPlayer))
            {

                persistentPlayer.networkAvatarGuidState.avatarInt.Value = playerInfo.SeatIdx;

                //m_NetworkCharSelection.ClientAvatarIntServerRpc(playerInfo.SeatIdx);

                //persistentPlayer.playerAvaterObj = m_NetworkCharSelection.AvatarConfiguration[playerInfo.SeatIdx];

                // pass avatar GUID to PersistentPlayer
                // it'd be great to simplify this with something like a NetworkScriptableObjects :(
                //persistentPlayer.name = "nameChange" + playerInfo.ClientId;
                //persistentPlayer.NetworkAvatarGuidState.AvatarGuid.Value = m_NetworkCharSelection.AvatarConfiguration[playerInfo.SeatIdx].GetComponent<SpriteRenderer>();
            }
        }
        NetworkManager.Singleton.SceneManager.OnSceneEvent -= OnSceneEvent;
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    //public override void OnNetworkSpawn()
    //{
    //    //if (IsServer)
    //    //{
    //    //    Debug.Print("動いています");
    //    //}
    //}

    ///// <summary>
    ///// プレイヤーがスポーンしたら実行される。
    ///// </summary>
    //public override void OnNetworkSpawn()
    //{
    //    ////if (!IsHost)
    //    ////{
    //    ////    return;
    //    ////}
    //    //Debug.Log("==== Secene Load Compalete ====");
    //    //ulong clientId = NetworkManager.Singleton.LocalClientId;

    //    //Debug.Log("clientId  " + clientId);
    //    //Debug.Log("AddClientId " + addClientId.Value);

    //    //GetClientCountRpc(clientId, hoge);
    //    //Debug.Log("HOGE " + hoge);
    //    ////// ネットワーク同期変数の値が変わったら呼ばれる処理
    //    //countClient.OnValueChanged += (int oldCountClient, int newCountClient) =>
    //    //{
    //    //    if (!IsHost)
    //    //    {
    //    //        return;
    //    //    }
    //    //    Debug.Log("値が変わった " + newCountClient);
    //    //    Debug.Log("CLIENT ID " + clientId);
    //    //    Debug.Log("AddClientId " + addClientId.Value);

    //    //    //if (clientId != OwnerClientId)
    //    //    //{
    //    //    //    Debug.Log("自分のじゃやないのでリターン");
    //    //    //    return;
    //    //    //}
    //    //    //Debug.Log("newCountClient " + newCountClient);
    //    //    ////リスポーンリストが0の場合はreturn
    //    //    //if (respawnPointsList.Count == 0)
    //    //    //{
    //    //    //    Debug.Log("リスポーン位置がなくなった。");
    //    //    //    return;
    //    //    //}
    //    //    ////スポーン地点を取得
    //    //    ////newCountClientだと[1]からなので、oldCountClientを使用する。
    //    //    //Vector3 respawnPoint = respawnPointsList[oldCountClient].transform.position;
    //    //    ////PlayerAvatarを生成
    //    //    //PlayerAvaterSpwanRpc(clientId, respawnPoint);
    //    //};

    //    ////base.OnNetworkSpawn();
    //    ////int ownerClientId = NetworkManager.Singleton.GetComponent<NetworkIdentity>().netId;
    //    //// クライアントIDを取得
    //    //ulong clientId = NetworkManager.Singleton.LocalClientId;
    //    ////ulong clientId = OwnerClientId;
    //    //Debug.Log("clientId !! " + clientId);
    //    ////Debug.Log("OwnerClientId = " + clientId);


    //    ////int clientCount = clientListData.DataList.Count;
    //    ////Debug.Log(clientCount);
    //    ////Debug.Log("clientCount " + clientCount);
    //    ////// リスポーンリストが0の場合はreturn
    //    ////if (respawnPointsList.Count == 0)
    //    ////{
    //    ////    Debug.Log("リスポーン位置がなくなった。");
    //    ////    return;
    //    ////}
    //    ////// スポーン地点を取得
    //    ////// newCountClientだと[1]からなので、oldCountClientを使用する。
    //    ////Vector3 respawnPoint = respawnPointsList[clientCount].transform.position;
    //    ////// PlayerAvatarを生成
    //    ////PlayerAvaterSpwanRpc(clientId, respawnPoint);

    //    ////if (IsHost)
    //    ////{
    //    ////    Vector3 respawnPoint = respawnPointsList[countClient.Value].transform.position;
    //    ////    //PlayerAvatarを生成
    //    ////    PlayerAvaterSpwanRpc(clientId, respawnPoint);
    //    ////}

    //    //// ネットワーク同期変数の値が変わったら呼ばれる処理
    //    //countClient.OnValueChanged += (int oldCountClient, int newCountClient) =>
    //    //{
    //    //    if (clientId != OwnerClientId)
    //    //    {
    //    //        Debug.Log("自分のじゃやないのでリターン");
    //    //        return;
    //    //    }
    //    //    Debug.Log("newCountClient " + newCountClient);
    //    //    //リスポーンリストが0の場合はreturn
    //    //    if (respawnPointsList.Count == 0)
    //    //    {
    //    //        Debug.Log("リスポーン位置がなくなった。");
    //    //        return;
    //    //    }
    //    //    //スポーン地点を取得
    //    //    //newCountClientだと[1]からなので、oldCountClientを使用する。
    //    //    Vector3 respawnPoint = respawnPointsList[oldCountClient].transform.position;
    //    //    //PlayerAvatarを生成
    //    //    PlayerAvaterSpwanRpc(clientId, respawnPoint);
    //    //};

    //    //GetClientCountRpc();

    //    //RespanListRpc(respawnPointsList);

    //    // 現在接続している人数を取得
    //    //GetClientCountRpc();



    //    //Debug.Log("ClientCount " + clientCount);


    //    //PlayerAvaterSpwanRpc(clientId);

    //}


    //void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    //{

    //}


    // クライアントIDに対応するプレイヤーネットワークオブジェクトを取得
    //var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);


    //[Rpc(SendTo.Server)]
    //void RespanListRpc(List<Transform> respawnPointsList)
    //{
    //    // リスポーン地点がなかった場合
    //    if (respawnPointsList.Count == 0)
    //    {
    //        Debug.Log("リストがなくなりました。");
    //        return;
    //    }

    //    // スポーン地点を取得
    //    Transform respawnPoint = respawnPointsList[0];
    //    //spwanPoint = respawnPoint.position;
    //    // 取得したスポーン地点を削除
    //    respawnPointsList.RemoveAt(0);
    //    return;
    //}





    ///// <summary>
    ///// 現在接続している人数を取得
    ///// </summary>
    //[Rpc(SendTo.Server,RequireOwnership = false)]
    //void GetClientCountRpc(ulong clientId,ulong hoge)
    //{
    //    countClient.Value = NetworkManager.Singleton.ConnectedClientsList.Count;
    //    addClientId.Value = clientId;
    //    hoge = clientId;
    //}

    //[Rpc(SendTo.Server)]
    //void ClientDataListClearRpc()
    //{
    //    Debug.Log("clearしてる？？");
    //    clientListData.DataList.Clear();
    //}


    ///// <summary>
    ///// PlayerAvatarを生成
    ///// </summary>
    ///// <param name="clientId"></param>
    //[Rpc(SendTo.Server)]
    //void PlayerAvaterSpwanRpc(ulong clientId, Vector3 respawnPoint)
    //{
    //    Debug.Log(respawnPoint);
    //    //// サーバー上でオブジェクトをインスタンスしスポーンさせる。
    //    NetworkManager.SpawnManager.InstantiateAndSpawn(Player, clientId, false, true, true, respawnPoint);

    //    //// Tagを持つオブジェクトを取得
    //    //GameObject[] characterObjArray = GameObject.FindGameObjectsWithTag("Character");

    //    //foreach (GameObject obj in characterObjArray)
    //    //{
    //    //    Debug.Log("ループしてる");
    //    //    NetworkObject identity = obj.GetComponent<NetworkObject>();
    //    //    ulong obj_id = identity.OwnerClientId;

    //    //    if (obj_id == clientId)
    //    //    {
    //    //        // 新しい ClientData オブジェクトを作成
    //    //        ClientData clientData = new ClientData();
    //    //        clientData.NetId = clientId;
    //    //        clientData.ClientObj = obj;
    //    //        clientListData.DataList.Add(clientData);
    //    //        Debug.Log("スクリプトテーブルに追加しました。");
    //    //    }
    //    //}

    //    //List<NetworkObject> hoge = new List<NetworkObject>
    //    //{
    //    //    NetworkManager.SpawnManager.GetPlayerNetworkObject(clientId)
    //    //};

    //    //// 取得したオブジェクトの名前を出力
    //    //foreach (NetworkObject obj in hoge)
    //    //{
    //    //    Debug.Log(obj.name);
    //    //}

    //    //// tag が付いているオブジェクトを取得
    //    //NetworkObject characterObject = hoge.Find(obj => obj.gameObject.tag == "Character");

    //    //if (characterObject != null)
    //    //{
    //    //    // 取得したオブジェクトを使用
    //    //    Debug.Log(characterObject.name);
    //    //}
    //    //else
    //    //{
    //    //    // オブジェクトが見つからなかった
    //    //    Debug.Log("Character object not found");
    //    //}

    //    //// Tagを持つオブジェクトを取得
    //    //GameObject[] characterObj = GameObject.FindGameObjectsWithTag("Character");

    //    //// 新しい ClientData オブジェクトを作成
    //    //ClientData clientData = new ClientData();
    //    //clientData.NetId = clientId;
    //    //clientData.ClientObj = characterObj;

    //    //// リストに追加
    //    //DataList.Add(clientData);
    //}



    //public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    //    NetworkManager.OnServerAddPlayer(conn, player, playerControllerId);
    //}

    //public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //}
    //public override void OnServerAddPlayer()
    //{
    //    // プレイヤーのリスポーンを無効にする
    //    base.OnPlayerAdded(conn, player, false);
    //}

    //NetworkObject.SpawnManager

}