using System;
using Unity.Netcode;
using UnityEngine;
public struct ActionRequestData : INetworkSerializable
{
    public ulong clientId;
    public int RequestedActionID;
    public Vector3 SpawnPosition;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref RequestedActionID);
        serializer.SerializeValue(ref SpawnPosition);
    }
}
