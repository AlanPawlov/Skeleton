using Leopotam.EcsLite;
using UnityEngine;

public class EnemyRotationSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var enemyFilter = world.Filter<HumanoidMovementComponent>().Inc<EnemyTag>()
            .Inc<TransformComponent>().End();
        var playerFilter = world.Filter<PlayerTag>().Inc<TransformComponent>().End();

        var movementPool = world.GetPool<HumanoidMovementComponent>();
        var transformPool = world.GetPool<TransformComponent>();

        foreach (var enemyEntity in enemyFilter)
        {
            foreach (var playerEntity in playerFilter)
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
