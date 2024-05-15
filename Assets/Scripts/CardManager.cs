using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private const int maxCards = 7;
    private const int cardSpacing = 20;
    private readonly Vector2 cardSize = new(120f, 177f);

    public GameObject[] cardPrefabs;

    private Transform drawPile;

    public List<GameObject> cards;

    /// <summary>
    /// Adds a card at the end of the hand.
    /// </summary>
    /// <param name="card"></param>
    public void AppendCard(GameObject card)
    {
        if (cards.Count >= maxCards)
        {
            Debug.LogWarning("Card buffer full");
            return;
        }

        AddCard(cards.Count, card);
    }

    /// <summary>
    /// Add a card based on the position of the mouse.
    /// </summary>
    /// <param name="card"></param>
    /// <param name="mousePos"></param>
    public void AddCard(GameObject card, Vector2 mousePos)
    {
        int rightPos = -1;

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].transform.position.x > mousePos.x)
            {
                rightPos = i;
                break;
            }
        }

        if (rightPos == -1)
        {
            rightPos = cards.Count;
        }

        Debug.Log("Inseting card between " + (rightPos - 1) + " and " + rightPos + "...");

        AddCard(rightPos, card);
    }

    /// <summary>
    /// Add a card before a specified position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="card"></param>
    private void AddCard(int position, GameObject card)
    {
        if (cards.Count >= maxCards)
        {
            Debug.LogWarning("Hand full.");
            return;
        }

        if (cards.Count == 0)
        {
            Vector3 newPos = new(transform.position.x, transform.position.y, transform.position.z);
            iTween.MoveTo(card, newPos, 0.5f);
            card.transform.SetParent(transform);
            cards.Add(card);
            return;
        }

        Vector3 expectedPos;

        if (position == cards.Count)
        {
            expectedPos = new(
                cards[cards.Count - 1].transform.position.x + (cardSize.x / 2f + cardSpacing / 2f),
                transform.position.y,
                transform.position.z
                );
        }
        else
        {
            expectedPos = new(
                cards[position].transform.position.x - (cardSize.x / 2f + cardSpacing / 2f),
                transform.position.y,
                transform.position.z
                );
        }

        for (int i = position - 1; i >= 0; i--)
        {
            Vector3 newPos = new(
                cards[i].transform.position.x - (cardSize.x / 2f + cardSpacing / 2f),
                cards[i].transform.position.y,
                cards[i].transform.position.z
                );
            iTween.MoveTo(cards[i], newPos, 0.1f);
        }

        for (int i = position; i < cards.Count; i++)
        {
            Vector3 newPos = new(
                cards[i].transform.position.x + (cardSize.x / 2f + cardSpacing / 2f),
                cards[i].transform.position.y,
                cards[i].transform.position.z
                );
            iTween.MoveTo(cards[i], newPos, 0.1f);
        }

        iTween.MoveTo(card, expectedPos, 0.2f);
        card.transform.SetParent(transform);
        cards.Insert(position, card);
    }

    /// <summary>
    /// Remove the card closest to the position of the mouse.
    /// </summary>
    /// <param name="mousePos"></param>
    public void RemoveCard(Vector2 mousePos)
    {
        float minDistance = float.MaxValue;
        int idx = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            float distance = Vector2.Distance(
                new Vector2(
                    cards[i].transform.position.x,
                    cards[i].transform.position.y
                ),
                mousePos
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                idx = i;
            }
        }

        RemoveCard(idx);
    }

    /// <summary>
    /// Remove the card at the specified position.
    /// </summary>
    /// <param name="position"></param>
    private void RemoveCard(int position)
    {
        if (cards.Count == 0)
        {
            Debug.LogWarning("Card buffer empty");
            return;
        }

        cards.RemoveAt(position);

        for (int i = position - 1; i >= 0; i--)
        {
            Vector3 newPos = new(
                cards[i].transform.position.x + (cardSize.x / 2f + cardSpacing / 2f),
                cards[i].transform.position.y,
                cards[i].transform.position.z
                );

            iTween.MoveTo(cards[i], newPos, 0.1f);
        }

        for (int i = position; i < cards.Count; i++)
        {
            Vector3 newPos = new(
                cards[i].transform.position.x - (cardSize.x / 2f + cardSpacing / 2f),
                cards[i].transform.position.y,
                cards[i].transform.position.z
                );

            iTween.MoveTo(cards[i], newPos, 0.1f);
        }
    }

    private void Start()
    {
        cards = new(maxCards);
        drawPile = GameObject.Find("DrawPile").transform;
    }

    /// <summary>
    /// Draw a card from the draw pile.
    /// </summary>
    public void DrawCard(GameObject card)
    {
        AppendCard(card);
    }
}
