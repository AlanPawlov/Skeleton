using UnityEngine;
using Leopotam.EcsLite;

public class ThirdPersonCameraSystem : IEcsRunSystem
{
    //private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var cameraComponentFilter = world.Filter<ThirdPersonCameraComponent>().End();
        var cameraPool = world.GetPool<ThirdPersonCameraComponent>();
        var inputComponentFilter = world.Filter<InputComponent>().End();
        var inputPool = world.GetPool<InputComponent>();

        foreach (var e in inputComponentFilter)
        {
            var input = inputPool.Get(e);

            foreach (var camera in cameraComponentFilter)
            {
                ref var cameraComponent = ref cameraPool.Get(camera);
                if (input.Look.sqrMagnitude >= cameraComponent.Threshold && !cameraComponent.LockCameraPosition)
                {
                    float deltaTimeMultiplier = 1.0f;
                    cameraComponent.CinemachineTargetYaw += input.Look.x * deltaTimeMultiplier;
                    cameraComponent.CinemachineTargetPitch += input.Look.y * deltaTimeMultiplier;
                }

                cameraComponent.CinemachineTargetYaw = ClampAngle(cameraComponent.CinemachineTargetYaw, float.MinValue, float.MaxValue);
                cameraComponent.CinemachineTargetPitch = ClampAngle(cameraComponent.CinemachineTargetPitch, cameraComponent.BottomClamp, cameraComponent.TopClamp);
                cameraComponent.CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cameraComponent.CinemachineTargetPitch + cameraComponent.CameraAngleOverride, cameraComponent.CinemachineTargetYaw, 0.0f);
            }
        }
    }

    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
        {
            lfAngle += 360f;
        }
        if (lfAngle > 360f)
        {
            lfAngle -= 360f;
        }

        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
