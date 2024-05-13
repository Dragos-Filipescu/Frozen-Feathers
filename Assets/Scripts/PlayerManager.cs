using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    public GameObject[] cardPrefabs;

    public GameObject playerArea;
    public GameObject enemyArea;
    public GameObject dropZone;
    private CardManager cardManager;

    List<GameObject> cards = new List<GameObject>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        dropZone = GameObject.Find("DropZone");
        cardManager = playerArea.GetComponent<CardManager>();
    }

    [Server]
    public override void OnStartServer()
    {
        cards.AddRange(cardPrefabs);
        Debug.Log(cards);
    }

    [Command]
    public void CmdDealCard()
    {
        cardManager.DrawCard();
    }

    [ClientRpc]
    void RpcShowCard()
    {

    }
}
