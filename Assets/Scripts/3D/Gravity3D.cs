using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity3D : MonoBehaviour
{
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;
 
    Rigidbody Rig;
 
    void OnEnable ()
        {
        Rig = GetComponent<Rigidbody>();
        Rig.useGravity = false;
        }
 
    void FixedUpdate ()
        {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        Rig.AddForce(gravity, ForceMode.Acceleration);
        }

}
