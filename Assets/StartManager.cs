using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartManager : NetworkBehaviour
{
    ////�R�C���̃v���n�u
    //[SerializeField] private NetworkObject m_coinPrefab;
    ////���������R�C���̃��X�g
    //private List<GameObject> m_coinObjects = new List<GameObject>();

    ////�X�|�[�����ꂽ�Ƃ�
    //public override void OnNetworkSpawn()
    //{
    //    Debug.Log("=== LOAD SELECT SECENE");
    //    ////�z�X�g�̏ꍇ
    //    //if (IsHost)
    //    //{
    //    //    GenerateCoin();
    //    //}
    //}

    //// �R�C������
    //public void GenerateCoin()
    //{
    //    //�R�C������
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

    ////�ȈՓI�ȃV���O���g��
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
