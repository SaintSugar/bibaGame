using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizeToObject : MonoBehaviour
{
    public Transform TargetingObject;
    void Update()
    {
        transform.eulerAngles = TargetingObject.eulerAngles;
    }
}
