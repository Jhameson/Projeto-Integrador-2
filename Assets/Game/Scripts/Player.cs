using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


public class Player : MonoBehaviourPunCallbacks
{
    [Header("Parâmetros Player")]
    public float MoveSpeed;
    public float JumpForce;
    private bool isJumping;
    private bool doubleJumping;
    public Text PlayerNameText;

    //public Text textPlayerName;

    [Header("Componentes")]
    public PhotonView pV;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;

    [Header("Projeteis")]
    public GameObject BulletObject;
    public GameObject BulletObjectLeft;
    public Transform FirePos;
    public Transform FirePosLeft;   

    public bool DisableInput = false;
    public bool isGrounded = false;

    [Header("Buffs")]
    /////////////itens
    public GameObject escudo;
    public float tempoEscudo; //tempo do escudo
    public float tempoVelocidade; //tempo de velocidade
    public float tempoJump;

    private bool RunEscudo = false;
    private bool RunVelocidade = false;
    private bool RunJump = false;

    // public BoxCollider2D bcr;

    private void Awake()
    {
        if (pV.IsMine)
        {
            PlayerCamera.SetActive(true);

            PlayerNameText.text = PhotonNetwork.NickName ;
            
        }
        else
        {
            PlayerNameText.text = pV.Owner.NickName ;
            PlayerNameText.color = Color.red;
        }
        
    }
    
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (photonView.IsMine && !DisableInput)
        {
            Moviment();
            Jump();

        }
        if(RunEscudo){
            contagemEscudo();
        }
        if(RunVelocidade){
            contagemVelocidade();
        }
        if(RunJump){
            contagemJump();
        }

    }

    [PunRPC]
    public void EnableShield(bool valor)
     {
         RunEscudo = true;        
         escudo.SetActive(valor);
         Bullet.imuneColisão = true;
        
    }

    [PunRPC] 
    public void Buffvel(float valor){
        RunVelocidade = true;
        MoveSpeed += valor;
    }
    

    [PunRPC] 
    public void Buffjump(float valor){
        RunEscudo = true;
        JumpForce +=valor;
    }
    


    //////////////////////////////////////////////// CRONOMETRO para itens
     private void contagemVelocidade()
    {
       
        tempoVelocidade -= Time.deltaTime;
        if(tempoVelocidade<=0)
        {
                Buffvel(-2f);
                tempoVelocidade = 5f;
                RunVelocidade = false;
                
                               
        }
    }

    //////////////////////////////////////////////// CRONOMETRO para itens
     private void contagemJump()
    {
        
        tempoJump -= Time.deltaTime;
        if(tempoJump<=0)
        {
                Buffjump(-40f);
                tempoJump = 8f;
                RunJump = false;
                                    
        }
    }

    // //////////////////////////////////////////////// CONTAGEM ESCUDO
    private void contagemEscudo()
    {
        
        tempoEscudo -= Time.deltaTime;
        if(tempoEscudo<=0)
        {
                EnableShield(false);
                RunEscudo = false;
                tempoEscudo= 10f;
                Bullet.imuneColisão = false;
                
        }
    }
    /////////////////////////////////////////////
    private void Shoot()
    {
        if(sr.flipX == false)
        {
        GameObject obj = PhotonNetwork.Instantiate(BulletObject.name, new Vector2(FirePos.transform.position.x, FirePos.transform.position.y), Quaternion.identity, 0);
        }
        

        if(sr.flipX == true)
        {
            GameObject obj = PhotonNetwork.Instantiate(BulletObjectLeft.name, new Vector2(FirePosLeft.transform.position.x, FirePos.transform.position.y), Quaternion.identity, 0);
            
            obj.GetComponent<PhotonView>().RPC("ChangeDir_left",RpcTarget.AllBuffered);
        }
        
        anim.SetTrigger("shootTrigger");
    }
    

    

    private void Moviment()
    {
        var move = new Vector3 (Input.GetAxisRaw("Horizontal"),0);

        transform.position += move * MoveSpeed * Time.deltaTime;
        //tiro
        if(Input.GetKeyDown(KeyCode.P))
        {
            Shoot();
        }
        //dir
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown("left"))
        {
            photonView.RPC("FlipTrue", Photon.Pun.RpcTarget.AllBuffered);
        }
        //esq
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown("right"))
        {

            photonView.RPC("FlipFalse", Photon.Pun.RpcTarget.AllBuffered);
        }
        

        
        if (((Input.GetKey(KeyCode.A) || Input.GetKey("right")) || (Input.GetKey(KeyCode.D) || Input.GetKey("left"))) && !isJumping)
        {
            anim.SetBool("RUN", true);
        }
        else
        {
            anim.SetBool("RUN", false);
        }
        
       
    }

    [PunRPC]
    private void FlipTrue()
    {
        sr.flipX = true;
    }
    [PunRPC]
    private void FlipFalse()
    {
        sr.flipX = false;
    }

    void Jump()
    {

       
        if (Input.GetButtonDown("Jump"))
        {
            anim.SetTrigger("JUMP");
            if (!isJumping)
            {
                rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
                
                isJumping = true;
            } 
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            isJumping = false;
        }
    }
     
}