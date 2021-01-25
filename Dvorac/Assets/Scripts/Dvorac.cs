using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Dvorac : MonoBehaviour
{
    // TODO: create all 24 card prefabs in the editor and add them here as game objects
    public GameObject levijatan;
    public GameObject goruciCovjek;
    public GameObject srebrnaKula;

    public GameObject dropZoneCastle;
    public GameObject dropZoneYard;
    public GameObject playerArea;

    public List<GameObject> castleDeck;
    public List<GameObject> yardDeck;
    public List<GameObject> playerDeck;
    public List<GameObject> botDeck;

    public GameObject playTo;

    public void StartNewGame()
    {
        castleDeck = GenerateDeck();
        Shuffle(castleDeck);

        // Deal cards to players
        for (int i = 0; i < 5; i++)
        {
            playerDeck.Add(castleDeck.Last<GameObject>());
            castleDeck.RemoveAt(castleDeck.Count - 1);
            botDeck.Add(castleDeck.Last<GameObject>());
            castleDeck.RemoveAt(castleDeck.Count - 1);
        }

        // Instantiate cards that should be visible
        foreach (GameObject card in castleDeck)
        {
            card.GetComponent<CardProperties>().FlipCardOn("back");
            card.GetComponent<CardProperties>().zoomable = false;
            card.GetComponent<CardProperties>().draggable = false;
            GameObject cardInstance = Instantiate(card, new Vector2(0, 0), Quaternion.identity);
            cardInstance.transform.SetParent(dropZoneCastle.transform, false);
        }
        foreach (GameObject card in playerDeck)
        {
            card.GetComponent<CardProperties>().FlipCardOn("front");
            card.GetComponent<CardProperties>().zoomable = true;
            card.GetComponent<CardProperties>().draggable = true;
            GameObject cardInstance = Instantiate(card, new Vector2(0, 0), Quaternion.identity);
            cardInstance.transform.SetParent(playerArea.transform, false);
        }

        // Define that next card must be played to dropZoneYard
        PlayNext("yard");
    }

    public void ClearBoard()
    {
        // Clear lists containing game objects
        castleDeck.Clear();
        yardDeck.Clear();
        playerDeck.Clear();
        botDeck.Clear();
        // Destroy all instantiations of card prefabs
        foreach (Transform child in dropZoneYard.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in dropZoneCastle.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in playerArea.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private List<GameObject> GenerateDeck()
    {
        List<GameObject> newDeck = new List<GameObject>();

        for (int i = 0; i < 16; i++)
        {
            // TODO: add all lvl 1 rarity cards to newDeck
            newDeck.Add(goruciCovjek);
        }
        for (int i = 0; i < 8; i++)
        {
            // TODO: add all lvl 2 rarity cards to newDeck
            newDeck.Add(srebrnaKula);
        }
        for (int i = 0; i < 4; i++)
        {
            // TODO: add all lvl 3 rarity cards to newDeck
        }
        for (int i = 0; i < 2; i++)
        {
            // TODO: add all lvl 4 rarity cards to newDeck
        }
        // TODO: add all lvl 5 rarity cards to newDeck
        newDeck.Add(levijatan);

        return newDeck;
    }

    public void Shuffle<T>(List<T> deck)
    {
        // TODO: write better shuffle algorithm
        System.Random random = new System.Random();
        int n = deck.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = deck[k];
            deck[k] = deck[n];
            deck[n] = temp;
        }
    }

    public void PlayNext(string dropZone)
    {
        if (dropZone == "yard")
        {
            playTo = dropZoneYard;
        }
        else if (dropZone == "castle")
        {
            playTo = dropZoneCastle;
        }
    }
}
