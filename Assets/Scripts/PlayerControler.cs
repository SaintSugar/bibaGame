using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControler : MonoBehaviour
{
    public float Acceleration;
    public bool Gravity;
    // Start is called before the first frame update
    void Start()
    {
        Gravity = GetComponent<Rigidbody2D>().gravityScale != 0;
    }

    // Update is called once per frame





    void Update()
    {
        if (GetComponent<Rigidbody2D>().position.y < 0) {
            GetComponent<Rigidbody2D>().position = new Vector2(GetComponent<Rigidbody2D>().position.x, 0);
        }

        float x,y;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        
        if (Gravity) {
            y = 0;
        }

        
        Vector2 contr = new Vector2(x, y);
        //GetComponent<Rigidbody2D>().velocity += contr*Acceleration;
        GetComponent<Rigidbody2D>().AddForce(contr*Acceleration);
    }







}
