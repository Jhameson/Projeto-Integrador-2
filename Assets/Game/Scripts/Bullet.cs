using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.Tilemaps;

public class Bullet : MonoBehaviourPunCallbacks
{
    public static Bullet instance;
   public bool MoveDir = false;
   
   public float MoveSpeed ;

   public float DestroyTime ;

   public float BulletDamage;
 
    Tilemap tilemap;
   private void Awake()
   {
       StartCoroutine("DestroyByTime");
       
    }

   

   IEnumerator DestroyByTime()
   {
       yield return new WaitForSeconds(DestroyTime);
       this.GetComponent<PhotonView>().RPC("DestroyObject", Photon.Pun.RpcTarget.AllBuffered);
       
   }

   [PunRPC]
   public void ChangeDir_left()
   {
       MoveDir = true;
   }

   [PunRPC]
   public void DestroyObject()
   {
        
        Destroy(this.gameObject);
   }

    private void Update()
    {
        if(!MoveDir)
            transform.Translate(Vector2.right * MoveSpeed * Time.deltaTime);
        else
            transform.Translate(Vector2.left * MoveSpeed * Time.deltaTime);
    }

    
        
        

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!photonView.IsMine)
            return;
        

        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if(target != null && (!target.IsMine || target.IsSceneView))
        {
            if(target.tag == "Player")
            {
                target.RPC("ReduceHealth",RpcTarget.AllBuffered,BulletDamage);
               PhotonNetwork.LocalPlayer.AddScore(1);
            }
            if (target.tag == "Chao" || target.tag == "Player" || target.tag == "Escudo")
            {
                Debug.Log("aquiii");
                this.GetComponent<PhotonView>().RPC("DestroyObject", Photon.Pun.RpcTarget.AllBuffered);
            }
           
        }

        
    }
    

    
     


}
