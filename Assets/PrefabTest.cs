using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        // Cube�v���n�u��GameObject�^�Ŏ擾
        // Cube�v���n�u�����ɁA�C���X�^���X�𐶐��A
        Instantiate(obj, new Vector3(0.0f, 2.0f, 0.0f), Quaternion.identity);
    }
}
