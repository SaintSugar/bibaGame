using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MaxSize;
    public float MinSize;
    public float SizeStep;
    public bool ScaleCameraRadius;
    public CameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private int CameraStepScaleControlerLastState = 0;
    private bool CameraMove = false;
    private bool CameraMoveButtonLastState = false;
    // Update is called once per frame
    void Update()
    {
        float CameraStepScaleControler = Input.GetAxis("Camera Scale");
        if (CameraStepScaleControlerLastState == 0) {
            if (CameraStepScaleControler > 0 && GetComponent<Camera>().orthographicSize < MaxSize) {
                if (ScaleCameraRadius)
                    CameraRadiusScaler(SizeStep);
                GetComponent<Camera>().orthographicSize += SizeStep;
            }
            else if (CameraStepScaleControler < 0 && GetComponent<Camera>().orthographicSize > MinSize) {
                if (ScaleCameraRadius)
                    CameraRadiusScaler(-SizeStep);
                GetComponent<Camera>().orthographicSize -= SizeStep;
            }
        }
        if (CameraStepScaleControler > 0)
            CameraStepScaleControlerLastState = 1;
        else if (CameraStepScaleControler < 0)
            CameraStepScaleControlerLastState = -1;
        else if (CameraStepScaleControler == 0)
            CameraStepScaleControlerLastState = 0;
        
        if (Input.GetAxis("Camera Movement Unlock") != 0 && !CameraMoveButtonLastState)
            CameraMove = !CameraMove;
        CameraMoveButtonLastState = Input.GetAxis("Camera Movement Unlock") != 0;

        if (CameraMove) {
            Vector2 control = new Vector2(Input.GetAxis("Camera Horizontal"), Input.GetAxis("Camera Vertical"));
        }

    }
    void CameraRadiusScaler(float deltaSize) {
        float R = GetComponent<Follow>().Radius;
        float K = GetComponent<Camera>().orthographicSize / (GetComponent<Camera>().orthographicSize + deltaSize);

        R = R / K;
        GetComponent<Follow>().Radius = R;
    }
}
