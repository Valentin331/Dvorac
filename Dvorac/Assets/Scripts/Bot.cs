using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    private Dvorac dvoracScript;
    public List<string> botMessages;

    private void Awake()
    {
        dvoracScript = GetComponent<Dvorac>();
        botMessages.Add("Bot razmišlja koju će kartu baciti...");
        botMessages.Add("Bot smišlja neku opaku taktiku..");
        botMessages.Add("Sad je bot na redu...");
    }

    public IEnumerator BotTurn(int wait)
    {
        dvoracScript.playerTurn = false;
        dvoracScript.gameplayMsg.text = botMessages[Random.Range(0, botMessages.Count - 1)];
        yield return new WaitForSeconds(wait);

        int cardIndex = Random.Range(0, dvoracScript.botDeck.Count - 1);

        GameObject selectedCard = dvoracScript.botDeck[cardIndex];
        CardProperties cardPropertiesScript = selectedCard.GetComponent<CardProperties>();
        cardPropertiesScript.FlipCardOn("front");
        cardPropertiesScript.zoomable = true;
        cardPropertiesScript.draggable = false;
        GameObject cardInstance = Instantiate(selectedCard, new Vector2(0, 0), Quaternion.identity);
        cardInstance.transform.SetParent(dvoracScript.dropZoneYard.transform, false);

        dvoracScript.yardDeck.Add(dvoracScript.botDeck[cardIndex]);
        dvoracScript.botDeck.RemoveAt(cardIndex);

        dvoracScript.gameplayMsg.text = "";
        dvoracScript.playerTurn = true;
    }
}
