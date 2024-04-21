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
    // ルーム参加コードを入力するためのUIフィールド
    [SerializeField] TMP_InputField joinCodeInput;
    [SerializeField] public string m_SceneName;

    /***********************
    マッチングサーバーを使用してのマッチング
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
    ここまで
    *************************/


    /***********************
        Relayサーバーを使用してのマッチング
    *************************/
    private async void Start()
    {
        // Unity Services を初期化します
        await UnityServices.InitializeAsync();

        // 匿名ログインが完了したときの処理を追加します
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("プレイヤーID: " + AuthenticationService.Instance.PlayerId + " でログインしました");
        };

        // 匿名ログインを実行します
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // ルーム作成ボタンが押されたときの処理
    public async void CreateRelayButton()
    {
        try
        {
            // 最大接続人数3人のルームを作成します
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            // ルーム参加コードを取得します
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            // ホスト側のRelayサーバー情報とキー情報を設定します
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
      allocation.RelayServer.IpV4,
      (ushort)allocation.RelayServer.Port,
      allocation.AllocationIdBytes,
      allocation.Key,
      allocation.ConnectionData
      );

            Debug.Log("ルーム参加コード: " + joinCode);

            // ホストとしてゲームを開始します
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

    // ルーム参加ボタンが押されたときの処理
    public void JoinRelayButton()
    {
        // 入力されたルーム参加コードを使ってルームに参加します
        JoinRelay(joinCodeInput.text);
    }

    // ルーム参加処理
    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("ルーム参加コード: " + joinCode);

            // ルーム参加コードからルーム情報を取得します
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            // クライアント側のRelayサーバー情報とキー情報を設定します
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
      joinAllocation.RelayServer.IpV4,
      (ushort)joinAllocation.RelayServer.Port,
      joinAllocation.AllocationIdBytes,
      joinAllocation.Key,
      joinAllocation.ConnectionData,
      joinAllocation.HostConnectionData
      );

            // クライアントとしてゲームに参加します
            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.SceneManager.PostSynchronizationSceneUnloading = true; // <-------- Unloads any scenes not used
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    /***********************
        ここまで
    *************************/


    //public virtual void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    //{
    //    var player = (GameObject)GameObject.Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);
    //    NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    //}
}
