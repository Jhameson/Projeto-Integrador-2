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
    
     [SerializeField] private GameObject InstrucoesPanel;


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Start()
    {
        UserNameMenu.SetActive(true);
    }

    public  override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Conectado");
    }

    public void ChangeUsernameInput()
    {
        if (UsernameInput.text.Length >= 4)
        {
            BtnStart.SetActive(true);
        }
        else
        {
            BtnStart.SetActive(false);
        }
    }

    public void Duvidas(){
        InstrucoesPanel.SetActive(true);
    }

    public void SetUserName()
    {
        UserNameMenu.SetActive(false);
        PhotonNetwork.NickName = UsernameInput.text;
    }
    public void CreateGame()
    {

        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions { MaxPlayers = 10 }, null);
        
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
    }

    public  override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("TelaGame");
    }

    

}