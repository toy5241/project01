using UnityEngine;

/// <summary>
/// A runtime list of <see cref="Player"/> objects that is populated both on clients and server.
/// </summary>
[CreateAssetMenu]
public class PersistentPlayerRuntimeCollection : RuntimeCollection<Player>
{
    public bool TryGetPlayer(ulong clientID, out Player persistentPlayer)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Debug.Log(Items[i]);
            if (clientID == Items[i].OwnerClientId)
            {
                persistentPlayer = Items[i];
                return true;
            }
        }

        persistentPlayer = null;
        return false;
    }


}