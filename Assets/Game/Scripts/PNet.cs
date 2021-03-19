using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PNet : MonoBehaviour
{
    public MonoBehaviour[] scriptsToIgnore;
    private PhotonView pV;
    // Start is called before the first frame update
    void Start()
    {
        pV = GetComponent<PhotonView>();
        if (!pV.IsMine)
        {
            foreach (var script in scriptsToIgnore)
            {
                script.enabled = false;
            }
        }
    }

  
}
