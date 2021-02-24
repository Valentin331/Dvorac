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
    private CardFunctions cardFunctionsScript;
    private CardMoveAnimator cardMoveAnimatorScript;
    private Dictionary<string, int> scoreDictionary;

    private void Awake()
    {
        dvoracScript = GetComponent<Dvorac>();
        cardFunctionsScript = GetComponent<CardFunctions>();
        cardMoveAnimatorScript = GetComponent<CardMoveAnimator>();
        gameLoopScript = GetComponent<GameLoop>();
        botMessages.Add("Bot razmišlja koju će kartu baciti...");
        botMessages.Add("Bot smišlja neku opaku taktiku..");
        botMessages.Add("Sad je bot na redu...");
        scoreDictionary = new Dictionary<string, int>();
    }

    public IEnumerator BotTurn(float wait, float animationDuration)
    {
        scoreDictionary = new Dictionary<string, int>();

        // Display bot turn message.
        dvoracScript.gameplayMsg.text = botMessages[Random.Range(0, botMessages.Count)];

        //Selecting next cart the bot will play based on chosen difficulty
        int cardIndex;
        int cardCounter;

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
            Debug.Log("POCETAK TURNA -- POCETAK TURNA -- POCETAK TURNA");

            //temp botdeck print
            foreach (GameObject karta in dvoracScript.botDeck)
            {
                Debug.Log(karta);
            }

            foreach (GameObject card in dvoracScript.botDeck)
            {
                if (scoreDictionary.ContainsKey(card.name)) {}
                else
                {
                    switch (card.name)
                    {
                        case "goruciCovjek":
                            scoreDictionary.Add(card.name, 150);
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
            }

            //temp score print
            foreach (KeyValuePair<string, int> kvp in scoreDictionary)
            {
                Debug.Log("karta: " + kvp.Key + "  score: " + kvp.Value);
            }

            //Getting chosen card index based on max score
            var keyOfMaxValue = scoreDictionary.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            cardCounter = 0;
            cardIndex = -1;
            foreach (GameObject card in dvoracScript.botDeck)
            {
                if (card.GetComponent<CardProperties>().cardCode == keyOfMaxValue)
                {
                    cardIndex = cardCounter;
                    break;
                }
                cardCounter += 1;
            }
        }
        Debug.Log("karta je: " + dvoracScript.botDeck[cardIndex]);
        // Select card which will be played.
        
        GameObject selectedCard = dvoracScript.botDeck[cardIndex];

        yield return new WaitForSeconds(wait);

        // Instantiate a card and start it's animation.
        //CardProperties cardPropertiesScript = selectedCard.GetComponent<CardProperties>();
        GameObject cardInstance = Instantiate(selectedCard, botArea.transform.position, Quaternion.identity);
        cardInstance.GetComponent<CardProperties>().FlipCardOn("back");
        cardInstance.GetComponent<CardProperties>().zoomable = false;
        cardInstance.GetComponent<CardProperties>().draggable = false;
        cardInstance.transform.SetParent(playScreen.transform, true);
        cardInstance.transform.localScale = new Vector3(1, 1, 1);

        StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, botArea.transform.position, dropZoneYard.transform.position, animationDuration));

        yield return new WaitForSeconds(animationDuration);

        // Actually move the card to correct list and set it's parent.
        cardInstance.transform.SetParent(dvoracScript.dropZoneYard.transform, true);
        cardInstance.GetComponent<CardProperties>().FlipCardOn("front");
        cardInstance.GetComponent<CardProperties>().zoomable = true;

        dvoracScript.yardDeck.Add(dvoracScript.botDeck[cardIndex]);
        dvoracScript.botDeck.RemoveAt(cardIndex);

        dvoracScript.botCardCount.text = dvoracScript.botDeck.Count().ToString();

        // Clear gameplay message.
        dvoracScript.gameplayMsg.text = "";

        // Call the correct function in regard of what card was played
        cardFunctionsScript.botCardFunctionalities[dvoracScript.yardDeck[dvoracScript.yardDeck.Count - 1].GetComponent<CardProperties>().name].Invoke();
    }

    public IEnumerator BotDiscard(float wait, float animationDuration)
    {
        dvoracScript.gameplayMsg.text = "Bot gleda koju bi kartu odbacio...";
        // TODO:
        // Write algorithm for selecting card to discard.
        int cardIndex = Random.Range(0, dvoracScript.botDeck.Count);
        GameObject selectedCard = dvoracScript.botDeck[cardIndex];

        yield return new WaitForSeconds(wait);

        dvoracScript.yardDeck.Add(dvoracScript.botDeck[cardIndex]);
        dvoracScript.botDeck.RemoveAt(cardIndex);

        GameObject cardInstance = Instantiate(selectedCard, botArea.transform.position, Quaternion.identity);
        cardInstance.GetComponent<CardProperties>().FlipCardOn("back");
        cardInstance.GetComponent<CardProperties>().zoomable = false;
        cardInstance.GetComponent<CardProperties>().draggable = false;
        cardInstance.transform.SetParent(playScreen.transform, true);
        cardInstance.transform.localScale = new Vector3(1, 1, 1);

        StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, botArea.transform.position, dropZoneYard.transform.position, animationDuration));
        dvoracScript.gameplayMsg.text = "";

        yield return new WaitForSeconds(animationDuration);

        cardInstance.transform.SetParent(dvoracScript.dropZoneYard.transform, true);
        cardInstance.GetComponent<CardProperties>().FlipCardOn("front");
        cardInstance.GetComponent<CardProperties>().zoomable = true;

        dvoracScript.botCardCount.text = dvoracScript.botDeck.Count().ToString();
    }
}
