using System.Reflection;
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
    private bool [] RotationControlPrevState = {false, false};
    [SerializeField]
    private float Speed = 1;
    public bool Gravity = false;
    [SerializeField]
    public int direction = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Fly") != 0 && ! FlyControlPrevState) 
            Gravity = !Gravity;
        FlyControlPrevState = Input.GetAxis("Fly") != 0;

        if (Input.GetAxis("RotateRight") != 0 && !RotationControlPrevState[0]) {
            direction--;
            if (direction < 0) direction = 3;
        }
        RotationControlPrevState[0] = Input.GetAxis("RotateRight") != 0;

        if (Input.GetAxis("RotateLeft") != 0 && !RotationControlPrevState[1]) {
            direction++;
            if (direction > 3) direction = 0;
        }
        RotationControlPrevState[1] = Input.GetAxis("RotateLeft") != 0;


        Vector3 targetAngle = new Vector3(0, 0, 0);
        if (Gravity) {
            //transform.eulerAngles = new Vector3(0.01f, 0, 0);
            targetAngle.x = 0;
        }
        else {
            //transform.eulerAngles = new Vector3(90, 0, 0);
            targetAngle.x = 90f;
        }
        switch(direction){
            case 0:
                targetAngle.y = 0;
                targetAngle.z = 0;
                break;
            case 1:
                targetAngle.y = -90f;
                targetAngle.z = 0;
                break;
            case 2:
                targetAngle.y = 180f;
                targetAngle.z = 0;
                break;
            case 3:
                targetAngle.y = 90f;
                targetAngle.z = 0;
                break;
        }
        Quaternion qTarget = Quaternion.Euler(targetAngle.x, targetAngle.y, targetAngle.z);
        rotation(qTarget);
    }
    void rotation(Quaternion Target) {
        Vector3 Current = transform.eulerAngles;
        //Target = Target/180*Mathf.PI;
        //Vector3 rotationSpeed = (Target - Current) * Speed;
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Target, Speed);
        //transform.rotation = Target;
        
        //transform.eulerAngles = Vector3.RotateTowards(Current, Target, Speed, Speed);
        //if ((Target - Current).magnitude < 1.5 * rotationSpeed.magnitude)
        //    transform.eulerAngles = Target;
        //else
        //    transform.eulerAngles += rotationSpeed;
    }
}
