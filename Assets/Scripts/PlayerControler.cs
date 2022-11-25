using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControler : MonoBehaviour
{
    public float Speed;
    public float Acceleration;
    public bool Gravity;
    public float JumpForce;

    private float[] ForcePull;
    // Start is called before the first frame update
    void Start()
    {
        ForcePull = new float[2];
        ForcePull[0] = 0;
        ForcePull[1] = 0;
        //Gravity = GetComponent<Rigidbody2D>().gravityScale != 0;
    }

    // Update is called once per frame
    void Update()
    {
        float x,y;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Gravity) {
            y = 0;
            if (Input.GetAxis("Jump") != 0 && isGrounded()) {
                //GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpForce));
                y = JumpForce;
            }
        }

        Vector2 control = new Vector2(x, y);

        Vector2 current_velocity = GetComponent<Rigidbody2D>().velocity;

        Vector2 new_velocity = GetComponent<Rigidbody2D>().velocity;

        new_velocity.x = speedCheck(current_velocity.x, control.x, new_velocity.x, 0);
        new_velocity.y = speedCheck(current_velocity.y, control.y, new_velocity.y, 1);

        GetComponent<Rigidbody2D>().velocity = new_velocity;
    }

    float speedCheck(float current_velocity, float control, float new_velocity, int axis) {
        if ((Mathf.Abs(current_velocity) <= Mathf.Abs(control) * Speed || control * current_velocity < 0) && ForcePull[axis] == 0) {
            new_velocity = control * Speed;
        }
        else {
                if (ForcePull[axis] == 0 && (Mathf.Abs(current_velocity) > Speed)) {
                    ForcePull[axis] = current_velocity / Mathf.Abs(current_velocity);
                }

                if (ForcePull[axis] * (current_velocity - control * Speed) <= 0 && ForcePull[axis] != 0) {
                    ForcePull[axis] = 0;
                    new_velocity = control * Speed;
                }
                
                
                if (((ForcePull[axis] * (current_velocity - control * Speed) > 0 && ForcePull[axis] * control < 0)) && ForcePull[axis] != 0) {

                    GetComponent<Rigidbody2D>().AddForce(new Vector2(Acceleration * control, 0));
                }
                
        }
        return new_velocity;
    }

    public Transform GroundCheck;
    public LayerMask GroundLayer;
    
    bool isGrounded() {
        bool onGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheck.GetComponent<CircleCollider2D>().radius, GroundLayer);
        return onGround;
    }

}