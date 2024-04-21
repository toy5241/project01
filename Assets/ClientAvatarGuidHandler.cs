using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

[RequireComponent(typeof(NetworkAvatarGuidState))]
public class ClientAvatarGuidHandler : NetworkBehaviour
{
    [SerializeField]
    NetworkAvatarGuidState m_NetworkAvatarGuidState;

    [SerializeField]
    GameObject[] m_SpriteRenderer;

    [SerializeField]
    Transform parentObj;


    public override void OnNetworkSpawn()
    {
        
        if (IsClient)
        {
            //InstantiateAvatar();
        }
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void CallInstantiateAvatarServerRpc(int num)
    {
        InstantiateAvatarClientRpc(num);
    }

    [ClientRpc(RequireOwnership = false)]
    public void InstantiateAvatarClientRpc(int num)
    {
        //Instantiate(m_SpriteRenderer[m_NetworkAvatarGuidState.avatarInt.Value], parentObj);
        Instantiate(m_SpriteRenderer[num], parentObj);
    }


    [ServerRpc]
    void CallRespawnCameraServerRpc()
    {
        Debug.Log("=== CallRespawnCameraServerRpc ===");
        var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.Singleton.LocalClientId);
        if (playerNetworkObject && playerNetworkObject.TryGetComponent(out ServerCharacter serverCharacter))
        {
            Debug.Log(serverCharacter.gameObject.tag);
            if (serverCharacter.gameObject.tag == "Player")
            {

            }
        }
        else
        {
            Debug.Log("éÊìæÇ≈Ç´Ç‹ÇπÇÒÇ≈ÇµÇΩÅB");
        }


        RespawnCameraClientRpc();
    }

    [ClientRpc]
    void RespawnCameraClientRpc()
    {

    }

}
