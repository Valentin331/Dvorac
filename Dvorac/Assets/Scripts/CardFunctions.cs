using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

public class CardFunctions : MonoBehaviour
{
    public GameObject audioManager;

    private AudioManager audioManagerScript;

    public Dictionary<string, Action> playerCardFunctionalities = new Dictionary<string, Action>();
    public Dictionary<string, Action> botCardFunctionalities = new Dictionary<string, Action>();

    private void Awake()
    {
        audioManager = GameObject.Find("AudioManager");
        audioManagerScript = audioManager.GetComponent<AudioManager>();
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

    public void GoruciCovjekPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("goruciCovjek");
    }

    public void ObjeseniCovjekPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("objeseniCovjek");
    }

    public void ZlatnaKulaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("zlatnaKula");
    }

    public void SrebrnaKulaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("srebrnaKula");
    }

    public void VitezPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("vitez");
    }

    public void DvorskaLudaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("dvorskaLuda");
    }

    public void SvetacPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("svetac");
    }

    public void CarobnjakPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("carobnjak");
    }

    public void KraljicaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("kraljica");
    }

    public void VjesticaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("vjestica");
    }

    public void KraljPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("kralj");
    }

    public void VragPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("vrag");
    }

    public void VodorigaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("vodoriga");
    }

    public void PatuljakPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("patuljak");
    }

    public void KoloSrecePlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("koloSrece");
    }

    public void LovacPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("lovac");
    }

    public void DivPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("div");
    }

    public void KocijaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("kocija");
    }

    public void NocnaMoraPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("nocnaMora");
    }

    public void GlasnikPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("glasnik");
    }

    public void OsudaPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("osuda");
    }

    public void JednorogPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("jednorog");
    }

    public void BehemotPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("behemot");
    }

    public void LevijatanPlayer()
    {
        // TODO:
        // Write code that will be executed when player plays this card
        audioManagerScript.PlaySound("levijatan");
    }

    public void GoruciCovjekBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("goruciCovjek");
    }

    public void ObjeseniCovjekBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("objeseniCovjek");
    }

    public void ZlatnaKulaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("zlatnaKula");
    }

    public void SrebrnaKulaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("srebrnaKula");
    }

    public void VitezBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("vitez");
    }

    public void DvorskaLudaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("dvorskaLuda");
    }

    public void SvetacBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("svetac");
    }

    public void CarobnjakBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("carobnjak");
    }

    public void KraljicaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("kraljica");
    }

    public void VjesticaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("vjestica");
    }

    public void KraljBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("kralj");
    }

    public void VragBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("vrag");
    }

    public void VodorigaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("vodoriga");
    }

    public void PatuljakBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("patuljak");
    }

    public void KoloSreceBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("koloSrece");
    }

    public void LovacBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("lovac");
    }

    public void DivBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("div");
    }

    public void KocijaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("kocija");
    }

    public void NocnaMoraBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("nocnaMora");
    }

    public void GlasnikBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("glasnik");
    }

    public void OsudaBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("osuda");
    }

    public void JednorogBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("jednorog");
    }

    public void BehemotBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("behemot");
    }

    public void LevijatanBot()
    {
        // TODO:
        // Write code that will be executed when bot plays this card
        audioManagerScript.PlaySound("levijatan");
    }
}
