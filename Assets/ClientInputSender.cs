using Cinemachine;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(ServerCharacter))]
public class ClientInputSender : NetworkBehaviour
{
    [SerializeField]
    NetworkAvatarGuidState m_NetworkAvatarGuidState;

    [SerializeField]
    public GameObject[] items;
    
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    public ClientInputSender clientInputSender;

    public ServerCharacter serverCharacter;

    int m_ActionRequestCount;

    readonly ActionRequest[] m_ActionRequests = new ActionRequest[1];

    struct ActionRequest
    {
        public ulong clientId;
        public int RequestedActionID;
        public Vector3 SpawnPosition;
    }

    float moveSpeed = 3f;

    Vector3 clickPosition;

    Rigidbody2D rbody2D;
    private void Start()
    {
        rbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RequestAction(0, Vector3.zero);
        }
        if (Input.GetMouseButtonDown(0))
        {
            clickPosition = Input.mousePosition;

            // ZŽ²C³
            clickPosition.z = 10f;
            Vector3 world_position = Camera.main.ScreenToWorldPoint(clickPosition);

            RequestAction(0, world_position);
        }
        if (Input.GetMouseButtonDown(1))
        {
            clickPosition = Input.mousePosition;

            // ZŽ²C³
            clickPosition.z = 10f;
            Vector3 world_position = Camera.main.ScreenToWorldPoint(clickPosition);

            RequestAction(1, world_position);
        }
        if (Input.GetKeyDown("space"))
        {
            // ã•ûŒü‚É—Í‚ð‰Á‚¦‚éŽ–‚ÅƒWƒƒƒ“ƒv‚·‚é
            rbody2D.AddForce(Vector2.up * 300);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsOwner == false)
        {
            return;
        }

        if (m_ActionRequestCount > 0)
        {
            Debug.Log("clientId = " + NetworkManager.Singleton.LocalClientId);
            int actionId = m_ActionRequests[0].RequestedActionID;
            ulong clientId = m_ActionRequests[0].clientId;
            Vector3 spawnPosition = m_ActionRequests[0].SpawnPosition;

            var data = new ActionRequestData();
            data.clientId = clientId;
            data.RequestedActionID = actionId;
            data.SpawnPosition = spawnPosition;

            serverCharacter.RecvDoActionServerRPC(data);


            //clientInputSender.SendCharacterActionServerRpc(actionId,clientId, spawnPosition);
        }

        m_ActionRequestCount = 0;

        Vector2 direction = new Vector2()
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = Input.GetAxisRaw("Vertical")
        };
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    public void RequestAction(int actionID, Vector3 position)
    {

        if (m_ActionRequestCount < m_ActionRequests.Length)
        {
            m_ActionRequests[m_ActionRequestCount].clientId = NetworkManager.Singleton.LocalClientId;
            m_ActionRequests[m_ActionRequestCount].RequestedActionID = actionID;
            m_ActionRequests[m_ActionRequestCount].SpawnPosition = position;
            m_ActionRequestCount++;
        }
    }

}
