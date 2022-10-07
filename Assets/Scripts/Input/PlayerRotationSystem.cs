using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotationSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<HumanoidMovementComponent>().Inc<ThirdPersonCameraComponent>()
            .Inc<TransformComponent>().Inc<InputComponent>().Inc<PlayerTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var inputPool = _world.GetPool<InputComponent>();
        var movementPool = _world.GetPool<HumanoidMovementComponent>();
        var cameraPool = _world.GetPool<ThirdPersonCameraComponent>();
        var transformPool = _world.GetPool<TransformComponent>();

        foreach (var entity in _filter)
        {
            var input = inputPool.Get(entity);
            ref var movement = ref movementPool.Get(entity);
            ref var camera = ref cameraPool.Get(entity);
            ref var transform = ref transformPool.Get(entity);

            Rotate(input, ref movement, ref transform, camera.CinemachineCameraTransform.transform.eulerAngles.y);
        }
    }

    private void Rotate(InputComponent input, ref HumanoidMovementComponent movement, ref TransformComponent transform, float yAngle)
    {
        if (!(input.Move == Vector2.zero))
        {
            movement.TargetRotation = Mathf.Atan2(input.Move.x, input.Move.y) *
                Mathf.Rad2Deg + yAngle;

            float rotation = Mathf.SmoothDampAngle(transform.Transform.eulerAngles.y,
                movement.TargetRotation, ref movement.RotationVelocity, movement.RotationSmoothTime);

            transform.Transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }
}
