using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardProperties : MonoBehaviour
{
    public Sprite cardFront;
    public Sprite cardBack;
    public string cardCode;
    public GameObject playScreen;
    public GameObject gameManager;

    public bool zoomable = false;
    public bool draggable = false;

    private GameObject zoomInstance;
    private Vector2 startPosition;
    private bool isBeingDragged = false;
    private bool isOverDropZone = false;
    private GameObject dropZone;
    private Dvorac dvoracScript;
    private GameLoop gameLoopScript;

    public void Awake()
    {
        playScreen = GameObject.Find("PlayScreen");
        gameManager = GameObject.Find("GameManager");
        dvoracScript = gameManager.GetComponent<Dvorac>();
        gameLoopScript = gameManager.GetComponent<GameLoop>();
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

    public void PlaySound()
    {
        GetComponent<AudioSource>().Play();
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
        // If the card is zoomable, create magnified instance of it
        if (zoomable)
        {
            //zoomInstance = Instantiate(gameObject, new Vector2(310, 660), Quaternion.identity);
            zoomInstance = Instantiate(gameObject, new Vector2(310, 660), Quaternion.identity);
            zoomInstance.transform.SetParent(playScreen.transform, true);
            zoomInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(572, 800);
        }
    }

    public void DestroyZoomCard()
    {
        // Destroy magnified instance of this card
        Destroy(zoomInstance);
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

    public void EndDrag()
    {
        if (draggable)
        {
            transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(dvoracScript.playTo.GetComponent<ColorChanger>().ChangeDZColor("off"));
            isBeingDragged = false;
            // If the card is dropped over valid drop zone than...
            if (isOverDropZone)
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
                        {
                            dvoracScript.yardDeck.Add(dvoracScript.playerDeck[cardIndex]);
                        }
                        else
                        {
                            dvoracScript.castleDeck.Add(dvoracScript.playerDeck[cardIndex]);
                            zoomable = false;
                            FlipCardOn("back");
                        }
                        dvoracScript.playerDeck.RemoveAt(cardIndex);
                        draggable = false;
                        PlaySound();
                        break;
                    }
                    cardIndex++;
                }
                // If player has no cards left, end the game
                if (dvoracScript.playerDeck.Count == 0)
                {
                    gameLoopScript.EndGame("defeat");
                }
            }
            else
            {
                transform.position = startPosition;
            }
        }
    }
}
