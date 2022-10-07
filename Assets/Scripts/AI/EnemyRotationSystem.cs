using Leopotam.EcsLite;
using UnityEngine;

public class EnemyRotationSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _enemyFilter;
    private EcsFilter _playerFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _enemyFilter = _world.Filter<HumanoidMovementComponent>().Inc<EnemyTag>()
                             .Inc<TransformComponent>().End();
        _playerFilter = _world.Filter<PlayerTag>().Inc<TransformComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var movementPool = _world.GetPool<HumanoidMovementComponent>();
        var transformPool = _world.GetPool<TransformComponent>();

        foreach (var enemyEntity in _enemyFilter)
        {
            foreach (var playerEntity in _playerFilter)
            {
                var playerTransform = transformPool.Get(playerEntity);
                ref var movement = ref movementPool.Get(enemyEntity);
                ref var transform = ref transformPool.Get(enemyEntity);
                var direction = (playerTransform.Transform.transform.position - transform.Transform.transform.position).normalized;
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

                movement.TargetRotation = angle;
                transform.Transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            }
        }
    }
}
