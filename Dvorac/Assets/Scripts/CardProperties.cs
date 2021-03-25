using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Asyncoroutine;

public class CardProperties : MonoBehaviour
{
    public Sprite cardFront;
    public Sprite cardBack;
    public string cardCode;
    public string cardSymbol;
    public string cardRarity;
    public GameObject playScreen;
    public GameObject gameManager;
    public GameObject cardZoomDisplay;

    public bool zoomable = false;
    public bool draggable = false;

    public int rarity;

    private GameObject zoomInstance;
    private Vector2 startPosition;
    private bool isBeingDragged = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private Dvorac dvoracScript;
    private GameLoop gameLoopScript;
    private Bot botScript;
    private CardFunctions cardFunctionsScript;

    public void Awake()
    {
        playScreen = GameObject.Find("PlayScreen");
        gameManager = GameObject.Find("GameManager");
        dvoracScript = gameManager.GetComponent<Dvorac>();
        gameLoopScript = gameManager.GetComponent<GameLoop>();
        botScript = gameManager.GetComponent<Bot>();
        cardFunctionsScript = gameManager.GetComponent<CardFunctions>();
    }

    private void Update()
    {
        if (isBeingDragged)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Detect if card is colliding with desired game object
        if (collision.gameObject.name == dvoracScript.playTo.name)
        {
            isOverDropZone = true;
            dropZone = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Detect if card is colliding with desired game object
        if (collision.gameObject.name == dvoracScript.playTo.name)
        {
            isOverDropZone = false;
            dropZone = null;
        }
    }

    public void FlipCardOn(string side)
    {
        // Sets card sprite to be either cardFront of cardBack
        if (side == "front")
        {
            this.GetComponent<Image>().sprite = cardFront;
        }
        else if(side == "back")
        {
            this.GetComponent<Image>().sprite = cardBack;
        }
    }

    public void ZoomCard()
    {
        // If the card is zoomable, attach the right sprite to CardZoomDisplay game object and set it to active
        if (zoomable)
        {
            dvoracScript.cardZoomDisplay.SetActive(true);
            dvoracScript.cardZoomDisplay.GetComponent<Image>().sprite = cardFront;
        }
    }

    public void DestroyZoomCard()
    {
        // Set CardZoomDisplay game object to inactive
        dvoracScript.cardZoomDisplay.SetActive(false);
    }

    public void BeginDrag()
    {
        if(draggable)
        {
            startPosition = transform.position;
            transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
            StartCoroutine(dvoracScript.playTo.GetComponent<ColorChanger>().ChangeDZColor("on"));
            isBeingDragged = true;
        }
    }

    public async void EndDrag()
    {
        if (draggable)
        {
            transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(dvoracScript.playTo.GetComponent<ColorChanger>().ChangeDZColor("off"));
            isBeingDragged = false;
            // If the card is dropped over valid drop zone than...
            if (isOverDropZone && dvoracScript.playerTurn)
            {
                // ...set it's parent to be that drop zone.
                transform.SetParent(dropZone.transform, false);
                transform.position = dropZone.transform.position;
                int cardIndex = 0;
                // Loop through each card in player's hand
                foreach (GameObject card in dvoracScript.playerDeck)
                {
                    // If it's the right card, remove it from player's hand and add it to belonging drop zone deck
                    if (cardCode == card.GetComponent<CardProperties>().cardCode)
                    {
                        if (dvoracScript.playTo.name == "DropZoneYard")
                        { // Cards are being played
                            dvoracScript.yardDeck.Add(dvoracScript.playerDeck[cardIndex]);

                            // Remove card from playerDeck list
                            dvoracScript.playerDeck.RemoveAt(cardIndex);
                            
                            // Determine under which condition card was played and act accordingly
                            if (dvoracScript.playAction == "play")
                            {
                                cardFunctionsScript.playerCardFunctionalities[cardCode].Invoke();
                            }
                            else if (dvoracScript.playAction == "discardOC")
                            {
                                // If player has no cards left; end game
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }
                                dvoracScript.playAction = "play";
                                // Disable player input
                                dvoracScript.playerTurn = false;
                                // Start bot's turn
                                StartCoroutine(botScript.BotTurn(2.6f, 0.4f));
                            }
                            else if (dvoracScript.playAction == "discardSKP1")
                            {
                                // If player has no cards left; end game
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }
                                dvoracScript.playAction = "discardSKP2";
                                // Disable player input
                                dvoracScript.playerTurn = false;
                                StartCoroutine(botScript.BotDiscard(1.1f, 0.4f, "castle"));
                                await new WaitForSeconds(1.5f);
                                // If bot has no cards left; end game
                                if (dvoracScript.botDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("victory");
                                    return;
                                }
                                dvoracScript.gameplayMsg.text = "Odbaci kartu.";
                                dvoracScript.playerTurn = true;
                            }
                            else if (dvoracScript.playAction == "discardSKP2")
                            {
                                // If player has no cards left; end game
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }
                                dvoracScript.playAction = "play";
                                dvoracScript.playerTurn = false;
                                StartCoroutine(botScript.BotDiscard(1.1f, 0.4f, "castle"));
                                await new WaitForSeconds(1.5f);
                                // If player has no cards left; end game
                                if (dvoracScript.botDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("victory");
                                    return;
                                }
                                StartCoroutine(botScript.BotTurn(2.6f, .4f));
                            }
                            else if (dvoracScript.playAction == "discardSKB1")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                dvoracScript.playerTurn = false;

                                StartCoroutine(botScript.BotDiscard(1.1f, .4f, "castle"));
                                await new WaitForSeconds(1.5f);

                                if (dvoracScript.botDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("victory");
                                    return;
                                }

                                dvoracScript.gameplayMsg.text = "Odbaci kartu.";
                                dvoracScript.playAction = "discardSKB2";
                                dvoracScript.playerTurn = true;
                            }
                            else if (dvoracScript.playAction == "discardSKB2")
                            {
                                if (dvoracScript.botDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("victory");
                                    return;
                                }

                                dvoracScript.playAction = "play";
                                dvoracScript.gameplayMsg.text = "";
                                dvoracScript.playerTurn = true;
                            }
                            else if (dvoracScript.playAction == "discardDLP1")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }
                                dvoracScript.playAction = "discardDLP2";
                                dvoracScript.gameplayMsg.text = "Odbaci drugu kartu";
                            }
                            else if (dvoracScript.playAction == "discardDLP2")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }
                                dvoracScript.playAction = "play";

                                if (dvoracScript.yardDeck[dvoracScript.yardDeck.Count - 1].GetComponent<CardProperties>().cardRarity == dvoracScript.yardDeck[dvoracScript.yardDeck.Count - 2].GetComponent<CardProperties>().cardRarity)
                                {
                                    StartCoroutine(botScript.BotDiscard(1.1f, .4f, "yard"));
                                    await new WaitForSeconds(1.5f);
                                    if (dvoracScript.botDeck.Count == 0)
                                    {
                                        gameLoopScript.EndGame("victory");
                                        return;
                                    }
                                    StartCoroutine(botScript.BotTurn(2.6f, .4f));
                                }
                                else
                                {
                                    StartCoroutine(botScript.BotTurn(2.6f, .4f));
                                }
                            }
                            else if (dvoracScript.playAction == "discardDLB")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }
                                dvoracScript.playAction = "play";
                                dvoracScript.gameplayMsg.text = "";
                            }
                            else if (dvoracScript.playAction == "discardPB1")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                dvoracScript.playAction = "discardPB2";
                                dvoracScript.gameplayMsg.text = "Odbaci drugu kartu.";
                            }
                            else if (dvoracScript.playAction == "discardPB2")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                dvoracScript.playAction = "play";
                                dvoracScript.gameplayMsg.text = "";

                                dvoracScript.playerTurn = true;
                            }
                            else if (dvoracScript.playAction == "discardPP1")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                dvoracScript.gameplayMsg.text = "Odbaci drugu kartu.";
                                dvoracScript.playAction = "discardPP2";
                            }
                            else if (dvoracScript.playAction == "discardPP2")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                dvoracScript.playAction = "play";

                                StartCoroutine(botScript.BotTurn(2.6f, .4f));
                            }
                        }
                        else
                        { // Cards are being roofed
                            dvoracScript.castleDeck.Add(dvoracScript.playerDeck[cardIndex]);

                            // Remove card from playerDeck list
                            dvoracScript.playerDeck.RemoveAt(cardIndex);

                            if (dvoracScript.playAction == "discardSKP1")
                            { // srebrnaKula first player card
                                // If player has no cards left; end game
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                zoomable = false;
                                FlipCardOn("back");

                                dvoracScript.playAction = "discardSKP2";
                                // Disable player input
                                dvoracScript.playerTurn = false;
                                StartCoroutine(botScript.BotDiscard(1.1f, 0.4f, "castle"));
                                await new WaitForSeconds(1.5f);
                                // If bot has no cards left; end game
                                if (dvoracScript.botDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("victory");
                                    return;
                                }
                                dvoracScript.gameplayMsg.text = "Krovaj kartu.";
                                dvoracScript.playerTurn = true;
                            }
                            else if (dvoracScript.playAction == "discardSKP2")
                            { // srebrnaKula second player card
                                // If player has no cards left; end game
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                zoomable = false;
                                FlipCardOn("back");

                                dvoracScript.playAction = "play";
                                dvoracScript.playerTurn = false;
                                StartCoroutine(botScript.BotDiscard(1.1f, 0.4f, "castle"));
                                await new WaitForSeconds(1.5f);
                                // If player has no cards left; end game
                                if (dvoracScript.botDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("victory");
                                    return;
                                }

                                dvoracScript.PlayNext("yard");

                                StartCoroutine(botScript.BotTurn(2.6f, .4f));
                            }
                            else if (dvoracScript.playAction == "discardSKB1")
                            { // srebrnaKula after first bot card
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                zoomable = false;
                                FlipCardOn("back");

                                dvoracScript.playerTurn = false;

                                StartCoroutine(botScript.BotDiscard(1.1f, .4f, "castle"));
                                await new WaitForSeconds(1.5f);

                                if (dvoracScript.botDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("victory");
                                    return;
                                }

                                dvoracScript.gameplayMsg.text = "Krovaj kartu.";
                                dvoracScript.playAction = "discardSKB2";
                                dvoracScript.playerTurn = true;
                            }
                            else if (dvoracScript.playAction == "discardSKB2")
                            { // srebrnaKula after second bot card
                                if (dvoracScript.botDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("victory");
                                    return;
                                }

                                zoomable = false;
                                FlipCardOn("back");

                                dvoracScript.PlayNext("yard");
                                dvoracScript.playAction = "play";
                                dvoracScript.gameplayMsg.text = "";
                                dvoracScript.playerTurn = true;
                            }
                            else if (dvoracScript.playAction == "discardKP")
                            {
                                if (dvoracScript.playerDeck.Count == 0)
                                {
                                    gameLoopScript.EndGame("defeat");
                                    return;
                                }

                                zoomable = false;
                                FlipCardOn("back");

                                dvoracScript.playAction = "play";
                                dvoracScript.playerTurn = false;

                                dvoracScript.PlayNext("yard");

                                StartCoroutine(botScript.BotTurn(2.6f, .4f));
                            }
                        }
                        draggable = false;
                        break;
                    }
                    cardIndex++;
                }
            }
            else
            {
                transform.position = startPosition;
            }
        }
    }
}
