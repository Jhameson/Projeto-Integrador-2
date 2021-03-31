using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class MenuController : MonoBehaviourPunCallbacks
{
    //[SerializeField] private string vM = "0.1";
    [SerializeField] private GameObject UserNameMenu;
    [SerializeField] private GameObject ConnectPanel;
    [SerializeField] private InputField UsernameInput;
    [SerializeField] private InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;
    
    [SerializeField] private GameObject BtnStart;
    [SerializeField] private GameObject BtnSair;
    
     [SerializeField] private GameObject InstrucoesPanel;
     [SerializeField] private GameObject InstrucoesPanel2;
     [SerializeField] private GameObject InstrucoesPanel3;


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("sa");
    }

    private void Start()
    {
        UserNameMenu.SetActive(true);
    }

     public void ChangeUsernameInput()
    {
        if (UsernameInput.text.Length >= 3 && UsernameInput.text.Length <=8)
        {
            BtnStart.SetActive(true);
        }
        else
        {
            BtnStart.SetActive(false);
        }
    }

    public void SetUserName()
    {
        UserNameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.text;
    }

    public void Duvidas(){
        InstrucoesPanel.SetActive(true);
    }

    public void proximo(){
        InstrucoesPanel.SetActive(false);
        InstrucoesPanel2.SetActive(true);
    }
    public void proximo2(){
        InstrucoesPanel2.SetActive(false);
        InstrucoesPanel3.SetActive(true);
    }
    public void anterior(){
        InstrucoesPanel.SetActive(true);
        InstrucoesPanel2.SetActive(false);
    }
    public void anterior2(){
        InstrucoesPanel2.SetActive(true);
        InstrucoesPanel3.SetActive(false);
    }

    public void sair(){
        
        InstrucoesPanel3.SetActive(false);
    }

    public  override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Conectado");
    }

    
    public void CreateGame()
    {

        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions { MaxPlayers = 20 }, null);
        Debug.Log("Sala criada");
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        //PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
         
         PhotonNetwork.JoinRoom(JoinGameInput.text);
    }

    public  override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("TelaGame");
    }

    public void QuitGame()
    {
        
        Application.Quit();
       
    }

}