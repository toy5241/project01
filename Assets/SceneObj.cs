using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneObj : NetworkBehaviour
{
    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        var clientOrServer = sceneEvent.ClientId == NetworkManager.ServerClientId ? "server" : "client";
        switch (sceneEvent.SceneEventType)
        {
            case SceneEventType.LoadComplete:
                {
                    // We want to handle this for only the server-side
                    if (sceneEvent.ClientId == NetworkManager.ServerClientId)
                    {
                        // *** IMPORTANT ***
                        // Keep track of the loaded scene, you need this to unload it
                        //m_LoadedScene = sceneEvent.Scene;
                        Debug.Log("ÉVÅ[ÉìÇ™ì«Ç›çûÇ‹ÇÍÇ‹ÇµÇΩ");
                    }
                    Debug.Log($"Loaded the {sceneEvent.SceneName} scene on " +
                        $"{clientOrServer}-({sceneEvent.ClientId}).");
                    break;
                }
        }
    }

    //var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
    //        if (status != SceneEventProgressStatus.Started)
    //        {
    //            Debug.LogWarning($"Failed to load {m_SceneName} " +
    //                  $"with a {nameof(SceneEventProgressStatus)}: {status}");
    //}
}
