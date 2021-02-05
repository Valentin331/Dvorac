using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public GameObject playScreen;
    public GameObject botArea;
    public GameObject dropZoneYard;

    public List<string> botMessages;
    private Dvorac dvoracScript;
    private GameLoop gameLoopScript;
    private CardMoveAnimator cardMoveAnimatorScript;
    private Dictionary<string, int> scoreDictionary;
    private CardMoveAnimator cardMoveAnimatorScript;

    private void Awake()
    {
        dvoracScript = GetComponent<Dvorac>();
        cardMoveAnimatorScript = GetComponent<CardMoveAnimator>();
        gameLoopScript = GetComponent<GameLoop>();

        botMessages.Add("Bot razmišlja koju će kartu baciti...");
        botMessages.Add("Bot smišlja neku opaku taktiku..");
        botMessages.Add("Sad je bot na redu...");
    }

    public IEnumerator BotTurn(float wait, float animationDuration)
    {
        // Disable player input and display bot turn message.
        dvoracScript.playerTurn = false;
        dvoracScript.gameplayMsg.text = botMessages[Random.Range(0, botMessages.Count)];

        //Selecting next cart the bot will play based on chosen difficulty
        int cardIndex;
        GameObject objectOfMaxValue;
        if (gameLoopScript.botDifficulty == 1)
        {
            //najgora karta (suprotno od hard)
            cardIndex = 0;
        }
        else if (gameLoopScript.botDifficulty == 2)
        {
            //Random card selected
            cardIndex = Random.Range(0, dvoracScript.botDeck.Count);
        }
        else
        {
            foreach (GameObject card in dvoracScript.botDeck)
            {
                Debug.Log(card);
                switch (card.name)
                {
                    case "goruciCovjek":
                        //scoreDictionary.Add(card.name, 150);
                        break;
                    case "objeseniCovjek":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "zlatnaKula":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "srebrnaKula":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "vitez":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "dvorskaLuda":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "svetac":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "carobnjak":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "kraljica":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "vjestica":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "kralj":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "vrag":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "vodoriga":
                        scoreDictionary.Add(card.name, 150);
                        break;
                    case "patuljak":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "koloSrece":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "lovac":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "div":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "kocija":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "nocnaMora":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "glasnik":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "osuda":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "jednorog":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "behemot":
                        scoreDictionary.Add(card.name, 100);
                        break;
                    case "levijatan":
                        scoreDictionary.Add(card.name, 100);
                        break;
                }

            }
            var keyOfMaxValue = scoreDictionary.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            objectOfMaxValue = GameObject.Find(keyOfMaxValue);
            cardIndex = dvoracScript.botDeck.IndexOf(objectOfMaxValue);
        }

        // Select card which will be played.
        
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
