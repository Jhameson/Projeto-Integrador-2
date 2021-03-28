using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


using Photon.Pun.UtilityScripts;

public class buffJump : MonoBehaviourPunCallbacks
{
    public float valor;

    [PunRPC]
    public void DestroyObject()
     {
        PhotonNetwork.LocalPlayer.AddScore(1);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 

        if(!photonView.IsMine)
            return;
        

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if(target.tag == "Player")
        {
            target.RPC("Buffjump",Photon.Pun.RpcTarget.AllBuffered, valor);
            //collision.gameObject.GetComponent<Health>().RaiseHealth(vida);
             this.GetComponent<PhotonView>().RPC("DestroyObject", Photon.Pun.RpcTarget.AllBuffered);
        }  
    }
}
