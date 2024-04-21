using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // 生成するオブジェクトのプレハブをインスペクターからアサインする
    public GameObject objectPrefab;
    public Transform parent;

    // シーンが読み込まれたときに呼ばれるメソッド
    private void OnEnable()
    {
        Debug.Log("OnEnable実行");

        // オブジェクトを生成する
        Instantiate(objectPrefab);
    }
    void Awake()
    {
        Debug.Log("Awake実行");
    }

    void Start()
    {
        Debug.Log("Start実行");
    }
}
