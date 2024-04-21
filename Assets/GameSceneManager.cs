using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    NetcodeHooks m_NetcodeHooks;

    [SerializeField]
    private NetworkObject m_PlayerPrefab;

    [SerializeField]
    public SpriteRenderer[] spriteRenderer;

    [SerializeField]
    CinemaChineCamera cinemaChineCamera;



    protected void Awake()
    {
        m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;

    }

    public void OnNetworkSpawn()
    {
        Debug.Log(NetworkManager.Singleton.IsServer);
        //NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        //NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
        //NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += OnSynchronizeComplete;
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
    }

    void SpawnPlayer(ulong clientId)
    {
        var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);
        if (playerNetworkObject && playerNetworkObject.TryGetComponent(out Player persistentPlayer))
        {
            var newPlayer = Instantiate(m_PlayerPrefab);
            newPlayer.SpawnWithOwnership(clientId, true);
            newPlayer.GetComponent<NetworkAvatarGuidState>().avatarInt.Value = persistentPlayer.networkAvatarGuidState.avatarInt.Value;
            newPlayer.GetComponent<ClientAvatarGuidHandler>().CallInstantiateAvatarServerRpc(persistentPlayer.networkAvatarGuidState.avatarInt.Value);
        }
    }

    void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        Debug.Log("=== ÉQÅ[ÉÄÉVÅ[ÉìÇ™ì«Ç›çûÇ‹ÇÍÇ‹ÇµÇΩ!! ===");

        if (NetworkManager.Singleton.IsServer)
        {
            foreach (var kvp in NetworkManager.Singleton.ConnectedClients)
            {
                SpawnPlayer(kvp.Key);
            }
            //if (loadSceneMode == LoadSceneMode.Single)
            //{
            //    foreach (var kvp in NetworkManager.Singleton.ConnectedClients)
            //    {
            //        SpawnPlayer(kvp.Key);
            //    }
            //}
        }
    }
}
