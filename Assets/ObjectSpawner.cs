using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    // ��������I�u�W�F�N�g�̃v���n�u���C���X�y�N�^�[����A�T�C������
    public GameObject objectPrefab;
    public Transform parent;

    // �V�[�����ǂݍ��܂ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    private void OnEnable()
    {
        Debug.Log("OnEnable���s");

        // �I�u�W�F�N�g�𐶐�����
        Instantiate(objectPrefab);
    }
    void Awake()
    {
        Debug.Log("Awake���s");
    }

    void Start()
    {
        Debug.Log("Start���s");
    }
}
