using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

using System.Text;
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;
    //score
    GameObject scoreBoard;
    int playerCount;
    ////
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    ////
    [SerializeField]
    public GameObject []PlayerPrefab;
    [SerializeField]
    private int id;
    
    [HideInInspector]
    public GameObject LocalPlayer;
    public GameObject Itens;
    
    public Text RespawnTimerText;
    
    public GameObject RespawnMenu;

    private float TimerAmount = 2f;
    private bool RunSpawnTimer = false;

    /////////tempo
    // float currentTime = 0f;
    // float startTime = 50f;
    [SerializeField] Text countTi;

    //novo
    bool startTimer = false;
    double timerDecrementeValue;
    double startTime;
    [SerializeField] double timer;
    ExitGames.Client.Photon.Hashtable CustomeValue;

    ///////////////
    [Header("Menus")]
    private bool Off = false;
    public GameObject pausePanel;
    public GameObject FimDeJogo;
    
    


    private void Awake()
    {
        Instance = this;
        GameCanvas.SetActive(true);
    }

    private void Start(){
       scoreBoard = GameObject.Find("GameCanvas").transform.Find("FimDeJogo").gameObject;
       //novo
        if (PhotonNetwork.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.Time;
            startTimer = true;
             CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.CurrentRoom.SetCustomProperties(CustomeValue);
        }
        else
        {
            startTime = double.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
            startTimer = true;
        }
    }

    void Update()
    {
        PauseScreen();
    
        if(RunSpawnTimer)
        {
            StartRespawn();
            
        }
        if (!startTimer) return;
            timerDecrementeValue = PhotonNetwork.Time - startTime;
            countTi.text = (-(timerDecrementeValue) + timer).ToString("0");
        if (timerDecrementeValue >= timer)
        {
            FimdoGame();
        }
    }


    


    public void EnableRespawn()
    {
        TimerAmount = 2f;
        RunSpawnTimer = true;
        RespawnMenu.SetActive(true);
    }

    private void StartRespawn()
    {
        TimerAmount = TimerAmount - Time.deltaTime;
        RespawnTimerText.text = "Renascendo em " + TimerAmount.ToString("F0");
        Debug.Log(TimerAmount);

        if(TimerAmount<=0)
        {
            LocalPlayer.GetComponent<PhotonView>().RPC("Respawn", Photon.Pun.RpcTarget.AllBuffered);
            LocalPlayer.GetComponent<Health>().EnableInput();
            RespawnLocation();
            RespawnMenu.SetActive(false);
            RunSpawnTimer = false;
        }
    }

    public void RespawnLocation()
    {
        float respRange = Random.Range(-25f, 25f);
        LocalPlayer.transform.localPosition = new Vector2(respRange, 4f);

    }

    // public void RespawnItens()
    // {
    //     float respRange = Random.Range(-25f, 25f);
    //     Ite.transform.localPosition = new Vector2(respRange, 4f);

    // }
    private void PauseScreen()
    {
        if (Off && Input.GetKeyDown(KeyCode.Escape))
        {
            
            pausePanel.SetActive(false);
            Off = false;
        }
        else if (!Off && Input.GetKeyDown(KeyCode.Escape))
        {
           
            pausePanel.SetActive(true);
            Off = true;
        }
    }  

    public void SpawnPlayer()
    {

        float respRange = Random.Range(-25f, 25f);
        PhotonNetwork.Instantiate(PlayerPrefab[id].name, new Vector2(respRange, 4f), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
    }

    public void SetID(int Id)
    {
        id = Id;
    }

    public void BackToMenu()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TelaMenu");
    }

    public void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
        Application.Quit();
       
    }
       


    void ScoreBoard(){
        playerCount = PhotonNetwork.PlayerList.Length;

        var playerNames = new StringBuilder();
        foreach(Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
             playerNames.Append("Nick: "+p.NickName+" -- SCORE: "+p.GetScore()+"\n");
        }

        string saida = "Quatidade de Jogadores na sala: "+playerCount.ToString() + "\n\n" +playerNames.ToString();
        scoreBoard.transform.Find("Text").GetComponent<Text>().text = saida;

        
    }   
    public void FimdoGame(){
        ScoreBoard();
        FimDeJogo.SetActive(true);
    }

    
   
}