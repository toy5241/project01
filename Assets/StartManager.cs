using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartManager : NetworkBehaviour
{
    ////コインのプレハブ
    //[SerializeField] private NetworkObject m_coinPrefab;
    ////生成したコインのリスト
    //private List<GameObject> m_coinObjects = new List<GameObject>();

    ////スポーンされたとき
    //public override void OnNetworkSpawn()
    //{
    //    Debug.Log("=== LOAD SELECT SECENE");
    //    ////ホストの場合
    //    //if (IsHost)
    //    //{
    //    //    GenerateCoin();
    //    //}
    //}

    //// コイン生成
    //public void GenerateCoin()
    //{
    //    //コイン生成
    //    for (int x = 0; x < 10; x++)
    //    {
    //        NetworkObject coin = Instantiate(m_coinPrefab);
    //        int posX = UnityEngine.Random.Range(0, 10) - 5;
    //        int posZ = UnityEngine.Random.Range(0, 10) - 5;
    //        coin.transform.position = new Vector3(posX, 0, posZ);
    //        coin.Spawn();
    //        m_coinObjects.Add(coin.gameObject);
    //    }
    //}

    ////簡易的なシングルトン
    //private static TempObjSpwan instance;
    //public static TempObjSpwan Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = (TempObjSpwan)FindObjectOfType(typeof(TempObjSpwan));
    //        }

    //        return instance;
    //    }
    //}

}
