using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControler : MonoBehaviour
{
    public float Speed;
    public float moveG;
    public float moveV;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveG = Input.GetAxis("Horizontal") * Speed;
        moveV = Input.GetAxis("Vertical") * Speed;
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveG, moveV); 
    }
}
