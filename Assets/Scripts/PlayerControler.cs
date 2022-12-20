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

    private bool JumpControlPrevState;
    // Update is called once per frame
    void Update()
    {
        RgBodySetup();
        float x,y;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (Gravity) {
            y = GetComponent<Rigidbody2D>().velocity.y;
            canJump();
            if (Input.GetAxis("Jump") != 0 && canJump() && (JumpMachine == 0 || !JumpControlPrevState)) {
                
                    y = JumpForce;
                    //GetComponent<Rigidbody2D>().AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
                    JumpMachine++;
                
            }
        }
        if (Input.GetAxis("Jump") == 0) {
            JumpControlPrevState = false;
        }
        else {
            JumpControlPrevState = true;
        }

        Vector2 control = new Vector2(x, y);

        Vector2 current_velocity = GetComponent<Rigidbody2D>().velocity;

        Vector2 new_velocity = GetComponent<Rigidbody2D>().velocity;

        new_velocity.x = speedCheck(current_velocity.x, control.x, new_velocity.x, 0);
        if (!Gravity){
            new_velocity.y = speedCheck(current_velocity.y, control.y, new_velocity.y, 1);
        }
        else
            new_velocity.y = y;

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
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(Acceleration * control * (Mathf.Abs(axis - 1)), Acceleration * control * axis));
                }
                
        }
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

    bool canJump() {
        if (isGrounded() && JumpMachine > 0 && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.y) < 10){
            JumpMachine = 0;
            return false;
        }
        else if (JumpMachine < JumpAmount) {
            return true;
        }
        return false;
    }

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