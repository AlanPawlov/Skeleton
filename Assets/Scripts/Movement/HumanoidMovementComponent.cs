using UnityEngine;

[System.Serializable]

public struct HumanoidMovementComponent
{
    public float MoveSpeed;// = 2.0f;
    public float SprintSpeed;// = 5.335f;
    public float RotationSmoothTime;// = 0.12f;
    public float SpeedChangeRate;// = 10.0f;
    public float JumpHeight;// = 1.2f;
    public float Gravity;// = -15.0f;
    public float JumpTimeout;// = 0.50f;
    public float FallTimeout;// = 0.15f;
    public bool Grounded;// = true;
    public float GroundedOffset;// = -0.14f;
    public float GroundedRadius;// = 0.28f;
    public LayerMask GroundLayers;

    public float Speed;
    public float AnimationBlend;
    public float TargetRotation;// = 0.0f;
    public float RotationVelocity;
    public float VerticalVelocity;
    public float TerminalVelocity;// = 53.0f;
    
    public float JumpTimeoutDelta;
    public float FallTimeoutDelta;
    
    public int AnimIDSpeed;
    public int AnimIDGrounded;
    public int AnimIDJump;
    public int AnimIDFreeFall;
    public int AnimIDMotionSpeed;

    public Animator Animator;
    public CharacterController Controller;
    public bool HasAnimator;
}
