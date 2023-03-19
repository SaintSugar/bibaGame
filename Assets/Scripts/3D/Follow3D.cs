using UnityEngine;
using System;
 

public class Follow3D : MonoBehaviour {
    public float Radius;
    public float Elasticity;
    public bool InfititeElasticity;
    public float Heigth;
    public Transform TargetObject;
    [SerializeField]
    private float deltaHeight;
    void start() {
    }
    void FixedUpdate () {
        Vector3 center = transform.position + transform.forward * (Heigth - deltaHeight);
        Vector3 targetPos = TargetObject.position + new Vector3(0, deltaHeight, 0);

        if ((center - targetPos).magnitude > Radius) {
            Vector3 nearest = (targetPos - center).normalized * Radius * 0.98f + center;
            Vector3 delta = targetPos - nearest;
            float Speed = Mathf.Pow((nearest - targetPos).magnitude, 2) / 1000 * Elasticity;
            if (InfititeElasticity) {
                transform.position += delta;
            }
            else {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + delta, Speed);
            }
        }
    }
}
