using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


public class Health : MonoBehaviourPunCallbacks
{
    public static Health instance;

    //
    public float HealthAmount;
    public Rigidbody2D rby;
    public BoxCollider2D bcr;
    public SpriteRenderer srr;
    public Player plMove;
    public GameObject PlayerCanvas;
    // 
    public Image FillImage;
    public bool kill=false;


    private void Awake()
    {
        if(photonView.IsMine)
        {
            GameManager.Instance.LocalPlayer = this.gameObject;
        }
    }
    

    [PunRPC] 
    public void ReduceHealth(float amount)
    {

        ModifyHealth(amount);
    }

    private void CheckHealth()
    {
        FillImage.fillAmount = HealthAmount / 50f;
        if(photonView.IsMine && HealthAmount <= 0)
        {
            GameManager.Instance.EnableRespawn(); 
            plMove.DisableInput = true;
            this.GetComponent<PhotonView>().RPC("Dead", Photon.Pun.RpcTarget.AllBuffered);
                      
            
        }
    }

   

    public void EnableInput()
    {
        plMove.DisableInput = false;
    }


    [PunRPC]
    private void Dead()
    {
        rby.gravityScale = 0;
        bcr.enabled = false;
        srr.enabled = false;
        PlayerCanvas.SetActive(false);
        
    }

    [PunRPC]
    private void Respawn()
    {
        rby.gravityScale = 1;
        bcr.enabled = true;
        srr.enabled = true;
        PlayerCanvas.SetActive(true);
        FillImage.fillAmount = 1f;
        HealthAmount = 100f;
    }

    private void ModifyHealth(float amount)
    {
        if(photonView.IsMine)
        {
            HealthAmount -= amount;
            FillImage.fillAmount -= amount;
        }
        else
        {
            HealthAmount -= amount;
            FillImage.fillAmount -= amount;
        }
        CheckHealth();
    }


    //////////////////////////////
    [PunRPC] 
    public void RaiseHealth(float amount)
    {
        ModifyRaiseHealth(amount);
    }

    

    private void ModifyRaiseHealth(float amount)
    {
        if(photonView.IsMine)
        {
            HealthAmount += amount;
            FillImage.fillAmount += amount/100;
        }
        else
        {
            HealthAmount += amount;
            FillImage.fillAmount += amount/100;
        }
    }
    


}
