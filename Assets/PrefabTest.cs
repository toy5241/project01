using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        // CubeプレハブをGameObject型で取得
        // Cubeプレハブを元に、インスタンスを生成、
        Instantiate(obj, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
    }
}
