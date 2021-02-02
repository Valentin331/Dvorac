using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public GameObject playScreen;
    public GameObject botArea;
    public GameObject dropZoneYard;

    public List<string> botMessages;
    private Dvorac dvoracScript;
    private CardMoveAnimator cardMoveAnimatorScript;

    private void Awake()
    {
        dvoracScript = GetComponent<Dvorac>();
        cardMoveAnimatorScript = GetComponent<CardMoveAnimator>();
        botMessages.Add("Bot razmišlja koju će kartu baciti...");
        botMessages.Add("Bot smišlja neku opaku taktiku..");
        botMessages.Add("Sad je bot na redu...");
    }

    public IEnumerator BotTurn(float wait, float animationDuration)
    {
        // Disable player input and display bot turn message.
        dvoracScript.playerTurn = false;
        dvoracScript.gameplayMsg.text = botMessages[Random.Range(0, botMessages.Count)];

        // Select card which will be played.
        int cardIndex = Random.Range(0, dvoracScript.botDeck.Count);
        GameObject selectedCard = dvoracScript.botDeck[cardIndex];

        yield return new WaitForSeconds(wait);

        // Instantiate a card and start it's animation.
        CardProperties cardPropertiesScript = selectedCard.GetComponent<CardProperties>();
        cardPropertiesScript.FlipCardOn("back");
        cardPropertiesScript.zoomable = false;
        cardPropertiesScript.draggable = false;
        GameObject cardInstance = Instantiate(selectedCard, botArea.transform.position, Quaternion.identity);
        cardInstance.transform.SetParent(playScreen.transform, true);
        cardInstance.transform.localScale = new Vector3(1, 1, 1);

        StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, botArea.transform.position, dropZoneYard.transform.position, animationDuration));

        yield return new WaitForSeconds(animationDuration);

        // If played card is goruciCovjek: fetch card.
        if (selectedCard.GetComponent<CardProperties>().cardCode == "goruciCovjek")
        {
            dvoracScript.FetchCard("bot");
        }

        // Actually move the card to correct list and set it's parent.
        cardInstance.transform.SetParent(dvoracScript.dropZoneYard.transform, true);
        cardInstance.GetComponent<CardProperties>().FlipCardOn("front");
        cardInstance.GetComponent<CardProperties>().zoomable = true;

        dvoracScript.yardDeck.Add(dvoracScript.botDeck[cardIndex]);
        dvoracScript.botDeck.RemoveAt(cardIndex);

        // Clear gameplay message and enable player input.
        dvoracScript.gameplayMsg.text = "";
        dvoracScript.playerTurn = true;
    }
}
