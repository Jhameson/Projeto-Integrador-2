using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;


using Random = UnityEngine.Random;

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
    // public GameObject Itens;
    
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
    
    [Header("Spawn de jogadores")]
    public GameObject[] spawnPoint;
    private int randPosition;

    [Header("Spawn de itens")]
    public GameObject[] si_itens;
    public GameObject[] si_spawnPoint;

    private int si_rand;
    private int si_randPosition;

    public float si_startTimeSpawner;
    private float si_timeSpwans;

    private int resultado;


    
    private void Awake()
    {
        Instance = this;
        GameCanvas.SetActive(true);
    }

    private void Start(){
       scoreBoard = GameObject.Find("GameCanvas").transform.Find("FimDeJogo").gameObject;
       //novo
       si_timeSpwans = si_startTimeSpawner;
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
            countTi.color = Color.blue;
            countTi.text = (-(timerDecrementeValue) + timer).ToString("0");
        if (timerDecrementeValue >= timer)
        {
            FimdoGame();
        }
        if (timerDecrementeValue >= 90)
        {
            countTi.color = Color.red;
            
            
        }
        

        if(si_timeSpwans <=0){

            SpawnItens();
        }else{
            si_timeSpwans -= Time.deltaTime;
        }
    }


    


    public void EnableRespawn()
    {
        TimerAmount = 1f;
        RunSpawnTimer = true;
        RespawnMenu.SetActive(true);
    }

    private void StartRespawn()
    {
        TimerAmount = TimerAmount - Time.deltaTime;
        RespawnTimerText.text = "Renascendo...";
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
         
       randPosition = Random.Range(0,spawnPoint.Length);
        LocalPlayer.transform.localPosition = new Vector2(spawnPoint[randPosition].transform.position.x, spawnPoint[randPosition].transform.position.y);

    }

    
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
        randPosition = Random.Range(0,spawnPoint.Length);

        PhotonNetwork.Instantiate(PlayerPrefab[id].name, new Vector2(spawnPoint[randPosition].transform.position.x, spawnPoint[randPosition].transform.position.y), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(false);
        
    }

    public void SetID(int Id)
    {
        id = Id;
    }

    public void BackToMenu()
    {
        resultado = PhotonNetwork.LocalPlayer.GetScore();
        PhotonNetwork.LocalPlayer.AddScore(-resultado);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("TelaMenu");
    }

    public void Restart()
    {
        resultado = PhotonNetwork.LocalPlayer.GetScore();
        PhotonNetwork.LocalPlayer.AddScore(-resultado);
        FimDeJogo.SetActive(false);
        
        PhotonNetwork.Destroy(LocalPlayer);
        GameCanvas.SetActive(true);
        timer = 180;
        Start();
        
    }

    public void QuitGame()
    {
        resultado = PhotonNetwork.LocalPlayer.GetScore();
        PhotonNetwork.LocalPlayer.AddScore(-resultado);
        PhotonNetwork.LeaveRoom();
        Application.Quit();
       
    }
       


    void ScoreBoard(){
        playerCount = PhotonNetwork.PlayerList.Length;

        var playerNames = new StringBuilder();
        foreach(Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
             playerNames.Append("- Nick: "+p.NickName+"   ====> SCORE: "+p.GetScore()+"\n");
        }

        string saida = "-> Quatidade de Jogadores na sala: "+playerCount.ToString() + "\n\n " +playerNames.ToString();
        scoreBoard.transform.Find("ListaScores").GetComponent<Text>().text = saida;

        
    }   
    public void FimdoGame(){
        
        ScoreBoard();
        FimDeJogo.SetActive(true);
    }

    /////////////Respwan de itens

    [PunRPC]
    private void SpawnItens(){
        si_rand = Random.Range(0,si_itens.Length);
        si_randPosition = Random.Range(0,si_spawnPoint.Length);


        PhotonNetwork.Instantiate(si_itens[si_rand].name, new Vector2(si_spawnPoint[si_randPosition].transform.position.x, si_spawnPoint[si_randPosition].transform.position.y), Quaternion.identity, 0);
        //PhotonNetwork.Instantiate(PlayerPrefab[id].name, new Vector2(respRange, 4f), Quaternion.identity, 0);
        si_timeSpwans = si_startTimeSpawner;
    }
   
}