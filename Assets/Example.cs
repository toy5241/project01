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
//        // ���݂̃I�u�W�F�N�g�����[�J���v���C���[���ǂ������m�F
//        if (NetworkBehaviour.IsLocalPlayer)
//        {
//            // NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject
//            // ���[�J���v���C���[�̃l�b�g���[�N�I�u�W�F�N�g���擾
//            var playerObject = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.LocalClientId);

//            // �v���C���[�I�u�W�F�N�g���� NetworkCharSelection �R���|�[�l���g���擾
//            var charSelection = playerObject.GetComponent<NetworkCharSelection>();

//            // NetworkCharSelection.LobbyPlayers
//            // ���r�[�Q���v���C���[�̏��ƃL�����N�^�[�I���X�e�[�^�X���擾
//            _lobbyPlayers = charSelection.LobbyPlayers;
//        }
//    }

//    private void Update()
//    {
//        // _lobbyPlayers �����[�v����
//        // �e�v���C���[�̏��ƃL�����N�^�[�I���X�e�[�^�X���m�F
//        foreach (var playerState in _lobbyPlayers)
//        {
//            Debug.Log($"Player Name: {playerState.PlayerName}");
//            Debug.Log($"Seat Index: {playerState.SeatIdx}");
//            Debug.Log($"Character Avatar Guid: {playerState.CharacterAvatarGuid}");
//        }
//    }
}
