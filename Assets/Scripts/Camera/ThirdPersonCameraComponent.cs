using UnityEngine;

[System.Serializable]
public struct ThirdPersonCameraComponent
{
    public Transform CinemachineCameraTarget;
    public Transform CinemachineCameraTransform;
    public float TopClamp;                  //= 70.0f;
    public float BottomClamp;               // = -30.0f;
    public float CameraAngleOverride;
    public float CinemachineTargetYaw;
    public float CinemachineTargetPitch;
    public float Threshold;                 //= 0.01f;
    public bool LockCameraPosition;         // = false;
}
