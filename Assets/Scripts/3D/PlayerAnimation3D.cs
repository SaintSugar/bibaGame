using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation3D : MonoBehaviour
{   
    // Start is called before the first frame update
    
    // Update is called once per frame

    public AnimationClip StateRun;
    public AnimationClip StateRunSide;
    public AnimationClip StateRunBack;

    public AnimationClip StateIdle;
    public AnimationClip StateIdleSide;
    public AnimationClip StateIdleBack;

    private Animator PlayerAnimator;
    private SpriteRenderer SpRenderer;
    private Rigidbody Rig;
    public float speed;
    public bool Gravity;
    [SerializeField]
    private PlatformerSwither PlatformerCamera;
    
    void Start() {
        PlayerAnimator = GetComponent<Animator>();
        SpRenderer = GetComponent<SpriteRenderer>();
        Rig = GetComponent<Rigidbody>();
    }
    void LateUpdate()
    {
        Vector3 CurrentSpeed = Rig.velocity;
        switch (PlatformerCamera.direction) {
            case 0:
                break;
            case 1:
                CurrentSpeed = new Vector3(CurrentSpeed.z, CurrentSpeed.y, -CurrentSpeed.x);
                break;
            case 2:
                CurrentSpeed = new Vector3(-CurrentSpeed.x, CurrentSpeed.y, -CurrentSpeed.z);
                break;
            case 3:
                CurrentSpeed = new Vector3(-CurrentSpeed.z, CurrentSpeed.y, CurrentSpeed.x);
                break;
            
        }


        
        if (CurrentSpeed.magnitude > speed){
            PlayerAnimator.SetBool("Idle", false);
            if (Mathf.Abs(CurrentSpeed.x) > Mathf.Abs(CurrentSpeed.z) || Gravity) {
                PlayerAnimator.Play(StateRunSide.name);
                if (CurrentSpeed.x < 0)
                    SpRenderer.flipX = true;
                else if (CurrentSpeed.x > 0)
                    SpRenderer.flipX = false;
            }
            else if (Mathf.Abs(CurrentSpeed.x) < Mathf.Abs(CurrentSpeed.z) && !Gravity) {
                if (CurrentSpeed.z < 0) {
                    PlayerAnimator.Play(StateRun.name);
                    SpRenderer.flipX = false;
                }
                else if (CurrentSpeed.z > 0) {
                    PlayerAnimator.Play(StateRunBack.name);
                    SpRenderer.flipX = true;
                }
            }
        }
        else
            PlayerAnimator.SetBool("Idle", true);
    }
}
