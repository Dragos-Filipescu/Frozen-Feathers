using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardId : MonoBehaviour
{
    private static int id = 0;

    public int cardId;
    // Start is called before the first frame update
    void Start()
    {
        cardId = id;
        id++;
    }
}
