using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mvBG : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 0.5f;

    [SerializeField]
    private float offset;

    private Vector2 startPosi;

    private float newXPos;
    // Start is called before the first frame update
    void Start()
    {
        startPosi = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newXPos = Mathf.Repeat(Time.time * -moveSpeed, offset);
        transform.position = startPosi + Vector2.right * newXPos;
    }
}
