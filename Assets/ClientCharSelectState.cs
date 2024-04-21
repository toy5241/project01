using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;
using System;
using Unity.VisualScripting;
using static NetworkCharaSelection;
using NUnit.Framework;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Xml;

[RequireComponent(typeof(NetcodeHooks))]
public class ClientCharSelectState : MonoBehaviour
{
    public static ClientCharSelectState Instance { get; private set; }

    [SerializeField]
    NetcodeHooks m_NetcodeHooks;

    [SerializeField]
    NetworkCharaSelection m_NetworkCharSelection;

    [Header("Lobby Seats")]
    [SerializeField]
    [Tooltip("")]
    List<UICharSelectPlayerSeat> m_PlayerSeats;

    [SerializeField]
    Button readySelectBtn;

    [SerializeField]
    GameObject m_CharacterGraphicsParent;

    [SerializeField]
    public List<Transform> respawnPointsList = new List<Transform>();

    [SerializeField]
    public TextMeshProUGUI playerCountTextUI;

    [SerializeField]
    public TextMeshProUGUI readyPlayerCountTextUI;

    [SerializeField]
    public GameObject startBtnGameObj;

    [SerializeField]
    public Button startBtnUI;


    GameObject m_CurrentCharacterGraphics;

    int m_LastSeatSelected = -1;


    List<Dictionary<string, GameObject>> playerAvaterList = new List<Dictionary<string, GameObject>>();

    protected void Awake()
    {
        Instance = this;
        m_NetcodeHooks.OnNetworkSpawnHook += OnNetworkSpawn;
    }

    protected void Start()
    {
        for (int i = 0; i < m_PlayerSeats.Count; ++i)
        {
            m_PlayerSeats[i].Initialize(i);
        }

        //UpdateCharacterSelection(NetworkCharaSelection.SeatState.Inactive);
    }


    public void OnNetworkSpawn()
    {
        if (!NetworkManager.Singleton.IsClient)
        {
            enabled = false;
        }
        else
        {
            m_NetworkCharSelection.LobbyPlayers.OnListChanged += OnLobbyPlayerStateChanged;
        }
    }

    void OnLobbyPlayerStateChanged(NetworkListEvent<NetworkCharaSelection.LobbyPlayerState> changeEvent)
    {
        UpdatePlayerCount();

        int localPlayerIdx = -1;
        for (int i = 0; i < m_NetworkCharSelection.LobbyPlayers.Count; ++i)
        {
            Debug.Log("プレイヤーリストのクライアントID :" + m_NetworkCharSelection.LobbyPlayers[i].ClientId);
            UpdateCharacterSelection(i);

            Debug.Log(NetworkManager.Singleton.LocalClientId);

            if (m_NetworkCharSelection.LobbyPlayers[i].IsHost)
            {
                if(m_NetworkCharSelection.LobbyPlayers[i].ClientId == NetworkManager.Singleton.LocalClientId)
                startBtnDisplay();
            }

            //if (m_NetworkCharSelection.LobbyPlayers[i].ClientId == NetworkManager.Singleton.LocalClientId)
            //{
            //    localPlayerIdx = i;
            //    break;
            //}
        }



        // 準備完了プレイヤー
        UpdateReadyStateCount();


        //UpdatePlayerCount();

        //int localPlayerIdx = -1;
        //for (int i = 0; i < m_NetworkCharSelection.LobbyPlayers.Count; ++i)
        //{
        //    if (m_NetworkCharSelection.LobbyPlayers[i].ClientId == NetworkManager.Singleton.LocalClientId)
        //    {
        //        localPlayerIdx = i;
        //        break;
        //    }
        //}
        //Debug.Log("プレイヤーリストのクライアントID :" + m_NetworkCharSelection.LobbyPlayers[localPlayerIdx].ClientId);
        //UpdateCharacterSelection(m_NetworkCharSelection.LobbyPlayers[localPlayerIdx].SeatIdx, m_NetworkCharSelection.LobbyPlayers[localPlayerIdx].RespawnNumber);
    }

    void UpdatePlayerCount()
    {

        int count = m_NetworkCharSelection.LobbyPlayers.Count;
        var pstr = (count > 1) ? "players" : "player";

        //m_NumPlayersText.text = "<b>" + count + "</b> " + pstr + " connected";

        // プレイヤー人数テキストに人数をセット
        playerCountTextUI.text = count.ToString();
    }

    void startBtnDisplay()
    {
        startBtnUI.interactable = false;
        startBtnGameObj.SetActive(true);
    }

    /// <summary>
    /// Called directly by UI elements!
    /// </summary>
    /// <param name="seatIdx"></param>
    public void OnPlayerClickedSeat(int seatIdx)
    {
        if (m_NetworkCharSelection.IsSpawned)
        {
            m_NetworkCharSelection.ChangeSeatServerRpc(NetworkManager.Singleton.LocalClientId, seatIdx);
        }
    }

    /// <summary>
    /// Called directly by UI elements!
    /// </summary>
    /// <param name="isReady"></param>
    public void OnPlayerClickedReadyState(bool isReady)
    {
        if (m_NetworkCharSelection.IsSpawned)
        {
            m_NetworkCharSelection.ChangeReadyStateServerRpc(NetworkManager.Singleton.LocalClientId,isReady);
        }
    }


    void UpdateCharacterSelection(int index)
    {
        LobbyPlayerState lobbyPlayerState = m_NetworkCharSelection.LobbyPlayers[index];

        ulong clientId = lobbyPlayerState.ClientId;
        int seatIdx = lobbyPlayerState.SeatIdx;
        int respawnNumber = lobbyPlayerState.RespawnNumber;
        

        //if (playerAvaterList == null)
        //{
        //    // エラー処理
        //    return;
        //}

        //// 2列目が存在するかどうかを調べる
        //bool exists = playerAvaterList.ElementAtOrDefault(index).Count != 0;


        bool exists = false;

        // 要素が存在するかどうかを確認
        if (playerAvaterList != null && index < playerAvaterList.Count)
        {
            exists = true;
        }

        // 処理
        if (!exists)
        {
            // 列が存在しない場合
            playerAvaterList.Add(new Dictionary<string, GameObject>());
            return;
        }

        Dictionary<string, GameObject> playerAvaterDictionary = playerAvaterList[index];

        foreach (GameObject obj in playerAvaterDictionary.Values)
        {
            obj.SetActive(false);
        }
        if(seatIdx != -1)
        {
            if (!playerAvaterDictionary.TryGetValue(m_NetworkCharSelection.AvatarConfiguration[seatIdx].tag, out GameObject characterGraphics))
            {
                characterGraphics = Instantiate(m_NetworkCharSelection.AvatarConfiguration[seatIdx], respawnPointsList[respawnNumber].transform);
                playerAvaterDictionary.Add(m_NetworkCharSelection.AvatarConfiguration[seatIdx].tag, characterGraphics);
            }
            characterGraphics.SetActive(true);
        }


        return;

        //bool allKeysFound = playerAvaterList.TrueForAll(dictionary => dictionary.TryGetValue(clientId, out GameObject characterGraphics));


        //if (!playerAvaterList.TryGetValue(clientId, out GameObject characterGraphics))
        //{
        //    characterGraphics = Instantiate(m_NetworkCharSelection.AvatarConfiguration[seatIdx], respawnPointsList[respawnNumber].transform);
        //    playerAvaterList.Add(clientId, characterGraphics);
        //}

        //if (!playerAvaterList.TryGetValue(clientId, out GameObject characterGraphics))
        //{
        //    characterGraphics = Instantiate(m_NetworkCharSelection.AvatarConfiguration[seatIdx], respawnPointsList[respawnNumber].transform);
        //    playerAvaterList.Add(clientId, characterGraphics);
        //}

        //if (seatIdx != -1)
        //{

        //    if (!playerAvaterList.TryGetValue(, out GameObject characterGraphics))
        //    {
        //        characterGraphics = Instantiate(avatar, respawnPointsList[respawnNumber].transform);
        //        m_SpawnedCharacterGraphics.Add(avatar.tag, characterGraphics);
        //    }

        //    Debug.Log(NetworkManager.Singleton.LocalClientId);
        //        var selectedCharacterGraphics = GetCharacterGraphics(m_NetworkCharSelection.AvatarConfiguration[seatIdx], respawnNumber);

        //        if (m_CurrentCharacterGraphics)
        //        {
        //            m_CurrentCharacterGraphics.SetActive(false);
        //        }

        //        selectedCharacterGraphics.SetActive(true);
        //        m_CurrentCharacterGraphics = selectedCharacterGraphics;
        //}


        //bool isNewSeat = m_LastSeatSelected != seatIdx;
        //Debug.Log(seatIdx);
        //if (seatIdx != -1)
        //{
        //    if (isNewSeat)
        //    {
        //        //m_NetworkCharSelection.ChangeCharacterServerRpc(NetworkManager.Singleton.LocalClientId);
        //        Debug.Log(NetworkManager.Singleton.LocalClientId);
        //        var selectedCharacterGraphics = GetCharacterGraphics(m_NetworkCharSelection.AvatarConfiguration[seatIdx], respawnNumber);

        //        if (m_CurrentCharacterGraphics)
        //        {
        //            m_CurrentCharacterGraphics.SetActive(false);
        //        }

        //        selectedCharacterGraphics.SetActive(true);
        //        m_CurrentCharacterGraphics = selectedCharacterGraphics;
        //    }
        //}
    }

    void UpdateReadyStateCount()
    {
        int count = 0;

        readyPlayerCountTextUI.text = count.ToString();

        for (int i = 0; i < m_NetworkCharSelection.LobbyPlayers.Count; ++i)
        {
            bool readyState = m_NetworkCharSelection.LobbyPlayers[i].ReadyState;
            if (readyState)
            {
                count++;
                readyPlayerCountTextUI.text = count.ToString();
            }
        }

        bool isReady = true;

        foreach (LobbyPlayerState obj in m_NetworkCharSelection.LobbyPlayers)
        {
            if (!obj.ReadyState)
            {
                isReady = false;
                break;
            }

        }

        if (isReady)
        {
            startBtnUI.interactable = true;
        }

    }

    /// <summary>
    /// Called directly by UI elements!
    /// </summary>
    public void OnPlayerClickedStart()
    {
        if (m_NetworkCharSelection.IsSpawned)
        {
            // request to lock in or unlock if already locked in
            m_NetworkCharSelection.StartGameServerRpc();
        }
    }

    //GameObject GetCharacterGraphics(GameObject avatar,int respawnNumber)
    //{
    //    if (!m_SpawnedCharacterGraphics.TryGetValue(avatar.tag, out GameObject characterGraphics))
    //    {
    //        characterGraphics = Instantiate(avatar, respawnPointsList[respawnNumber].transform);
    //        m_SpawnedCharacterGraphics.Add(avatar.tag, characterGraphics);
    //    }

    //    return characterGraphics;

    //}

}
