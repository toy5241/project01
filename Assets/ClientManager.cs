using Unity.Netcode;
using UnityEngine;

public class ClientManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            Debug.Log("クライアントが接続しました。");
        }
    }

}
