using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            //シーンを切り替え

        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            //シーンを切り替え
        });
    }
}