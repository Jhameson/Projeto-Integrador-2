using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;





public class ItemBuffVel : MonoBehaviourPunCallbacks
{
    public float valor ;

    [PunRPC]
    public void DestroyObject()
     {
       
       Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    { 

        if(!photonView.IsMine)
            return;
        

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if(target.tag == "Player")
        {
            target.RPC("Buffvel",Photon.Pun.RpcTarget.AllBuffered, valor);
            //collision.gameObject.GetComponent<Health>().RaiseHealth(vida);
             this.GetComponent<PhotonView>().RPC("DestroyObject", Photon.Pun.RpcTarget.AllBuffered);
        }      
    }
}


