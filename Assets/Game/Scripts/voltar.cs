using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voltar : MonoBehaviour
{
    public GameObject painel;
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape)) {
           painel.SetActive(false);
       }
    }
}
