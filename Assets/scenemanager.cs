using Unity.Netcode;
using UnityEngine;

public class Scenemanager : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
            Debug.Log("scenemanagerÉçÅ[Éh");
    }
}
