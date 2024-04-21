using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;




public class ServerState : NetworkBehaviour
{
    [SerializeField]
    [Tooltip("Make sure this is included in the NetworkManager's list of prefabs!")]
    private NetworkObject m_PlayerPrefab;

    public override void OnNetworkSpawn()
    {
        //NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
        // ���ׂĂ̐ڑ����ꂽ�N���C�A���g��ID���o�͂���


        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            Debug.Log("======== �T�[�o�[�X�e�[�g�����Ă܂��I�I  ==============");
            Debug.Log(client.Key);
            SpwanPlayerServerRpc(client.Key);
        }
    }

    void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        // ���ׂĂ̐ڑ����ꂽ�N���C�A���g��ID���o�͂���
        foreach (var client in NetworkManager.Singleton.ConnectedClients)
        {
            Debug.Log(client.Key);
            SpwanPlayerServerRpc(client.Key);
        }


    }

    [ServerRpc(RequireOwnership = false)]   
    public void SpwanPlayerServerRpc(ulong clientId) {
        // �N���C�A���gID�ɑΉ�����v���C���[�l�b�g���[�N�I�u�W�F�N�g���擾
        var playerNetworkObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);

        var newPlayer = Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity);
        // �v���C���[�L�����N�^�[���V�[���ƂƂ��ɔj������悤�ɂ��ăX�|�[��
        newPlayer.SpawnWithOwnership(clientId, true);
    }
}
