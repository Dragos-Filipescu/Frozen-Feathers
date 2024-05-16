using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class PlayerManager : NetworkBehaviour
{
    enum CardActionType
    {
        Dealt = 0,
        Played = 1,
    }

    private const int maxCards = 7;

    public GameObject[] cardPrefabs;

    private GameObject playerArea;
    private GameObject enemyArea;
    private GameObject dropZone;
    private GameObject drawPile;
    private DropZoneManager dropZoneManager;
    private CardManager playerAreaCardManager;
    private CardManager enemyAreaCardManager;

    private readonly System.Random cardPicker = new();

    //IEnumerator DrawInitialHand()
    //{
    //    for (int i = 0; i < maxCards; i++)
    //    {
    //        GameObject card = Instantiate(
    //            cardPrefabs[cardPicker.Next(0, cardPrefabs.Count())],
    //            drawPile.transform.position,
    //            Quaternion.identity
    //            );
    //        NetworkServer.Spawn(card, connectionToClient);
    //        RpcShowCard(card, CardActionType.Dealt);

    //        yield return new WaitForSeconds(0.5f);
    //    }
    //}

    public override void OnStartClient()
    {
        base.OnStartClient();

        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        dropZone = GameObject.Find("DropZone");
        drawPile = GameObject.Find("DrawPile");

        dropZoneManager = dropZone.GetComponent<DropZoneManager>();
        playerAreaCardManager = playerArea.GetComponent<CardManager>();
        enemyAreaCardManager = enemyArea.GetComponent<CardManager>();
    }

    [Server]
    public override void OnStartServer()
    {
        base.OnStartServer();

        playerArea = GameObject.Find("PlayerArea");
        enemyArea = GameObject.Find("EnemyArea");
        dropZone = GameObject.Find("DropZone");
        drawPile = GameObject.Find("DrawPile");

        dropZoneManager = dropZone.GetComponent<DropZoneManager>();
        playerAreaCardManager = playerArea.GetComponent<CardManager>();
        enemyAreaCardManager = enemyArea.GetComponent<CardManager>();
    }

    [Command]
    public void CmdDealCard()
    {
        GameObject card = Instantiate(
                cardPrefabs[cardPicker.Next(0, cardPrefabs.Count())],
                drawPile.transform.position,
                Quaternion.identity
                );
        NetworkServer.Spawn(card, connectionToClient);
        RpcShowCard(card, CardActionType.Dealt);
    }

    public void PlayCard(GameObject card)
    {
        CmdPlayCard(card);
    }

    [Command]
    void CmdPlayCard(GameObject card)
    {
        RpcShowCard(card, CardActionType.Played);
    }

    [ClientRpc]
    void RpcShowCard(GameObject card, CardActionType type)
    {
        switch (type)
        {
            case CardActionType.Dealt:
                {
                    if (isOwned)
                    {
                        playerAreaCardManager.AppendCard(card);
                    }
                    else
                    {
                        enemyAreaCardManager.AppendCard(card);
                    }
                    break;
                }
            case CardActionType.Played:
                {
                    dropZoneManager.AddCard(card);
                    break;
                }
            default:
                {
                    Debug.LogError("Unsupported Type");
                    break;
                }
        }
    }
}
