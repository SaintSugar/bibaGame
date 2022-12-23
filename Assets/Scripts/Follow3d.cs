using UnityEngine;
using System;
 

public class Follow3d : MonoBehaviour {

    public float Radius;
    public float Elasticity;
    public bool InfititeElasticity;
    public float Heidth;
    public Transform TargetObject;
    void start() {
    }
    void FixedUpdate () {
        Vector3 Watching = transform.forward.normalized * Heidth;
        Vector3 toObject = transform.position - TargetObject.position;
        
        Vector3 DeltaHidth = Vector3.Project(toObject, Watching);
        Vector3 DeltaRadius = Vector3.ProjectOnPlane(toObject, Watching);

        Vector3 V = Vector3.zero;

        if (DeltaRadius.magnitude > Radius)
            V -= DeltaRadius - DeltaRadius.normalized * Radius;

        V -= DeltaHidth + Watching;

        if (InfititeElasticity) {
            transform.position += V;
        }
        else {
            transform.position += V * (Mathf.Exp(-1/(Elasticity + 0.001f)));
        }




    }
}
