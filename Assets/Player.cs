using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;
public class Player : NetworkBehaviour
{
    [SerializeField]
    public GameObject playerAvaterObj;

    [SerializeField]
    public NetworkAvatarGuidState networkAvatarGuidState;

    //[SerializeField]
    //NetworkAvatarGuidState m_NetworkAvatarGuidState;

    //public NetworkAvatarGuidState NetworkAvatarGuidState => m_NetworkAvatarGuidState;

    //[SerializeField]
    //private List<GameObject> respawnPointsList = new List<GameObject>();
    //[SerializeField]
    //private NetworkObject CharacterAvatar;

    //public Vector3 respawnPoint;

    //////�l�b�g���[�N�����ϐ�
    //public NetworkVariable<int> countClient = new NetworkVariable<int>(
    //        0,
    //        NetworkVariableReadPermission.Everyone,     // �ǂݍ��݊Ǘ���
    //        NetworkVariableWritePermission.Server       // ���������Ǘ���
    //    );

    public override void OnNetworkSpawn()
    {
        gameObject.name = "PersistentPlayer" + OwnerClientId;

        //if (respawnPointsList.Count == 0)
        //{
        //    Debug.Log("���X�|�[���ʒu���Ȃ��Ȃ����B");
        //    return;
        //}

        ////// �l�b�g���[�N�����ϐ��̒l���ς������Ă΂�鏈��
        //countClient.OnValueChanged += (int oldCountClient, int newCountClient) =>
        //{
        //    Debug.Log("oldCountClient " + oldCountClient);
        //    Debug.Log("newCountClient " + newCountClient);

        //    //�X�|�[���n�_���擾
        //    //newCountClient����[1]����Ȃ̂ŁAoldCountClient���g�p����B
        //    Vector3 respawnPoint = respawnPointsList[oldCountClient].transform.position;
        //    //PlayerAvatar�𐶐�
        //    Debug.Log("==== PlayerAvaterSpawnRPC ���s  ====");
        //    if (IsServer)
        //    {
        //        PlayerAvaterSpwanRpc(OwnerClientId, respawnPoint);
        //    }
        //};

        //if (IsServer)
        //{
        //    GetClientCountRpc();
        //}



    }

    ///// <summary>
    ///// ���ݐڑ����Ă���l�����擾
    ///// </summary>
    //[Rpc(SendTo.Server, RequireOwnership = false)]
    //void GetClientCountRpc()
    //{
    //    countClient.Value = NetworkManager.Singleton.ConnectedClientsList.Count;
    //}


    ///// <summary>
    ///// PlayerAvatar�𐶐�
    ///// </summary>
    ///// <param name="clientId"></param>
    //[Rpc(SendTo.Server, RequireOwnership = false)]
    //void PlayerAvaterSpwanRpc(ulong clientId, Vector3 respawnPoint)
    //{
    //    ////// �T�[�o�[��ŃI�u�W�F�N�g���C���X�^���X���X�|�[��������B
    //    NetworkManager.SpawnManager.InstantiateAndSpawn(CharacterAvatar, clientId, false, true, true, respawnPoint);
    //}

    //private Player m_Player;
    //[SerializeField]
    //PersistentPlayerRuntimeCollection m_PersistentPlayerRuntimeCollection;
    //public override void OnNetworkSpawn()
    //{
    //    gameObject.name = "Player" + OwnerClientId; // �Q�[���I�u�W�F�N�g�̖��O��ݒ�
    //    m_Player = (Player)this;
    //    m_PersistentPlayerRuntimeCollection.Add(m_Player);
    //}
    //private void Update()
    //{
    //    if (IsOwner == false)
    //    {
    //        return;
    //    }


    //    Vector2 direction = new Vector2()
    //    {
    //        x = Input.GetAxisRaw("Horizontal"),
    //        y = Input.GetAxisRaw("Vertical")
    //    };
    //    float moveSpeed = 3f;
    //    transform.Translate(direction * moveSpeed * Time.deltaTime);
    //}

    // �I�u�W�F�N�g���X�|�[�������甭�����鏈��
    //public override void OnNetworkSpawn()
    //{
    //    if (IsClient)
    //    {
    //        Debug.Log("�N���C�A���g���ڑ����܂����B");
    //    }
    //}
}
