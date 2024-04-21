using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.Collections;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkCharaSelection : NetworkBehaviour
{

    public struct LobbyPlayerState : INetworkSerializable, IEquatable<LobbyPlayerState>
    {
        public ulong ClientId;

        public int PlayerNumber; // this player's assigned "P#". (0=P1, 1=P2, etc.)
        public float LastChangeTime;
        public int SeatIdx;
        public int RespawnNumber;
        public bool ReadyState;
        public bool IsHost;

        public LobbyPlayerState(ulong clientId, int playerNumber, int respawnNumber, bool isHost ,float lastChangeTime = 0, int seatIdx = -1, bool readyState = false)
        {
            ClientId = clientId;
            PlayerNumber = playerNumber;
            RespawnNumber = respawnNumber;
            IsHost = isHost;
            LastChangeTime = lastChangeTime;
            SeatIdx = seatIdx;
            ReadyState = readyState;
        }

        // ƒŠƒXƒg‚Ì’l‚ð”äŠr‚·‚é‚à‚Ì
        public bool Equals(LobbyPlayerState other)
        {
            return ClientId == other.ClientId &&
                   PlayerNumber == other.PlayerNumber &&
                   RespawnNumber == other.RespawnNumber &&
                   IsHost == other.IsHost &&
                   LastChangeTime.Equals(other.LastChangeTime) &&
                   SeatIdx == other.SeatIdx &&
                   ReadyState == other.ReadyState;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref ClientId);
            serializer.SerializeValue(ref PlayerNumber);
            serializer.SerializeValue(ref RespawnNumber);
            serializer.SerializeValue(ref IsHost);
            serializer.SerializeValue(ref LastChangeTime);
            serializer.SerializeValue(ref SeatIdx);
            serializer.SerializeValue(ref ReadyState);
        }

    }
    //public Avatar[] AvatarConfiguration;
    public GameObject[] AvatarConfiguration;
    
    private NetworkList<LobbyPlayerState> m_LobbyPlayers;
    private void Awake()
    {
        m_LobbyPlayers = new NetworkList<LobbyPlayerState>();
    }
    public NetworkList<LobbyPlayerState> LobbyPlayers => m_LobbyPlayers;

    /// <summary>
    /// Server notification when a client requests a different lobby-seat, or locks in their seat choice
    /// </summary>
    public event Action<ulong, int> OnClientChangedSeat;

    public event Action<ulong, bool> OnClientChangedReadyState;


    public event Action OnStartGame;


    /// <summary>
    /// RPC to notify the server that a client has chosen a seat.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void ChangeSeatServerRpc(ulong clientId, int seatIdx)
    {
        OnClientChangedSeat?.Invoke(clientId, seatIdx);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeReadyStateServerRpc(ulong clientId, bool isReady)
    {
        OnClientChangedReadyState?.Invoke(clientId, isReady);
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartGameServerRpc()
    {
        //NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        //SceneLoaderWrapper.Instance.LoadScene("GameScene", true);
        //NetworkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        OnStartGame?.Invoke();
    }

}
