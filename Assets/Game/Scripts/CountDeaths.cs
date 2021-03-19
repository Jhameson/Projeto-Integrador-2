using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
public class CountDeaths: MonoBehaviourPunCallbacks
{
   
    Text sco;
    // Start is called before the first frame update
    void Start()
    {
        sco = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        sco.text = "Score: " + PhotonNetwork.LocalPlayer.GetScore();
    }
}
