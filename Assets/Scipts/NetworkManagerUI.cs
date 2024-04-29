using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : NetworkBehaviour
{
    public GameObject networkManagerPrefab;
    

    private void Awake()
    {
        if(NetworkManager.Singleton == null)
        {
            Instantiate(networkManagerPrefab);
        }
    }

}
