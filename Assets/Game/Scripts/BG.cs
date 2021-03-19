using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BG : MonoBehaviour
{
    private Material mat;
    private GameObject player;
    public float speed = 0;
    private float pos = 0;

    void start()
    {
        mat = GetComponent<Renderer>().material;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Update is called once per frame
    void Update()
    {
        var vel = player.GetComponent<Rigidbody2D>().velocity.x;

        if (vel != 0)
        {
            var side = player.transform.localScale.x;
            pos += speed * side;
            mat.mainTextureOffset = new Vector2(pos, 0);
        }
    }
}

