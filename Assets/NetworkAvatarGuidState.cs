using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkAvatarGuidState : NetworkBehaviour
{
    // �v���C���[�̃v���n�u
    //public GameObject playerPrefab;

    //private static GameObject tempobj;

    public GameObject avatar;

    public NetworkVariable<int> avatarInt = new NetworkVariable<int>(
        0,                                          // �����l
        NetworkVariableReadPermission.Everyone,     // �ǂݎ�茠��
        NetworkVariableWritePermission.Server       // �������݌���
        );

    public int avatarCount = 0;

    //public NetworkVariable<GameObject> avatar = new NetworkVariable<GameObject>(
    //    tempobj,                                          // �����l
    //    NetworkVariableReadPermission.Everyone,     // �ǂݎ�茠��
    //    NetworkVariableWritePermission.Owner        // �������݌���
    //    );

}
