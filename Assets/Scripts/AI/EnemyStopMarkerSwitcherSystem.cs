using Leopotam.EcsLite;
using UnityEngine;

public class EnemyStopMarkerSwitcherSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var enemyFilter = world.Filter<HumanoidMovementComponent>().Inc<EnemyTag>()
            .Inc<TransformComponent>().End();
        var playerFilter = world.Filter<PlayerTag>().Inc<TransformComponent>().End();
        var transformPool = world.GetPool<TransformComponent>();
        var stopMarkerPool = world.GetPool<StopMarker>();

        foreach (var enemyEntity in enemyFilter)
        {
            var enemyTransform = transformPool.Get(enemyEntity).Transform;

            foreach (var playerEntity in playerFilter)
            {
                var playerTransform = transformPool.Get(playerEntity).Transform;
                var hasStopMarker = stopMarkerPool.Has(enemyEntity);
                var isStopDistanceEnter = Vector3.Distance(playerTransform.position, enemyTransform.position) < 1.5f;
                if (hasStopMarker)
                {
                    if (!isStopDistanceEnter)
                    {
                        stopMarkerPool.Del(enemyEntity);
                    }
                }
                else if (isStopDistanceEnter)
                {
                    stopMarkerPool.Add(enemyEntity);
                }
            }
        }
    }
}
