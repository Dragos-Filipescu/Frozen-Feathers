using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using Mirror;

public class DrawCard : NetworkBehaviour
{
    public PlayerManager playerManager;
    

    public void Start()
    {
        
    }

    public void OnClick()
    {
        if (playerManager == null)
        {
            NetworkIdentity networkIdentity = NetworkClient.connection.identity;
            playerManager = networkIdentity.GetComponent<PlayerManager>();
        }
        
        playerManager.CmdDealCard();
    }
}
