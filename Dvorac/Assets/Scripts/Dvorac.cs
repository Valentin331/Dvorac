using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Asyncoroutine;

public class Dvorac : MonoBehaviour
{
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

    public GameObject playScreen;
    public GameObject dropZoneCastle;
    public GameObject dropZoneYard;
    public GameObject playerArea;
    public GameObject botArea;
    public GameObject cardZoomDisplay;
    public GameObject cardReveal1;
    public GameObject cardReveal2;
    public Text gameplayMsg;
    public Text botCardCount;

    public List<GameObject> castleDeck;
    public List<GameObject> yardDeck;
    public List<GameObject> playerDeck;
    public List<GameObject> botDeck;

    public GameObject playTo;
    public bool playerTurn;
    public string playAction;

    private CardMoveAnimator cardMoveAnimatorScript;

    private void Start()
    {
        cardZoomDisplay.SetActive(false);
        cardReveal1.SetActive(false);
        cardReveal2.SetActive(false);
        cardMoveAnimatorScript = GetComponent<CardMoveAnimator>();
    }

    public IEnumerator StartNewGame()
    {
        playerTurn = false;
        castleDeck = GenerateDeck();
        Shuffle(castleDeck);
        PlayNext("yard");

        GameObject castleCardInstance = Instantiate(castleDeck[0], new Vector2(0, 0), Quaternion.identity);
        castleCardInstance.GetComponent<CardProperties>().zoomable = false;
        castleCardInstance.GetComponent<CardProperties>().zoomable = false;
        castleCardInstance.GetComponent<CardProperties>().FlipCardOn("back");
        castleCardInstance.transform.SetParent(dropZoneCastle.transform, false);

        // Deal cards to players
        for (int i = 0; i < 12; i++)
        {
            GameObject playerCardInstance = Instantiate(castleDeck.Last<GameObject>(), dropZoneCastle.transform.position, Quaternion.identity);
            playerCardInstance.transform.SetParent(playScreen.transform, true);
            playerCardInstance.transform.localScale = new Vector3(1, 1, 1);
            playerCardInstance.GetComponent<CardProperties>().FlipCardOn("front");
            StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(playerCardInstance, dropZoneCastle.transform.position, playerArea.transform.position, 0.1f));
            yield return new WaitForSeconds(0.1f);
            playerCardInstance.transform.SetParent(playerArea.transform, false);
            playerCardInstance.GetComponent<CardProperties>().zoomable = true;
            playerCardInstance.GetComponent<CardProperties>().draggable = true;
            playerDeck.Add(castleDeck.Last<GameObject>());
            castleDeck.RemoveAt(castleDeck.Count - 1);

            GameObject botCardInstance = Instantiate(castleDeck.Last<GameObject>(), dropZoneCastle.transform.position, Quaternion.identity);
            botCardInstance.transform.SetParent(playScreen.transform, true);
            botCardInstance.transform.localScale = new Vector3(1, 1, 1);
            botCardInstance.GetComponent<CardProperties>().FlipCardOn("back");
            StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(botCardInstance, dropZoneCastle.transform.position, botArea.transform.position, 0.1f));
            yield return new WaitForSeconds(0.1f);
            botCardInstance.transform.SetParent(botArea.transform, false);
            Destroy(botCardInstance);
            botDeck.Add(castleDeck.Last<GameObject>());
            castleDeck.RemoveAt(castleDeck.Count - 1);
            botCardCount.text = botDeck.Count().ToString();
        }

        playAction = "play";
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

    public async void FetchCard(string who)
    {
        float duration = .2f;
        if (who == "player")
        {
            // Add last item from castleDeck list to playerDeck list.
            playerDeck.Add(castleDeck.Last<GameObject>());
            castleDeck.RemoveAt(castleDeck.Count - 1);

            // Select last card from player deck, instantiate it and play fetching animation
            GameObject card = playerDeck.Last<GameObject>();
            GameObject cardInstance = Instantiate(card, new Vector2(0, 0), Quaternion.identity);
            cardInstance.transform.SetParent(playScreen.transform, true);
            cardInstance.GetComponent<CardProperties>().FlipCardOn("front");
            cardInstance.GetComponent<CardProperties>().zoomable = false;
            cardInstance.GetComponent<CardProperties>().draggable = false;
            cardInstance.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, dropZoneCastle.transform.position, playerArea.transform.position, duration));

            await new WaitForSeconds(duration);

            // Set card's parent to be playerArea and make it zoomable and draggable
            cardInstance.transform.SetParent(playerArea.transform, false);
            cardInstance.GetComponent<CardProperties>().zoomable = true;
            cardInstance.GetComponent<CardProperties>().draggable = true;
        }
        else if (who == "bot")
        {
            // Add last item from castleDeck list to botDeck list.
            botDeck.Add(castleDeck.Last<GameObject>());
            castleDeck.RemoveAt(castleDeck.Count - 1);

            GameObject card = botDeck.Last<GameObject>();
            GameObject cardInstance = Instantiate(card, new Vector2(0, 0), Quaternion.identity);
            cardInstance.transform.SetParent(playScreen.transform, true);
            cardInstance.GetComponent<CardProperties>().FlipCardOn("back");
            cardInstance.GetComponent<CardProperties>().zoomable = false;
            cardInstance.GetComponent<CardProperties>().draggable = false;
            cardInstance.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, dropZoneCastle.transform.position, botArea.transform.position, duration));

            await new WaitForSeconds(duration);

            botCardCount.text = botDeck.Count.ToString();

            Destroy(cardInstance);
        }
    }

    public void RevealCard(GameObject card, GameObject where)
    {
        where.SetActive(true);
        where.GetComponent<Image>().sprite = card.GetComponent<CardProperties>().cardFront;
    }

    public void CancelRevealCard()
    {
        cardReveal1.SetActive(false);
        cardReveal2.SetActive(false);
    }

    public async void MlineCard()
    {
        float duration = .4f;

        yardDeck.Add(castleDeck.Last<GameObject>());
        castleDeck.RemoveAt(castleDeck.Count - 1);

        GameObject card = yardDeck.Last<GameObject>();
        GameObject cardInstance = Instantiate(card, new Vector2(0, 0), Quaternion.identity);
        cardInstance.transform.SetParent(playScreen.transform, true);
        cardInstance.GetComponent<CardProperties>().FlipCardOn("back");
        cardInstance.GetComponent<CardProperties>().zoomable = false;
        cardInstance.GetComponent<CardProperties>().draggable = false;
        cardInstance.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, dropZoneCastle.transform.position, dropZoneYard.transform.position, duration));

        await new WaitForSeconds(duration);

        // Set card's parent to be playerArea and make it zoomable and draggable
        cardInstance.transform.SetParent(dropZoneYard.transform, true);
        cardInstance.GetComponent<CardProperties>().FlipCardOn("front");
        cardInstance.GetComponent<CardProperties>().zoomable = true;
        cardInstance.GetComponent<CardProperties>().draggable = true;
    }

    public async void CaptureCard(string who)
    {
        float duration = .4f;
        int index = Random.Range(0, botDeck.Count);

        if (who == "player")
        {
            playerDeck.Add(botDeck[index]);
            botDeck.RemoveAt(index);

            GameObject card = playerDeck.Last<GameObject>();
            GameObject cardInstance = Instantiate(card, new Vector2(0, 0), Quaternion.identity);
            cardInstance.transform.SetParent(playScreen.transform, true);
            cardInstance.GetComponent<CardProperties>().FlipCardOn("back");
            cardInstance.GetComponent<CardProperties>().zoomable = false;
            cardInstance.GetComponent<CardProperties>().draggable = false;
            cardInstance.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, botArea.transform.position, playerArea.transform.position, duration));

            await new WaitForSeconds(duration);

            // Set card's parent to be playerArea and make it zoomable and draggable
            cardInstance.transform.SetParent(playerArea.transform, true);
            cardInstance.GetComponent<CardProperties>().FlipCardOn("front");
            cardInstance.GetComponent<CardProperties>().zoomable = true;
            cardInstance.GetComponent<CardProperties>().draggable = true;
        }
        else
        {
            botDeck.Add(playerDeck[index]);
            playerDeck.RemoveAt(index);

            GameObject card = null;

            foreach (Transform child in playerArea.transform)
            {
                if (child.gameObject.GetComponent<CardProperties>().cardCode == botDeck.Last<GameObject>().GetComponent<CardProperties>().cardCode)
                {
                    card = child.gameObject;
                    break;
                }
            }

            GameObject cardInstance = card;
            cardInstance.transform.SetParent(playScreen.transform, true);
            cardInstance.GetComponent<CardProperties>().FlipCardOn("back");
            cardInstance.GetComponent<CardProperties>().zoomable = false;
            cardInstance.GetComponent<CardProperties>().draggable = false;
            cardInstance.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, playerArea.transform.position, botArea.transform.position, duration));

            await new WaitForSeconds(duration);

            // Set card's parent to be playerArea and make it zoomable and draggable
            Destroy(cardInstance);
        }
    }
}