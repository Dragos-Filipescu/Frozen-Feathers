using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneManager : MonoBehaviour
{
    public int numCards = 0;
    private const float cardSpacing = 20f;
    private readonly Vector2 cardSize = new(120f, 177f);

    public void UpdateCardPositions()
    {
        if (numCards == 0)
            return;

        float totalWidth = numCards * (cardSize.x + cardSpacing) - cardSpacing;
        float startX = transform.position.x - totalWidth / 2f + cardSize.x / 2f;

        for (int i = 0; i < numCards; i++)
        {
            Transform card = transform.GetChild(i);
            float xPos = startX + i * (cardSize.x + cardSpacing);
            Vector3 newPos = new(xPos, transform.position.y, transform.position.z);
            iTween.MoveTo(card.gameObject, newPos, 0.2f);
        }
    }

    public void AddCard(GameObject newCard)
    {
        numCards++;

        iTween.MoveTo(newCard, transform.position, 0.2f);
        newCard.transform.SetParent(transform);
        UpdateCardPositions();
    }

    // Method to remove a card from the hand
    public void RemoveCard()
    {
        // cardObject.transform.SetParent(null);
        numCards--;
        UpdateCardPositions();
    }
}
