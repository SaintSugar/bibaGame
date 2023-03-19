using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerControler3D : MonoBehaviour
{
    public float Speed;
    public float Acceleration;
    private bool Gravity;
    public float JumpForce;
    public float DashForce;

    public float[] ForcePull;
    public PlatformerSwither PlatformerCamera;
 
    void Start()
    {
        ForcePull = new float[3];
        ForcePull[0] = 0;
        ForcePull[1] = 0;
        ForcePull[2] = 0;
        Rig = GetComponent<Rigidbody>();
    }

    private bool JumpControlPrevState = false;
    private Rigidbody Rig;
    void Update()
    {
        //if (Input.GetAxis("Fly") != 0 && ! FlyControlPrevState) 
        //    Gravity = !Gravity;
        Gravity = PlatformerCamera.Gravity;

        Vector3 current_velocity = Rig.velocity;
        Vector3 new_velocity = Rig.velocity;
        Vector2 control = new Vector2(0,0);
        RigidbodyConstraints constraints = RigidbodyConstraints.FreezeRotation;
        switch (PlatformerCamera.direction) {
            case 0:
                control = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                break;
            case 1:
                control = new Vector2(-Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
                constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                break;
            case 2:
                control = new Vector2(-Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"));
                constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                break;
            case 3:
                control = new Vector2(Input.GetAxis("Vertical"), -Input.GetAxis("Horizontal"));
                constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                break;
            
        }

        if (Gravity) {
            Rig.constraints = constraints;
            if (isGrounded() && JumpMachine > 0 && Mathf.Abs(Rig.velocity.y) < 10)
                JumpMachine = 0;
            if (Input.GetAxis("Jump") != 0 && JumpMachine < JumpAmount && (JumpMachine == 0 || !JumpControlPrevState)) {
                    new_velocity.y = JumpForce;
                    JumpMachine++;
            }
        }
        else {
            Rig.constraints = RigidbodyConstraints.FreezeRotation;
            if (Input.GetAxis("Jump") != 0 &&!JumpControlPrevState) {
                    //new_velocity.z = JumpForce;
                    Rig.AddForce(new Vector3(control.x, 0, control.y).normalized * DashForce, ForceMode.Impulse);
            }
        }
        new_velocity.x = speedCheck(current_velocity.x, control.x, new_velocity.x, 0);
        new_velocity.z = speedCheck(current_velocity.z, control.y, new_velocity.z, 1);
        JumpControlPrevState = Input.GetAxis("Jump") != 0;
        Rig.velocity = new_velocity;
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
            //Rig.AddForce(new Vector2(Acceleration * control * (Mathf.Abs(axis - 1)), Acceleration * control * axis));
            new_velocity -= Acceleration * ForcePull[axis] * Mathf.Abs(control);
                
                
        return new_velocity;
    }
    
    bool isGrounded() {
        bool onGround = Physics.OverlapSphere(GetComponent<SphereCollider>().center + Rig.position, GetComponent<SphereCollider>().radius).Length > 3;
        return onGround;
    }

    private int JumpMachine = 0;
    public int JumpAmount;

}