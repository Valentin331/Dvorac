using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Dvorac : MonoBehaviour
{
    // TODO: create all 24 card prefabs in the editor and add them here as game objects
    public GameObject goruciCovjek;
    public GameObject objeseniCovjek;
    public GameObject zlatnaKula;
    public GameObject srebrnaKula;
    public GameObject vitez;
    public GameObject dvorskaLuda;
    public GameObject svetac;
    public GameObject carobnjak;
    public GameObject kraljica;
    public GameObject vjestica;
    public GameObject kralj;
    public GameObject vrag;
    public GameObject vodoriga;
    public GameObject patuljak;
    public GameObject koloSrece;
    public GameObject lovac;
    public GameObject div;
    public GameObject kocija;
    public GameObject nocnaMora;
    public GameObject glasnik;
    public GameObject osuda;
    public GameObject jednorog;
    public GameObject behemot;
    public GameObject levijatan;

    public GameObject dropZoneCastle;
    public GameObject dropZoneYard;
    public GameObject playerArea;
    public GameObject cardZoomDisplay;
    public Text gameplayMsg;

    public List<GameObject> castleDeck;
    public List<GameObject> yardDeck;
    public List<GameObject> playerDeck;
    public List<GameObject> botDeck;

    public GameObject playTo;
    public bool playerTurn;

    private void Start()
    {
        cardZoomDisplay.SetActive(false);
    }

    public void StartNewGame()
    {
        castleDeck = GenerateDeck();
        Shuffle(castleDeck);

        // Deal cards to players
        for (int i = 0; i < 12; i++)
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
        playerTurn = true;
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
            newDeck.Add(goruciCovjek);
            newDeck.Add(objeseniCovjek);
            newDeck.Add(vodoriga);
        }
        for (int i = 0; i < 8; i++)
        {
            newDeck.Add(srebrnaKula);
            newDeck.Add(zlatnaKula);
            newDeck.Add(patuljak);
        }
        for (int i = 0; i < 4; i++)
        {
            newDeck.Add(vitez);
            newDeck.Add(dvorskaLuda);
            newDeck.Add(koloSrece);
            newDeck.Add(lovac);
        }
        for (int i = 0; i < 2; i++)
        {
            newDeck.Add(svetac);
            newDeck.Add(carobnjak);
            newDeck.Add(div);
            newDeck.Add(kocija);
            newDeck.Add(nocnaMora);
            newDeck.Add(glasnik);
        }
        newDeck.Add(kraljica);
        newDeck.Add(vjestica);
        newDeck.Add(kralj);
        newDeck.Add(vrag);
        newDeck.Add(osuda);
        newDeck.Add(jednorog);
        newDeck.Add(behemot);
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
