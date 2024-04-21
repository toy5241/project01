using Unity.Multiplayer.Samples.Utilities;
using Unity.Netcode;
using UnityEngine;

public class CharacterSelect : NetworkBehaviour
{
    [SerializeField]
    private ClientList clientListData;

    // Start is called before the first frame update
    public void CharacterClick(int target)
    {
        //int childrenCount = character.transform.childCount;

        //for (int i = 0; i < childrenCount; i++)
        //{
        //    if (i == target)
        //    {
        //        character.transform.GetChild(i).gameObject.SetActive(true);
        //        continue;
        //    }
        //    character.transform.GetChild(i).gameObject.SetActive(false);
        //}
        ulong clientId = NetworkManager.Singleton.LocalClientId;

        //// NetIdと一致するデータを取得
        //ClientData clientData = clientListData.DataList.Find(data => data.NetId == clientId);
        //GameObject obj = clientData.ClientObj;
        //int childrenCount = obj.transform.childCount;
        //for (int i = 0; i < childrenCount; i++)
        //{
        //    if (i == target)
        //    {
        //        //obj.transform.GetChild(i).gameObject.SetActive(true);
        //        Debug.Log("来てますか");
        //        Debug.Log(obj);
        //        obj.transform.GetChild(i).gameObject.SetActive(true);

        //        continue;
        //    }
        //    obj.transform.GetChild(i).gameObject.SetActive(false);
        //}

        CharacterShowHideRpc(target, clientId);

    }

    [Rpc(SendTo.ClientsAndHost)]
    void CharacterShowHideRpc(int target,ulong clientId)
    {
        // Tagを持つオブジェクトを取得
        GameObject[] CharacterObj = GameObject.FindGameObjectsWithTag("Character");

        foreach (GameObject obj in CharacterObj)
        {
            NetworkObject identity = obj.GetComponent<NetworkObject>();
            ulong obj_id = identity.OwnerClientId;

            if (obj_id == clientId)
            {
                int childrenCount = obj.transform.childCount;
                for (int i = 0; i < childrenCount; i++)
                {
                    if (i == target)
                    {
                        //obj.transform.GetChild(i).gameObject.SetActive(true);
                        obj.transform.GetChild(i).gameObject.SetActive(true);
                        
                        continue;
                    }
                    obj.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
