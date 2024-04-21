using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class ClientData
{
    public ulong NetId;
    public GameObject ClientObj ;
}

[CreateAssetMenu(menuName = "ScriptableObject/ClietnData", fileName = "ClientList")]
public class ClientList : ScriptableObject
{
    public List<ClientData> DataList;
}