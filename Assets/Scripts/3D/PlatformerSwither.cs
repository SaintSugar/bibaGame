using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerSwither : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private bool FlyControlPrevState = false;
    public Vector3 Speed;
    public bool Gravity = false;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Fly") != 0 && ! FlyControlPrevState) 
            Gravity = !Gravity;
        FlyControlPrevState = Input.GetAxis("Fly") != 0;
        if (Gravity) {
            //transform.eulerAngles = new Vector3(0.01f, 0, 0);
            rotation(new Vector3(0.01f, 0, 0), -Speed);
        }
        else {
            //transform.eulerAngles = new Vector3(90, 0, 0);
            rotation(new Vector3(90f, 0, 0), Speed);
        }
    }
    void rotation(Vector3 Target, Vector3 Speed) {
        Vector3 Current = transform.eulerAngles;
        if ((Target - Current).magnitude < Speed.magnitude)
            transform.eulerAngles = Target;
        else
            transform.eulerAngles += Speed;
    }
}
