using Cinemachine;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ServerCharacter : NetworkBehaviour
{
    [SerializeField]
    public GameObject[] items;

    [SerializeField]
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField]
    private GameObject player;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Debug.Log("ÉäÉXÉ|Å[ÉìÇµÇΩ" + NetworkManager.Singleton.LocalClientId);
            cinemachineVirtualCamera.Follow = player.transform;
            cinemachineVirtualCamera.LookAt = player.transform;
            Instantiate(cinemachineVirtualCamera);
        }
    }


    [ServerRpc]
    public void RecvDoActionServerRPC(ActionRequestData data)
    {
        ActionRequestData data1 = data;
        PlayerActionClientRpc(data1);
    }

    [ClientRpc]
    void PlayerActionClientRpc(ActionRequestData data) 
    {
        Debug.Log(data.clientId);
        var itemObj = items[data.RequestedActionID];
        itemObj.transform.position = data.SpawnPosition;
        Instantiate(itemObj);

        if (itemObj && itemObj.TryGetComponent(out NetworkObject item))
        {
            //item.SpawnWithOwnership(data.clientId, true);
        }
    }

    //public void SendCharacterAction(int actionId, ulong clientId, Vector3 position)
    //{
    //    if (!IsServer)
    //    {
    //        return;
    //    }
    //    Debug.Log(actionId);
    //    var itemObj = items[actionId];
    //    Instantiate(itemObj);
    //    itemObj.transform.position = position;
    //    if (itemObj && itemObj.TryGetComponent(out NetworkObject item))
    //    {
    //        item.Spawn();
    //    }
    //}

    //[ClientRpc(RequireOwnership = false)]
    //public void SendCharacterActionClientRpc(int actionId, ulong clientId, Vector3 position)
    //{
    //    Debug.Log(actionId);
    //    var itemObj = items[actionId];
    //    Instantiate(itemObj);
    //    itemObj.transform.position = position;
    //    if (itemObj && itemObj.TryGetComponent(out NetworkObject item))
    //    {
    //        item.Spawn();
    //    }
    //}


}
