using UnityEngine;
using Leopotam.EcsLite;

public class ThirdPersonCameraSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _cameraComponentFilter;
    private EcsFilter _inputComponentFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _cameraComponentFilter = _world.Filter<ThirdPersonCameraComponent>().End();
        _inputComponentFilter = _world.Filter<InputComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var cameraPool = _world.GetPool<ThirdPersonCameraComponent>();
        var inputPool = _world.GetPool<InputComponent>();

        foreach (var e in _inputComponentFilter)
        {
            var input = inputPool.Get(e);

            foreach (var camera in _cameraComponentFilter)
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
