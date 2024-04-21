
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
    ////         NetworkVariableReadPermission.Everyone,     // �ǂݍ��݊Ǘ���
    ////         NetworkVariableWritePermission.Server       // ���������Ǘ���
    ////    );


    //public Vector3 respawnPoint;

    ////�l�b�g���[�N�����ϐ�
    //public NetworkVariable<int> countClient = new NetworkVariable<int>(
    //        0,
    //        NetworkVariableReadPermission.Everyone,     // �ǂݍ��݊Ǘ���
    //        NetworkVariableWritePermission.Server       // ���������Ǘ���
    //    );

    //public NetworkVariable<ulong> addClientId = new NetworkVariable<ulong>(
    //    0,
    //    NetworkVariableReadPermission.Everyone,     // �ǂݍ��݊Ǘ���
    //    NetworkVariableWritePermission.Server       // ���������Ǘ���
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


        Debug.Log("=== ���r�[�V�[�����ǂݍ��܂�܂��� ===");
        Debug.Log("�N���C�A���gID " + sceneEvent.ClientId);
        //Debug.Log("���r�[�v���C���[�J�E���g " + networkCharSelection.LobbyPlayers.Count);

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
            // �v���C���[�����݂��Ȃ��ꍇ
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
            // �v���C���[�����݂��Ȃ��ꍇ
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
    //    //    Debug.Print("�����Ă��܂�");
    //    //}
    //}

    ///// <summary>
    ///// �v���C���[���X�|�[����������s�����B
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
    //    ////// �l�b�g���[�N�����ϐ��̒l���ς������Ă΂�鏈��
    //    //countClient.OnValueChanged += (int oldCountClient, int newCountClient) =>
    //    //{
    //    //    if (!IsHost)
    //    //    {
    //    //        return;
    //    //    }
    //    //    Debug.Log("�l���ς���� " + newCountClient);
    //    //    Debug.Log("CLIENT ID " + clientId);
    //    //    Debug.Log("AddClientId " + addClientId.Value);

    //    //    //if (clientId != OwnerClientId)
    //    //    //{
    //    //    //    Debug.Log("�����̂����Ȃ��̂Ń��^�[��");
    //    //    //    return;
    //    //    //}
    //    //    //Debug.Log("newCountClient " + newCountClient);
    //    //    ////���X�|�[�����X�g��0�̏ꍇ��return
    //    //    //if (respawnPointsList.Count == 0)
    //    //    //{
    //    //    //    Debug.Log("���X�|�[���ʒu���Ȃ��Ȃ����B");
    //    //    //    return;
    //    //    //}
    //    //    ////�X�|�[���n�_���擾
    //    //    ////newCountClient����[1]����Ȃ̂ŁAoldCountClient���g�p����B
    //    //    //Vector3 respawnPoint = respawnPointsList[oldCountClient].transform.position;
    //    //    ////PlayerAvatar�𐶐�
    //    //    //PlayerAvaterSpwanRpc(clientId, respawnPoint);
    //    //};

    //    ////base.OnNetworkSpawn();
    //    ////int ownerClientId = NetworkManager.Singleton.GetComponent<NetworkIdentity>().netId;
    //    //// �N���C�A���gID���擾
    //    //ulong clientId = NetworkManager.Singleton.LocalClientId;
    //    ////ulong clientId = OwnerClientId;
    //    //Debug.Log("clientId !! " + clientId);
    //    ////Debug.Log("OwnerClientId = " + clientId);


    //    ////int clientCount = clientListData.DataList.Count;
    //    ////Debug.Log(clientCount);
    //    ////Debug.Log("clientCount " + clientCount);
    //    ////// ���X�|�[�����X�g��0�̏ꍇ��return
    //    ////if (respawnPointsList.Count == 0)
    //    ////{
    //    ////    Debug.Log("���X�|�[���ʒu���Ȃ��Ȃ����B");
    //    ////    return;
    //    ////}
    //    ////// �X�|�[���n�_���擾
    //    ////// newCountClient����[1]����Ȃ̂ŁAoldCountClient���g�p����B
    //    ////Vector3 respawnPoint = respawnPointsList[clientCount].transform.position;
    //    ////// PlayerAvatar�𐶐�
    //    ////PlayerAvaterSpwanRpc(clientId, respawnPoint);

    //    ////if (IsHost)
    //    ////{
    //    ////    Vector3 respawnPoint = respawnPointsList[countClient.Value].transform.position;
    //    ////    //PlayerAvatar�𐶐�
    //    ////    PlayerAvaterSpwanRpc(clientId, respawnPoint);
    //    ////}

    //    //// �l�b�g���[�N�����ϐ��̒l���ς������Ă΂�鏈��
    //    //countClient.OnValueChanged += (int oldCountClient, int newCountClient) =>
    //    //{
    //    //    if (clientId != OwnerClientId)
    //    //    {
    //    //        Debug.Log("�����̂����Ȃ��̂Ń��^�[��");
    //    //        return;
    //    //    }
    //    //    Debug.Log("newCountClient " + newCountClient);
    //    //    //���X�|�[�����X�g��0�̏ꍇ��return
    //    //    if (respawnPointsList.Count == 0)
    //    //    {
    //    //        Debug.Log("���X�|�[���ʒu���Ȃ��Ȃ����B");
    //    //        return;
    //    //    }
    //    //    //�X�|�[���n�_���擾
    //    //    //newCountClient����[1]����Ȃ̂ŁAoldCountClient���g�p����B
    //    //    Vector3 respawnPoint = respawnPointsList[oldCountClient].transform.position;
    //    //    //PlayerAvatar�𐶐�
    //    //    PlayerAvaterSpwanRpc(clientId, respawnPoint);
    //    //};

    //    //GetClientCountRpc();

    //    //RespanListRpc(respawnPointsList);

    //    // ���ݐڑ����Ă���l�����擾
    //    //GetClientCountRpc();



    //    //Debug.Log("ClientCount " + clientCount);


    //    //PlayerAvaterSpwanRpc(clientId);

    //}


    //void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    //{

    //}


    // �N���C�A���gID�ɑΉ�����v���C���[�l�b�g���[�N�I�u�W�F�N�g���擾
    //var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);


    //[Rpc(SendTo.Server)]
    //void RespanListRpc(List<Transform> respawnPointsList)
    //{
    //    // ���X�|�[���n�_���Ȃ������ꍇ
    //    if (respawnPointsList.Count == 0)
    //    {
    //        Debug.Log("���X�g���Ȃ��Ȃ�܂����B");
    //        return;
    //    }

    //    // �X�|�[���n�_���擾
    //    Transform respawnPoint = respawnPointsList[0];
    //    //spwanPoint = respawnPoint.position;
    //    // �擾�����X�|�[���n�_���폜
    //    respawnPointsList.RemoveAt(0);
    //    return;
    //}





    ///// <summary>
    ///// ���ݐڑ����Ă���l�����擾
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
    //    Debug.Log("clear���Ă�H�H");
    //    clientListData.DataList.Clear();
    //}


    ///// <summary>
    ///// PlayerAvatar�𐶐�
    ///// </summary>
    ///// <param name="clientId"></param>
    //[Rpc(SendTo.Server)]
    //void PlayerAvaterSpwanRpc(ulong clientId, Vector3 respawnPoint)
    //{
    //    Debug.Log(respawnPoint);
    //    //// �T�[�o�[��ŃI�u�W�F�N�g���C���X�^���X���X�|�[��������B
    //    NetworkManager.SpawnManager.InstantiateAndSpawn(Player, clientId, false, true, true, respawnPoint);

    //    //// Tag�����I�u�W�F�N�g���擾
    //    //GameObject[] characterObjArray = GameObject.FindGameObjectsWithTag("Character");

    //    //foreach (GameObject obj in characterObjArray)
    //    //{
    //    //    Debug.Log("���[�v���Ă�");
    //    //    NetworkObject identity = obj.GetComponent<NetworkObject>();
    //    //    ulong obj_id = identity.OwnerClientId;

    //    //    if (obj_id == clientId)
    //    //    {
    //    //        // �V���� ClientData �I�u�W�F�N�g���쐬
    //    //        ClientData clientData = new ClientData();
    //    //        clientData.NetId = clientId;
    //    //        clientData.ClientObj = obj;
    //    //        clientListData.DataList.Add(clientData);
    //    //        Debug.Log("�X�N���v�g�e�[�u���ɒǉ����܂����B");
    //    //    }
    //    //}

    //    //List<NetworkObject> hoge = new List<NetworkObject>
    //    //{
    //    //    NetworkManager.SpawnManager.GetPlayerNetworkObject(clientId)
    //    //};

    //    //// �擾�����I�u�W�F�N�g�̖��O���o��
    //    //foreach (NetworkObject obj in hoge)
    //    //{
    //    //    Debug.Log(obj.name);
    //    //}

    //    //// tag ���t���Ă���I�u�W�F�N�g���擾
    //    //NetworkObject characterObject = hoge.Find(obj => obj.gameObject.tag == "Character");

    //    //if (characterObject != null)
    //    //{
    //    //    // �擾�����I�u�W�F�N�g���g�p
    //    //    Debug.Log(characterObject.name);
    //    //}
    //    //else
    //    //{
    //    //    // �I�u�W�F�N�g��������Ȃ�����
    //    //    Debug.Log("Character object not found");
    //    //}

    //    //// Tag�����I�u�W�F�N�g���擾
    //    //GameObject[] characterObj = GameObject.FindGameObjectsWithTag("Character");

    //    //// �V���� ClientData �I�u�W�F�N�g���쐬
    //    //ClientData clientData = new ClientData();
    //    //clientData.NetId = clientId;
    //    //clientData.ClientObj = characterObj;

    //    //// ���X�g�ɒǉ�
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
    //    // �v���C���[�̃��X�|�[���𖳌��ɂ���
    //    base.OnPlayerAdded(conn, player, false);
    //}

    //NetworkObject.SpawnManager

}