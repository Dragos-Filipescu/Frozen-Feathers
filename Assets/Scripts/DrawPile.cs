using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    private const int maxPileSize = 10;
    public GameObject[] cardPrefabs;
    private Stack<GameObject> drawPile = new Stack<GameObject>(maxPileSize);

    // Start is called before the first frame update
    void Start()
    {
    
        for (int i = 0; i < maxPileSize; i++)
        {
            drawPile.Push(
                Instantiate(
                    cardPrefabs[Random.Range(0, cardPrefabs.Length)],
                    transform.position,
                    Quaternion.identity
                    )
                );
        }
    }

    public GameObject ServeCard()
    {
        GameObject result;

        var popped = drawPile.TryPop(out result);
        if (!popped)
        {
            Debug.LogWarning("Draw pile empty");
        }

        return result;
    }
    public Stack<GameObject> GetDeck()
    {

        return drawPile;
    }
    public void InitCards()
    {
        Debug.LogWarning("Draw pile empty");
        for (int i = 0; i < maxPileSize; i++)
        {
            drawPile.Push(
                Instantiate(
                    cardPrefabs[Random.Range(0, cardPrefabs.Length)],
                    transform.position,
                    Quaternion.identity
                    )
                );
        }
    }
    public void AddCard(GameObject card)
    {

    }
}
