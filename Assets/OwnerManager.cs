using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OwnerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            // オーナー処理
            Debug.Log("オーナーが接続しました");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
