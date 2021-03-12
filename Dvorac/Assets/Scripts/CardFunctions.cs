using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using Asyncoroutine;
using UnityEngine;

public class CardFunctions : MonoBehaviour
{
    public GameObject audioManager;

    private AudioManager audioManagerScript;
    private GameLoop gameLoopScript;
    private Bot botScript;
    private Dvorac dvoracScript;

    public Dictionary<string, Action> playerCardFunctionalities = new Dictionary<string, Action>();
    public Dictionary<string, Action> botCardFunctionalities = new Dictionary<string, Action>();

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager");
        audioManagerScript = audioManager.GetComponent<AudioManager>();
        dvoracScript = GetComponent<Dvorac>();
        gameLoopScript = GetComponent<GameLoop>();
        botScript = GetComponent<Bot>();
    }

    private void Start()
    {
        playerCardFunctionalities.Add("goruciCovjek", GoruciCovjekPlayer);
        playerCardFunctionalities.Add("objeseniCovjek", ObjeseniCovjekPlayer);
        playerCardFunctionalities.Add("zlatnaKula", ZlatnaKulaPlayer);
        playerCardFunctionalities.Add("srebrnaKula", SrebrnaKulaPlayer);
        playerCardFunctionalities.Add("vitez", VitezPlayer);
        playerCardFunctionalities.Add("dvorskaLuda", DvorskaLudaPlayer);
        playerCardFunctionalities.Add("svetac", SvetacPlayer);
        playerCardFunctionalities.Add("carobnjak", CarobnjakPlayer);
        playerCardFunctionalities.Add("kraljica", KraljicaPlayer);
        playerCardFunctionalities.Add("vjestica", VjesticaPlayer);
        playerCardFunctionalities.Add("kralj", KraljPlayer);
        playerCardFunctionalities.Add("vrag", VragPlayer);
        playerCardFunctionalities.Add("vodoriga", VodorigaPlayer);
        playerCardFunctionalities.Add("patuljak", PatuljakPlayer);
        playerCardFunctionalities.Add("koloSrece", KoloSrecePlayer);
        playerCardFunctionalities.Add("lovac", LovacPlayer);
        playerCardFunctionalities.Add("div", DivPlayer);
        playerCardFunctionalities.Add("kocija", KocijaPlayer);
        playerCardFunctionalities.Add("nocnaMora", NocnaMoraPlayer);
        playerCardFunctionalities.Add("glasnik", GlasnikPlayer);
        playerCardFunctionalities.Add("osuda", OsudaPlayer);
        playerCardFunctionalities.Add("jednorog", JednorogPlayer);
        playerCardFunctionalities.Add("behemot", BehemotPlayer);
        playerCardFunctionalities.Add("levijatan", LevijatanPlayer);

        botCardFunctionalities.Add("goruciCovjek", GoruciCovjekBot);
        botCardFunctionalities.Add("objeseniCovjek", ObjeseniCovjekBot);
        botCardFunctionalities.Add("zlatnaKula", ZlatnaKulaBot);
        botCardFunctionalities.Add("srebrnaKula", SrebrnaKulaBot);
        botCardFunctionalities.Add("vitez", VitezBot);
        botCardFunctionalities.Add("dvorskaLuda", DvorskaLudaBot);
        botCardFunctionalities.Add("svetac", SvetacBot);
        botCardFunctionalities.Add("carobnjak", CarobnjakBot);
        botCardFunctionalities.Add("kraljica", KraljicaBot);
        botCardFunctionalities.Add("vjestica", VjesticaBot);
        botCardFunctionalities.Add("kralj", KraljBot);
        botCardFunctionalities.Add("vrag", VragBot);
        botCardFunctionalities.Add("vodoriga", VodorigaBot);
        botCardFunctionalities.Add("patuljak", PatuljakBot);
        botCardFunctionalities.Add("koloSrece", KoloSreceBot);
        botCardFunctionalities.Add("lovac", LovacBot);
        botCardFunctionalities.Add("div", DivBot);
        botCardFunctionalities.Add("kocija", KocijaBot);
        botCardFunctionalities.Add("nocnaMora", NocnaMoraBot);
        botCardFunctionalities.Add("glasnik", GlasnikBot);
        botCardFunctionalities.Add("osuda", OsudaBot);
        botCardFunctionalities.Add("jednorog", JednorogBot);
        botCardFunctionalities.Add("behemot", BehemotBot);
        botCardFunctionalities.Add("levijatan", LevijatanBot);
    }

    private void EndOfTurn(string who)
    {
        if(who == "player")
        {
            // If player has no cards left; end game
            if (dvoracScript.playerDeck.Count == 0)
            {
                gameLoopScript.EndGame("defeat");
                return;
            }
            // Disable player input
            dvoracScript.playerTurn = false;
            // Start bot's turn
            StartCoroutine(botScript.BotTurn(2.6f, 0.4f));
        }
        else
        {
            // If bot has no cards left; end game
            if (dvoracScript.botDeck.Count == 0)
            {
                gameLoopScript.EndGame("victory");
                return;
            }
            // Enable player input
            dvoracScript.playerTurn = true;
        }
    }

    public async void GoruciCovjekPlayer()
    {
        audioManagerScript.PlaySound("goruciCovjek");

        dvoracScript.FetchCard("player");
        // Wait for the duration of FetchCard function
        await new WaitForSeconds(.2f);

        EndOfTurn("player");
    }

    public void ObjeseniCovjekPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("objeseniCovjek");

        // If player has no cards left; end game
        if (dvoracScript.playerDeck.Count == 0)
        {
            gameLoopScript.EndGame("defeat");
            return;
        }

        dvoracScript.gameplayMsg.text = "Odbaci kartu.";
        // Define that the next played card will be discarded
        dvoracScript.playAction = "discardOC";
    }

    public async void ZlatnaKulaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("zlatnaKula");

        for (int i = 0; i < 2; i++)
        {
            dvoracScript.FetchCard("player");
            await new WaitForSeconds(.2f);
            dvoracScript.FetchCard("bot");
            await new WaitForSeconds(.2f);
        }

        EndOfTurn("player");
    }

    public void SrebrnaKulaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("srebrnaKula");

        // If player has no cards left; end game
        if (dvoracScript.playerDeck.Count == 0)
        {
            gameLoopScript.EndGame("defeat");
            return;
        }

        dvoracScript.PlayNext("castle");
        dvoracScript.gameplayMsg.text = "Krovaj kartu.";
        dvoracScript.playAction = "discardSKP1";
    }

    public async void VitezPlayer()
    {
        // TODO:
        // Set revealed cards to bot memory depending on difficulty level
        audioManagerScript.PlaySound("vitez");

        for (int i = 0; i < 2; i++)
        {
            dvoracScript.FetchCard("player");
            // Wait for the duration of FetchCard function
            await new WaitForSeconds(.2f);
            if (i == 0)
            {
                dvoracScript.RevealCard(dvoracScript.playerDeck.Last<GameObject>(), dvoracScript.cardReveal1);
            }
            else
            {
                dvoracScript.RevealCard(dvoracScript.playerDeck.Last<GameObject>(), dvoracScript.cardReveal2);
            }
        }
        if (dvoracScript.playerDeck[dvoracScript.playerDeck.Count - 1].GetComponent<CardProperties>().cardSymbol != dvoracScript.playerDeck[dvoracScript.playerDeck.Count - 2].GetComponent<CardProperties>().cardSymbol)
        {
            dvoracScript.gameplayMsg.text = "Bot preskače svoj potez.";
        }
        await new WaitForSeconds(1.6f);

        dvoracScript.CancelRevealCard();
        dvoracScript.gameplayMsg.text = "";

        if (dvoracScript.playerDeck[dvoracScript.playerDeck.Count - 1].GetComponent<CardProperties>().cardSymbol == dvoracScript.playerDeck[dvoracScript.playerDeck.Count - 2].GetComponent<CardProperties>().cardSymbol)
        {
            EndOfTurn("player");
        }
    }

    public void DvorskaLudaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("dvorskaLuda");

        // If player has no cards left; end game
        if (dvoracScript.playerDeck.Count == 0)
        {
            gameLoopScript.EndGame("defeat");
            return;
        }

        dvoracScript.gameplayMsg.text = "Odbaci kartu.";
        // Define that the next played card will be discarded
        dvoracScript.playAction = "discardDLP1";
    }

    public void SvetacPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("svetac");

        EndOfTurn("player");
    }

    public void CarobnjakPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("carobnjak");

        EndOfTurn("player");
    }

    public void KraljicaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("kraljica");

        EndOfTurn("player");
    }

    public void VjesticaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("vjestica");

        EndOfTurn("player");
    }

    public void KraljPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("kralj");

        EndOfTurn("player");
    }

    public void VragPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("vrag");

        EndOfTurn("player");
    }

    public void VodorigaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("vodoriga");

        EndOfTurn("player");
    }

    public void PatuljakPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("patuljak");

        EndOfTurn("player");
    }

    public void KoloSrecePlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("koloSrece");

        EndOfTurn("player");
    }

    public void LovacPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("lovac");

        EndOfTurn("player");
    }

    public void DivPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("div");

        EndOfTurn("player");
    }

    public void KocijaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("kocija");

        EndOfTurn("player");
    }

    public void NocnaMoraPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("nocnaMora");

        EndOfTurn("player");
    }

    public void GlasnikPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("glasnik");

        EndOfTurn("player");
    }

    public void OsudaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("osuda");

        EndOfTurn("player");
    }

    public void JednorogPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("jednorog");

        EndOfTurn("player");
    }

    public void BehemotPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("behemot");

        EndOfTurn("player");
    }

    public void LevijatanPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("levijatan");

        EndOfTurn("player");
    }

    public async void GoruciCovjekBot()
    {
        audioManagerScript.PlaySound("goruciCovjek");

        dvoracScript.FetchCard("bot");
        // Wait for the duration of FetchCard function
        await new WaitForSeconds(.2f);

        EndOfTurn("bot");
    }

    public async void ObjeseniCovjekBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("objeseniCovjek");

        // If bot has no cards left; end game
        if (dvoracScript.botDeck.Count == 0)
        {
            gameLoopScript.EndGame("victory");
            return;
        }

        StartCoroutine(botScript.BotDiscard(1.1f, .4f, "yard"));

        await new WaitForSeconds(1.5f);

        EndOfTurn("bot");
    }

    public async void ZlatnaKulaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("zlatnaKula");

        for (int i = 0; i < 2; i++)
        {
            dvoracScript.FetchCard("bot");
            await new WaitForSeconds(.2f);
            dvoracScript.FetchCard("player");
            await new WaitForSeconds(.2f);
        }

        EndOfTurn("bot");
    }

    public async void SrebrnaKulaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("srebrnaKula");

        if (dvoracScript.botDeck.Count == 0)
        {
            gameLoopScript.EndGame("victory");
            return;
        }

        dvoracScript.PlayNext("castle");

        StartCoroutine(botScript.BotDiscard(1.1f, .4f, "castle"));
        await new WaitForSeconds(1.5f);

        if (dvoracScript.botDeck.Count == 0)
        {
            gameLoopScript.EndGame("victory");
            return;
        }

        dvoracScript.playerTurn = true;
        dvoracScript.gameplayMsg.text = "Krovaj kartu.";
        dvoracScript.playAction = "discardSKB1";
    }

    public async void VitezBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("vitez");

        for (int i = 0; i < 2; i++)
        {
            dvoracScript.FetchCard("bot");
            // Wait for the duration of FetchCard function
            await new WaitForSeconds(.2f);
            if (i == 0)
            {
                dvoracScript.RevealCard(dvoracScript.botDeck.Last<GameObject>(), dvoracScript.cardReveal1);
            }
            else
            {
                dvoracScript.RevealCard(dvoracScript.botDeck.Last<GameObject>(), dvoracScript.cardReveal2);
            }
        }
        if (dvoracScript.playerDeck[dvoracScript.playerDeck.Count - 1].GetComponent<CardProperties>().cardSymbol != dvoracScript.playerDeck[dvoracScript.playerDeck.Count - 2].GetComponent<CardProperties>().cardSymbol)
        {
            dvoracScript.gameplayMsg.text = "Preskačeš svoj potez.";
        }
        await new WaitForSeconds(1.6f);

        dvoracScript.CancelRevealCard();
        dvoracScript.gameplayMsg.text = "";

        if (dvoracScript.playerDeck[dvoracScript.playerDeck.Count - 1].GetComponent<CardProperties>().cardSymbol == dvoracScript.playerDeck[dvoracScript.playerDeck.Count - 2].GetComponent<CardProperties>().cardSymbol)
        {
            EndOfTurn("bot");
        }
        else
        {
            StartCoroutine(botScript.BotTurn(2.6f, 0.4f));
        }
    }

    public async void DvorskaLudaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("dvorskaLuda");

        // If bot has no cards left; end game
        if (dvoracScript.botDeck.Count == 0)
        {
            gameLoopScript.EndGame("victory");
            return;
        }

        StartCoroutine(botScript.BotDiscard(1.1f, .4f, "yard"));
        await new WaitForSeconds(1.5f);
        if (dvoracScript.botDeck.Count == 0)
        {
            gameLoopScript.EndGame("victory");
            return;
        }
        StartCoroutine(botScript.BotDiscard(1.1f, .4f, "yard"));
        await new WaitForSeconds(1.5f);
        if (dvoracScript.botDeck.Count == 0)
        {
            gameLoopScript.EndGame("victory");
            return;
        }

        if (dvoracScript.yardDeck[dvoracScript.yardDeck.Count - 1].GetComponent<CardProperties>().cardRarity == dvoracScript.yardDeck[dvoracScript.yardDeck.Count - 2].GetComponent<CardProperties>().cardRarity)
        {
            dvoracScript.playAction = "discardDLB";
            dvoracScript.playerTurn = true;
            dvoracScript.gameplayMsg.text = "Odbaci kartu.";
        }
        else
        {
            EndOfTurn("bot");
        }
    }

    public void SvetacBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("svetac");

        EndOfTurn("bot");
    }

    public void CarobnjakBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("carobnjak");

        EndOfTurn("bot");
    }

    public void KraljicaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("kraljica");

        EndOfTurn("bot");
    }

    public void VjesticaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("vjestica");

        EndOfTurn("bot");
    }

    public void KraljBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("kralj");

        EndOfTurn("bot");
    }

    public void VragBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("vrag");

        EndOfTurn("bot");
    }

    public void VodorigaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("vodoriga");

        EndOfTurn("bot");
    }

    public void PatuljakBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("patuljak");

        EndOfTurn("bot");
    }

    public void KoloSreceBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("koloSrece");

        EndOfTurn("bot");
    }

    public void LovacBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("lovac");

        EndOfTurn("bot");
    }

    public void DivBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("div");

        EndOfTurn("bot");
    }

    public void KocijaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("kocija");

        EndOfTurn("bot");
    }

    public void NocnaMoraBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("nocnaMora");

        EndOfTurn("bot");
    }

    public void GlasnikBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("glasnik");

        EndOfTurn("bot");
    }

    public void OsudaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("osuda");

        EndOfTurn("bot");
    }

    public void JednorogBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("jednorog");

        EndOfTurn("bot");
    }

    public void BehemotBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("behemot");

        EndOfTurn("bot");
    }

    public void LevijatanBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("levijatan");

        EndOfTurn("bot");
    }
}
