using Unity.Netcode;
using UnityEngine;

public class Example : MonoBehaviour
{
//    public struct LobbyPlayerState
//    {
//        public string PlayerName;
//        public int SeatIndex;
//        public int CharacterAvatarGuid;
//    }

//    private NetworkList<LobbyPlayerState> _lobbyPlayers;

//    private void Awake()
//    {
//        // NetworkBehaviour.IsLocalPlayer 
//        // 現在のオブジェクトがローカルプレイヤーかどうかを確認
//        if (NetworkBehaviour.IsLocalPlayer)
//        {
//            // NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject
//            // ローカルプレイヤーのネットワークオブジェクトを取得
//            var playerObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.LocalClientId);

//            // プレイヤーオブジェクトから NetworkCharSelection コンポーネントを取得
//            var charSelection = playerObject.GetComponent<NetworkCharSelection>();

//            // NetworkCharSelection.LobbyPlayers
//            // ロビー参加プレイヤーの情報とキャラクター選択ステータスを取得
//            _lobbyPlayers = charSelection.LobbyPlayers;
//        }
//    }

//    private void Update()
//    {
//        // _lobbyPlayers をループ処理
//        // 各プレイヤーの情報とキャラクター選択ステータスを確認
//        foreach (var playerState in _lobbyPlayers)
//        {
//            Debug.Log($"Player Name: {playerState.PlayerName}");
//            Debug.Log($"Seat Index: {playerState.SeatIdx}");
//            Debug.Log($"Character Avatar Guid: {playerState.CharacterAvatarGuid}");
//        }
//    }
}
