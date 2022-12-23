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
 
    void Start()
    {
        ForcePull = new float[2];
        ForcePull[0] = 0;
        ForcePull[1] = 0;
    }

    private bool JumpControlPrevState;

    void Update()
    {
        RgBodySetup();

        Vector2 current_velocity = GetComponent<Rigidbody2D>().velocity;
        Vector2 new_velocity = GetComponent<Rigidbody2D>().velocity;
        Vector2 control = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        new_velocity.x = speedCheck(current_velocity.x, control.x, new_velocity.x, 0);

        if (Gravity) {
            control.y = 0;
            if (isGrounded() && JumpMachine > 0 && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 10)
                JumpMachine = 0;
            if (Input.GetAxis("Jump") != 0 && JumpMachine < JumpAmount && (JumpMachine == 0 || !JumpControlPrevState)) {
                    new_velocity.y = JumpForce;
                    JumpMachine++;
            }
            if (Input.GetAxis("Jump") == 0) {
                JumpControlPrevState = false;
            }
            else {
                JumpControlPrevState = true;
            }
        }
        else
            new_velocity.y = speedCheck(current_velocity.y, control.y, new_velocity.y, 1);

        GetComponent<Rigidbody2D>().velocity = new_velocity;
    }

    float speedCheck(float current_velocity, float control, float new_velocity, int axis) {
        if (ForcePull[axis] == 0 && (Mathf.Abs(current_velocity) > Speed)) {
                    ForcePull[axis] = current_velocity / Mathf.Abs(current_velocity);
                }
        else if (ForcePull[axis] * (current_velocity - control * Speed) <= 0 && ForcePull[axis] != 0) {
                    ForcePull[axis] = 0;
                    new_velocity = control * Speed;
                }

        if ((Mathf.Abs(current_velocity) <= Mathf.Abs(control) * Speed || control * current_velocity < 0) && ForcePull[axis] == 0) {
            new_velocity = control * Speed;
        }
        else if (((ForcePull[axis] * (current_velocity - control * Speed) > 0 && ForcePull[axis] * control < 0)) && ForcePull[axis] != 0) 
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Acceleration * control * (Mathf.Abs(axis - 1)), Acceleration * control * axis));
                
                
        return new_velocity;
    }

    //public Transform GroundCheck;
    public LayerMask GroundLayer;
    
    bool isGrounded() {
        bool onGround = Physics2D.OverlapCircle(GetComponent<CircleCollider2D>().offset + GetComponent<Rigidbody2D>().position, GetComponent<CircleCollider2D>().radius, GroundLayer);
        return onGround;
    }

    private int JumpMachine = 0;
    public int JumpAmount;
    public float gravityScale;
    public float LinearDragPlatformer;
    public float LinearDragIsometric;

    void RgBodySetup() {
        if (Gravity) {
            GetComponent<Rigidbody2D>().gravityScale = gravityScale;
            GetComponent<Rigidbody2D>().drag = LinearDragPlatformer;
        }
        else {
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().drag = LinearDragIsometric;
        }
    }

}