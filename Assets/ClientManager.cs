using Unity.Netcode;
using UnityEngine;

public class ClientManager : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            Debug.Log("�N���C�A���g���ڑ����܂����B");
        }
    }

}
