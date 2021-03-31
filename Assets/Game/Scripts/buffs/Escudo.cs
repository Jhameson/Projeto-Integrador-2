using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


using Photon.Pun.UtilityScripts;

public class Escudo : MonoBehaviourPunCallbacks
{
    

    [PunRPC]
    public void DestroyObject()
     {
        //PhotonNetwork.LocalPlayer.AddScore(-3);
        Destroy(gameObject);
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    { 

        if(!photonView.IsMine)
            return;
        

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if(target.tag == "Player")
        {
            //target.RPC("RaiseHealth",RpcTarget.AllBuffered,vida);
            target.RPC("EnableShield",RpcTarget.AllBuffered, true);
            
            //collision.gameObject.GetComponent<Health>().RaiseHealth(vida);
             this.GetComponent<PhotonView>().RPC("DestroyObject", Photon.Pun.RpcTarget.AllBuffered);
             
        }      
    }
}


