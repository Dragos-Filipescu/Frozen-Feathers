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

    public GameObject playerArea;
    public GameObject enemyArea;
    public GameObject dropZone;
    public GameObject drawPile;
    private CardManager playerAreaCardManager;
    private CardManager enemyAreaCardManager;
    private DrawPile drawPileManager;
    private DrawPile dP;
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
        drawPileManager = drawPile.GetComponent<DrawPile>();

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
        drawPileManager = drawPile.GetComponent<DrawPile>();
        playerAreaCardManager = playerArea.GetComponent<CardManager>();
        enemyAreaCardManager = enemyArea.GetComponent<CardManager>();
    }

    [Command]
    public void CmdDealCard()
    {
        //GameObject card = Instantiate(
        //        cardPrefabs[cardPicker.Next(0, cardPrefabs.Count())],
        //        drawPile.transform.position,
        //        Quaternion.identity
        //        );
        GameObject card;
        if (drawPileManager.GetDeck().Count > 0)
        {
             card = drawPileManager.ServeCard();
            NetworkServer.Spawn(card, connectionToClient);
            RpcShowCard(card, CardActionType.Dealt);
        }
        else
        {
            //drawPileManager.InitCards();
            Debug.LogWarning("Draw pile empty");
           // card = drawPileManager.ServeCard();
        }
        
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
                        playerAreaCardManager.DrawCard(card);
                    }
                    else
                    {
                        enemyAreaCardManager.DrawCard(card);
                    }
                    break;
                }
            case CardActionType.Played:
                {
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
