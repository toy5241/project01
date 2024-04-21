using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Networking.Transport;

public class RelayTest : MonoBehaviour
{
    // ���[���Q���R�[�h����͂��邽�߂�UI�t�B�[���h
    [SerializeField] TMP_InputField joinCodeInput;
    [SerializeField] public string m_SceneName;

    /***********************
    �}�b�`���O�T�[�o�[���g�p���Ẵ}�b�`���O
    *************************/
    /**
    public void CreateRelayButton()
    {
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Additive);
        NetworkManager.Singleton.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
    }

    public void JoinRelayButton()
    {
        NetworkManager.Singleton.StartClient();
        NetworkManager.Singleton.SceneManager.PostSynchronizationSceneUnloading = true;
    }
    **/
    /***********************
    �����܂�
    *************************/


    /***********************
        Relay�T�[�o�[���g�p���Ẵ}�b�`���O
    *************************/
    private async void Start()
    {
        // Unity Services �����������܂�
        await UnityServices.InitializeAsync();

        // �������O�C�������������Ƃ��̏�����ǉ����܂�
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("�v���C���[ID: " + AuthenticationService.Instance.PlayerId + " �Ń��O�C�����܂���");
        };

        // �������O�C�������s���܂�
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // ���[���쐬�{�^���������ꂽ�Ƃ��̏���
    public async void CreateRelayButton()
    {
        try
        {
            // �ő�ڑ��l��3�l�̃��[�����쐬���܂�
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            // ���[���Q���R�[�h���擾���܂�
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            // �z�X�g����Relay�T�[�o�[���ƃL�[����ݒ肵�܂�
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
      allocation.RelayServer.IpV4,
      (ushort)allocation.RelayServer.Port,
      allocation.AllocationIdBytes,
      allocation.Key,
      allocation.ConnectionData
      );

            Debug.Log("���[���Q���R�[�h: " + joinCode);

            // �z�X�g�Ƃ��ăQ�[�����J�n���܂�
            NetworkManager.Singleton.StartHost();
            //NetworkManager.Singleton.SceneManager.ActiveSceneSynchronizationEnabled = true;
            //NetworkManager.Singleton.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);

            NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Additive); // <-------- Prevents initial issue
            NetworkManager.Singleton.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);

        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    // ���[���Q���{�^���������ꂽ�Ƃ��̏���
    public void JoinRelayButton()
    {
        // ���͂��ꂽ���[���Q���R�[�h���g���ă��[���ɎQ�����܂�
        JoinRelay(joinCodeInput.text);
    }

    // ���[���Q������
    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("���[���Q���R�[�h: " + joinCode);

            // ���[���Q���R�[�h���烋�[�������擾���܂�
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // �N���C�A���g����Relay�T�[�o�[���ƃL�[����ݒ肵�܂�
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
      joinAllocation.RelayServer.IpV4,
      (ushort)joinAllocation.RelayServer.Port,
      joinAllocation.AllocationIdBytes,
      joinAllocation.Key,
      joinAllocation.ConnectionData,
      joinAllocation.HostConnectionData
      );

            // �N���C�A���g�Ƃ��ăQ�[���ɎQ�����܂�
            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.SceneManager.PostSynchronizationSceneUnloading = true; // <-------- Unloads any scenes not used
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    /***********************
        �����܂�
    *************************/


    //public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //}
}
