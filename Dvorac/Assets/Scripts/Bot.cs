using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public GameObject playScreen;
    public GameObject botArea;
    public GameObject dropZoneYard;
    public GameObject dropZoneCastle;
    public int botInfoPresumedBadCardsInCastle, nocnaMoraCounter;

    public List<string> botMessages1, botMessages2, botMessages3;
    private Dvorac dvoracScript;
    private GameLoop gameLoopScript;
    private CardFunctions cardFunctionsScript;
    private CardMoveAnimator cardMoveAnimatorScript;
    private Dictionary<string, int> scoreDictionary;
    private int score, extra, oneStarCounter, twoStarCounter, threeStarCounter, fourStarCounter, fiveStarCounter, sunCounter, moonCounter, starCounter, vodorigaCounter, svetacCarobnjakInBotDeck, levijatanInEitherDeck;
    private List<string> oneStarCards, twoStarCards, threeStarCards, fourStarCards, fiveStarCards, sunSymbolCards, moonSymbolCards, starSymbolCards;

    private void Awake()
    {
        dvoracScript = GetComponent<Dvorac>();
        cardFunctionsScript = GetComponent<CardFunctions>();
        cardMoveAnimatorScript = GetComponent<CardMoveAnimator>();
        gameLoopScript = GetComponent<GameLoop>();
        botMessages1.Add("Woof woof... (Agatodemon razmišlja koju će kartu baciti...)");
        botMessages1.Add("Woof woof... (Agatodemon smišlja neku opaku taktiku..");
        botMessages1.Add("Sad je Agatodemon na redu...");
        botMessages2.Add("Hermes al-Yazīd razmišlja koju će kartu baciti...");
        botMessages2.Add("Hermes al-Yazīd smišlja neku opaku taktiku..");
        botMessages2.Add("Sad je Hermes al-Yazīd na redu...");
        botMessages3.Add("Sfingurina razmišlja koju će kartu baciti...");
        botMessages3.Add("Sfingurina smišlja neku opaku taktiku..");
        botMessages3.Add("Sad je Sfingurina na redu...");
        scoreDictionary = new Dictionary<string, int>();
        oneStarCards = new List<string> { "goruciCovjek", "objeseniCovjek", "vodoriga" };
        twoStarCards = new List<string> { "zlatnaKula", "srebrnaKula", "patuljak" };
        threeStarCards = new List<string> { "vitez", "dvorskaLuda", "koloSrece", "lovac" };
        fourStarCards = new List<string> { "svetac", "carobnjak", "div", "kocija", "nocnaMora", "glasnik" };
        fiveStarCards = new List<string> { "kraljica", "vjestica", "kralj", "vrag", "osuda", "jednorog", "behemot", "levijatan" };
        sunSymbolCards = new List<string> { "objeseniCovjek", "srebrnaKula", "dvorskaLuda", "carobnjak", "vjestica", "vrag" };
        moonSymbolCards = new List<string> { "goruciCovjek", "zlatnaKula", "vitez", "svetac", "kraljica", "kralj" };
        starSymbolCards = new List<string> { "vodoriga", "patuljak", "koloSrece", "lovac", "div", "kocija", "nocnaMora", "glasnik", "osuda", "jednorog", "behemot", "levijatan" };
    }

    public IEnumerator BotTurn(float wait, float animationDuration)
    {
        scoreDictionary = new Dictionary<string, int>();

        //Bot takes card from the top of the castle pile and removes 1 presumed bad card if any exist
        if (botInfoPresumedBadCardsInCastle > 0) botInfoPresumedBadCardsInCastle -= 1;

        // Display bot turn message.
        if (gameLoopScript.botDifficulty == 1) dvoracScript.gameplayMsg.text = botMessages1[Random.Range(0, botMessages1.Count)];
        else if (gameLoopScript.botDifficulty == 2) dvoracScript.gameplayMsg.text = botMessages2[Random.Range(0, botMessages2.Count)];
        else dvoracScript.gameplayMsg.text = botMessages3[Random.Range(0, botMessages3.Count)];


        int cardIndex = ChooseCard();

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

    public IEnumerator BotDiscard(float wait, float animationDuration, string deck)
    {
        if (deck == "yard")
        {
            if (gameLoopScript.botDifficulty == 1) dvoracScript.gameplayMsg.text = "Woof woof... (Agatodemon gleda koju bi kartu odbacio...)";
            else if (gameLoopScript.botDifficulty == 2) dvoracScript.gameplayMsg.text = "Hermes al-Yazīd gleda koju bi kartu odbacio...";
            else dvoracScript.gameplayMsg.text = "Sfingurina gleda koju bi kartu odbacila...";
        }
        else if (deck == "castle")
        {
            if (gameLoopScript.botDifficulty == 1) dvoracScript.gameplayMsg.text = "Woof woof... (Agatodemon gleda koju bi kartu krovao...)";
            else if (gameLoopScript.botDifficulty == 2) dvoracScript.gameplayMsg.text = "Hermes al-Yazīd gleda koju bi kartu krovao...";
            else dvoracScript.gameplayMsg.text = "Sfingurina gleda koju bi kartu krovala...";
        }

        int cardIndex = ChooseCard();

        GameObject selectedCard = dvoracScript.botDeck[cardIndex];
        Debug.Log("broj karata" + dvoracScript.botDeck.Count);
        Debug.Log("index" + cardIndex);

        yield return new WaitForSeconds(wait);

        if (deck == "yard")
        {
            dvoracScript.yardDeck.Add(dvoracScript.botDeck[cardIndex]);
        }
        else if (deck == "castle")
        {
            dvoracScript.castleDeck.Add(dvoracScript.botDeck[cardIndex]);
        }
        dvoracScript.botDeck.RemoveAt(cardIndex);

        GameObject cardInstance = Instantiate(selectedCard, botArea.transform.position, Quaternion.identity);
        cardInstance.GetComponent<CardProperties>().FlipCardOn("back");
        cardInstance.GetComponent<CardProperties>().zoomable = false;
        cardInstance.GetComponent<CardProperties>().draggable = false;
        cardInstance.transform.SetParent(playScreen.transform, true);
        cardInstance.transform.localScale = new Vector3(1, 1, 1);

        if (deck == "yard")
        {
            StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, botArea.transform.position, dropZoneYard.transform.position, animationDuration));
        }
        else if (deck == "castle")
        {
            StartCoroutine(cardMoveAnimatorScript.AnimateCardMove(cardInstance, botArea.transform.position, dropZoneCastle.transform.position, animationDuration));
        }
        dvoracScript.gameplayMsg.text = "";

        yield return new WaitForSeconds(animationDuration);

        if (deck == "yard")
        {
            cardInstance.transform.SetParent(dvoracScript.dropZoneYard.transform, true);
            cardInstance.GetComponent<CardProperties>().FlipCardOn("front");
            cardInstance.GetComponent<CardProperties>().zoomable = true;
        }
        else if (deck == "castle")
        {
            cardInstance.transform.SetParent(dvoracScript.dropZoneCastle.transform, true);
        }

        dvoracScript.botCardCount.text = dvoracScript.botDeck.Count().ToString();
    }

    public int ChooseCard()
    {
        //Selecting next cart the bot will play based on chosen difficulty
        int cardIndex;
        int cardCounter;

        //Middle difficulty bot that chooses a random card
        if (gameLoopScript.botDifficulty == 2)
        {
            cardIndex = Random.Range(0, dvoracScript.botDeck.Count);
        }
        else
        {
            Debug.Log("POCETAK TURNA -- POCETAK TURNA -- POCETAK TURNA");

            //Initial cardIndex that gets revalued later, to prevent errors in code
            cardIndex = -1;

            //temp botdeck print
            foreach (GameObject karta in dvoracScript.botDeck)
            {
                Debug.Log(karta);
            }

            foreach (GameObject card in dvoracScript.botDeck)
            {
                if (scoreDictionary.ContainsKey(card.name)) { }
                else
                {
                    score = 0;
                    Debug.Log("broj karata " + dvoracScript.botDeck.Count);
                    Debug.Log("presumed bad " + botInfoPresumedBadCardsInCastle);
                    switch (card.name)
                    {
                        case "goruciCovjek":
                            score = 40 * (12 - dvoracScript.botDeck.Count);
                            score -= botInfoPresumedBadCardsInCastle * 50;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "objeseniCovjek":
                            extra = 0;
                            if (dvoracScript.botDeck.Count > 5)
                            {
                                score = 50;
                            }
                            else
                            {
                                foreach (GameObject card2 in dvoracScript.botDeck)
                                {
                                    if (moonSymbolCards.Contains(card2.name)) extra = 1;
                                }
                                score = 100 + extra * 50;
                            }
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "zlatnaKula":
                            if (dvoracScript.playerDeck.Count == 1 && dvoracScript.botDeck.Count > 2)
                            {
                                score = 0;
                            }
                            else
                            {
                                if (dvoracScript.botDeck.Count > 5)
                                {
                                    score = 175;
                                }
                                else
                                {
                                    if (botInfoPresumedBadCardsInCastle == 0)
                                    {
                                        score = 250;
                                    }
                                    else
                                    {
                                        if (botInfoPresumedBadCardsInCastle % 2 == 0) score = 175;
                                        else score = 125;
                                    }
                                }
                            }
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "srebrnaKula":
                            extra = 0;
                            if (dvoracScript.botDeck.Count < 4)
                            {
                                score = 0;
                            }
                            else
                            {
                                foreach (GameObject card2 in dvoracScript.botDeck)
                                {
                                    if (moonSymbolCards.Contains(card2.name)) extra += 1;
                                }
                                if (extra >= 2) score = 75;
                                else score = 10;
                            }
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "vitez":
                            if (dvoracScript.playerDeck.Count == 1) score = 350;
                            else score = 275;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "dvorskaLuda":
                            oneStarCounter = twoStarCounter = threeStarCounter = fourStarCounter = fiveStarCounter = 0;
                            foreach (GameObject card2 in dvoracScript.botDeck)
                            {
                                if (oneStarCards.Contains(card2.name)) oneStarCounter += 1;
                                else if (twoStarCards.Contains(card2.name)) twoStarCounter += 1;
                                else if (threeStarCards.Contains(card2.name)) threeStarCounter += 1;
                                else if (fourStarCards.Contains(card2.name)) fourStarCounter += 1;
                                else if (fiveStarCards.Contains(card2.name)) fiveStarCounter += 1;
                            }
                            if (oneStarCounter > 1 || twoStarCounter > 1 || threeStarCounter > 1 || fourStarCounter > 1 || fiveStarCounter > 1)
                            {
                                if (dvoracScript.playerDeck.Count == 1 && dvoracScript.botDeck.Count > 3) score = 200;
                                else if (dvoracScript.botDeck.Count < 4) score = 0;
                                else score = 75;
                            }
                            else
                            {
                                score = 10;
                            }
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "svetac":
                            nocnaMoraCounter = 0;
                            foreach (GameObject card2 in dvoracScript.yardDeck)
                            {
                                if (card2.name == "nocnaMora") nocnaMoraCounter += 1;
                            }

                            if (nocnaMoraCounter != 2)
                            {
                                if (dvoracScript.botDeck.Count < 12) score = 350;
                                else score = 0;
                            }
                            else
                            {
                                if (dvoracScript.botDeck.Count < 6) score = 350;
                                else score = 0;
                            }
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "carobnjak":
                            nocnaMoraCounter = 0;
                            foreach (GameObject card2 in dvoracScript.yardDeck)
                            {
                                if (card2.name == "nocnaMora") nocnaMoraCounter += 1;
                            }
                            if (nocnaMoraCounter != 2)
                            {
                                if (dvoracScript.playerDeck.Count > 12 && dvoracScript.botDeck.Count < 14) score = 275;
                                else if (dvoracScript.playerDeck.Count > 13 && dvoracScript.botDeck.Count < 14) score = 300;
                                else if (dvoracScript.playerDeck.Count > 12 && dvoracScript.botDeck.Count < 13) score = 80;
                                else score = 25;
                            }
                            else
                            {
                                if (dvoracScript.playerDeck.Count > 6 && dvoracScript.botDeck.Count < 8) score = 275;
                                else if (dvoracScript.playerDeck.Count > 7 && dvoracScript.botDeck.Count < 8) score = 300;
                                else if (dvoracScript.playerDeck.Count > 6 && dvoracScript.botDeck.Count < 7) score = 80;
                                else score = 25;
                            }
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "kraljica":
                            score = 325;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "vjestica":
                            if (dvoracScript.playerDeck.Count < 3 && dvoracScript.botDeck.Count > 2) score = 250;
                            else if (dvoracScript.playerDeck.Count < 2 && dvoracScript.botDeck.Count > 2) score = 350;
                            else score = 75;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "kralj":
                            score = 324;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "vrag":
                            if (dvoracScript.botDeck.Count < 3) score = 50;
                            else score = 100;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "vodoriga":
                            vodorigaCounter = levijatanInEitherDeck = 0;
                            foreach (GameObject card2 in dvoracScript.yardDeck)
                            {
                                if (card2.name == "vodoriga") vodorigaCounter += 1;
                                else if (card2.name == "levijatan") levijatanInEitherDeck += 1;
                            }
                            foreach (GameObject card2 in dvoracScript.botDeck)
                            {
                                if (card2.name == "vodoriga") vodorigaCounter += 1;
                                else if (card2.name == "levijatan") levijatanInEitherDeck += 1;
                            }
                            score = (16 - vodorigaCounter) * 18;
                            if (levijatanInEitherDeck == 0)
                            {
                                score += 100 * ((108 - dvoracScript.botDeck.Count - dvoracScript.yardDeck.Count) / 108);
                            }
                            else
                            {
                                score += 80;
                            }
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "patuljak":
                            if (dvoracScript.playerDeck.Count > dvoracScript.botDeck.Count - 1)
                            {
                                if (dvoracScript.playerDeck.Count < 3) score = 400;
                                else score = 200;
                            }
                            else score = 0;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "koloSrece":
                            sunCounter = moonCounter = starCounter = 0;
                            foreach (GameObject card2 in dvoracScript.botDeck)
                            {
                                if (sunSymbolCards.Contains(card2.name)) sunCounter += 1;
                                else if (moonSymbolCards.Contains(card2.name)) moonCounter += 1;
                                else if (starSymbolCards.Contains(card2.name)) starCounter += 1;
                            }
                            if (sunCounter < moonCounter && starCounter < moonCounter) score = 225;
                            else if (sunCounter < moonCounter && starCounter > moonCounter) score = 190;
                            else score = 100;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "lovac":
                            if (dvoracScript.playerDeck.Count == 1) score = 400;
                            else if (dvoracScript.playerDeck.Count < 4) score = 200;
                            else score = 150;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "div":
                            moonCounter = 0;
                            foreach (GameObject card2 in dvoracScript.botDeck)
                            {
                                if (moonSymbolCards.Contains(card2.name)) moonCounter += 1;
                            }
                            if (moonCounter > 0) score = 125;
                            else score = 15;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "kocija":
                            moonCounter = 0;
                            foreach (GameObject card2 in dvoracScript.botDeck)
                            {
                                if (moonSymbolCards.Contains(card2.name)) moonCounter += 1;
                            }
                            if (moonCounter > 0) score = 75;
                            else if (moonCounter > 12) score = 150;
                            else score = 5;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "nocnaMora":
                            svetacCarobnjakInBotDeck = 0;
                            foreach (GameObject card2 in dvoracScript.botDeck)
                            {
                                if (card2.name == "svetac") svetacCarobnjakInBotDeck += 1;
                                else if (card2.name == "carobnjak") svetacCarobnjakInBotDeck += 1;
                            }
                            if (svetacCarobnjakInBotDeck > 0) score = 225;
                            else score = 100;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "glasnik":
                            if (dvoracScript.playerDeck.Count == 1) score = 325;
                            else if (dvoracScript.playerDeck.Count == 2) score = 275;
                            else if (dvoracScript.playerDeck.Count == 3) score = 210;
                            else if (dvoracScript.playerDeck.Count == 4) score = 180;
                            else score = 100;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "osuda":
                            score = 25 * (14 - dvoracScript.botDeck.Count) - (10 - dvoracScript.playerDeck.Count) * 15;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "jednorog":
                            if (dvoracScript.playerDeck.Count < 5) score = 325;
                            else score = 225;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "behemot":
                            starCounter = 0;
                            foreach (GameObject card2 in dvoracScript.yardDeck)
                            {
                                if (starSymbolCards.Contains(card2.name)) starCounter += 1;
                            }
                            score = 22 * (14 - dvoracScript.botDeck.Count) - (10 - dvoracScript.playerDeck.Count) * 15;
                            if (starCounter % 2 == 1) score += 110;
                            scoreDictionary.Add(card.name, score);
                            break;

                        case "levijatan":
                            oneStarCounter = 0;
                            foreach (GameObject card2 in dvoracScript.yardDeck)
                            {
                                if (oneStarCards.Contains(card2.name)) oneStarCounter += 1;
                            }
                            sunCounter = moonCounter = starCounter = 0;
                            foreach (GameObject card2 in dvoracScript.botDeck)
                            {
                                if (sunSymbolCards.Contains(card2.name)) sunCounter += 1;
                                else if (moonSymbolCards.Contains(card2.name)) moonCounter += 1;
                                else if (starSymbolCards.Contains(card2.name)) starCounter += 1;
                            }
                            if (dvoracScript.botDeck.Count < 5 && sunCounter < moonCounter && starCounter < moonCounter) score = 200;
                            else if (dvoracScript.botDeck.Count < 3 && sunCounter < moonCounter && starCounter < moonCounter) score = 120;
                            score += oneStarCounter * 20;
                            scoreDictionary.Add(card.name, score);
                            break;
                    }
                }
            }

            //temp score print
            foreach (KeyValuePair<string, int> kvp in scoreDictionary)
            {
                Debug.Log("karta: " + kvp.Key + "  score: " + kvp.Value);
            }

            if (gameLoopScript.botDifficulty == 1)
            {
                //Getting chosen card index based on min score
                var keyOfMaxValue = scoreDictionary.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;
                cardCounter = 0;
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
            else if (gameLoopScript.botDifficulty == 3)
            {
                //Getting chosen card index based on max score
                var keyOfMaxValue = scoreDictionary.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                cardCounter = 0;
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

        }
        return cardIndex;
    }
}
