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
        /*if (GetComponent<Rigidbody2D>().position.y < 0) {
            GetComponent<Rigidbody2D>().position = new Vector2(GetComponent<Rigidbody2D>().position.x, 0);
        }*/

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

        //if (current_velocity.magnitude <= Speed && control.magnitude > 0) {
        //    GetComponent<Rigidbody2D>().velocity = control * Speed;
        //}
        if ((Mathf.Abs(current_velocity.x) <= Mathf.Abs(control.x) * Speed || control.x * current_velocity.x < 0) && ForcePull[0] == 0) {
            new_velocity.x = control.x * Speed;
        }
        else {
                if (ForcePull[0] == 0 && (Mathf.Abs(current_velocity.x) > Speed)) {
                    ForcePull[0] = current_velocity.x / Mathf.Abs(current_velocity.x);
                }

                if (ForcePull[0] * (current_velocity.x - control.x * Speed) <= 0 && ForcePull[0] != 0) {
                    ForcePull[0] = 0;
                    new_velocity.x = control.x * Speed;
                }
                
                
                if (((ForcePull[0] * (current_velocity.x - control.x * Speed) > 0 && ForcePull[0] * control.x < 0)) && ForcePull[0] != 0) {
                    //new_velocity.x += control.x * Acceleration;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(Acceleration * control.x, 0));
                }
                
        }

        if ((Mathf.Abs(current_velocity.y) <= Mathf.Abs(control.y) * Speed || control.y * current_velocity.y < 0) && ForcePull[1] == 0) {
            new_velocity.y = control.y * Speed;
        }
        else {
                if (ForcePull[1] == 0 && (Mathf.Abs(current_velocity.y) > Speed)) {
                    ForcePull[1] = current_velocity.y / Mathf.Abs(current_velocity.y);
                }

                if (ForcePull[1] * (current_velocity.y - control.y * Speed) <= 0 && ForcePull[1] != 0) {
                    ForcePull[1] = 0;
                    new_velocity.y = control.y * Speed;
                }
                
                
                if (((ForcePull[1] * (current_velocity.y - control.y * Speed) > 0 && ForcePull[1] * control.y < 0)) && ForcePull[1] != 0) {

                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Acceleration * control.y));
                }
                
        }

        GetComponent<Rigidbody2D>().velocity = new_velocity;
        //Vector2 contr = new Vector2(x, y) * Speed;
        //GetComponent<Rigidbody2D>().velocity += contr*Acceleration;
        //GetComponent<Rigidbody2D>().velocity += contr;
    }



    public Transform GroundCheck;
    public LayerMask GroundLayer;
    bool isGrounded() {
        bool onGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheck.GetComponent<CircleCollider2D>().radius, GroundLayer);
        return onGround;
    }

}
/*using System.Collections;
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




*/