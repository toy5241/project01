using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            //ƒV[ƒ“‚ğØ‚è‘Ö‚¦
        });
    }
}
