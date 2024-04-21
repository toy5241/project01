using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerStartUp : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        bool isServer = false;
        ushort serverPort = 7777;
        string externalServerIP = "0.0.0.0";

        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-dedicatedServer")
            {
                isServer = true;
            }
            else if (args[i] == "-port" && i + 1 < args.Length)
            {
                serverPort = ushort.Parse(args[i + 1]);
            }
            else if (args[i] == "-ip" && i + 1 < args.Length)
            {
                externalServerIP = args[i + 1];
            }
        }
        if (isServer)
        {
            StartServer(externalServerIP, serverPort);
        }
    }

    private void StartServer(string ip, ushort port)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.SetClientSynchronizationMode(LoadSceneMode.Additive);
        NetworkManager.Singleton.SceneManager.LoadScene("LobyScene", LoadSceneMode.Single);
    }
}
