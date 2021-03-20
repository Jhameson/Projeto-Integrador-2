using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;


public class ItemSpawner : MonoBehaviourPunCallbacks
{
    public GameObject[] itens;
    public GameObject[] spawnPoint;

    private int rand;
    private int randPosition;

    public float startTimeSpawner;
    private float timeSpwans;

    

    private void Start(){
        timeSpwans = startTimeSpawner;
    }
    private void Update(){
        if(timeSpwans <=0){

            SpawnItens();
        }else{
            timeSpwans -= Time.deltaTime;
        }
    }
    [PunRPC]
    private void SpawnItens(){
        rand = Random.Range(0,itens.Length);
        randPosition = Random.Range(0,spawnPoint.Length);


        PhotonNetwork.Instantiate(itens[rand].name, new Vector2(spawnPoint[randPosition].transform.position.x, spawnPoint[randPosition].transform.position.y), Quaternion.identity, 0);
        //PhotonNetwork.Instantiate(PlayerPrefab[id].name, new Vector2(respRange, 4f), Quaternion.identity, 0);
        timeSpwans = startTimeSpawner;
    }
}













// public class ItemSpawner : MonoBehaviour
// {
//     public GameObject[] itens;
//     public Transform[] spawnPoint;

//     private int rand;
//     private int randPosition;

//     public float startTimeSpawner;
//     private float timeSpwans;

    

//     private void Start(){
//         timeSpwans = startTimeSpawner;
//     }
//     private void Update(){
//         if(timeSpwans <=0){

//             rand = Random.Range(0,itens.Length);
//             randPosition = Random.Range(0,spawnPoint.Length);
//             Instantiate(itens[rand], spawnPoint[randPosition].transform.position, Quaternion.identity);
//             timeSpwans = startTimeSpawner;
//         }else{
//             timeSpwans -= Time.deltaTime;
//         }
//     }
// }
