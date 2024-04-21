using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class NetworkAvatarGuidState : NetworkBehaviour
{
    // プレイヤーのプレハブ
    //public GameObject playerPrefab;

    //private static GameObject tempobj;

    public GameObject avatar;

    public NetworkVariable<int> avatarInt = new NetworkVariable<int>(
        0,                                          // 初期値
        NetworkVariableReadPermission.Everyone,     // 読み取り権限
        NetworkVariableWritePermission.Server       // 書き込み権限
        );

    public int avatarCount = 0;

    //public NetworkVariable<GameObject> avatar = new NetworkVariable<GameObject>(
    //    tempobj,                                          // 初期値
    //    NetworkVariableReadPermission.Everyone,     // 読み取り権限
    //    NetworkVariableWritePermission.Owner        // 書き込み権限
    //    );

}
