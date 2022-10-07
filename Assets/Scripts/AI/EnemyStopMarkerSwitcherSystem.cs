using Leopotam.EcsLite;
using UnityEngine;

public class EnemyStopMarkerSwitcherSystem : IEcsRunSystem, IEcsInitSystem
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
        var transformPool = _world.GetPool<TransformComponent>();
        var stopMarkerPool = _world.GetPool<StopMarker>();

        foreach (var enemyEntity in _enemyFilter)
        {
            var enemyTransform = transformPool.Get(enemyEntity).Transform;

            foreach (var playerEntity in _playerFilter)
            {
                var playerTransform = transformPool.Get(playerEntity).Transform;
                var hasStopMarker = stopMarkerPool.Has(enemyEntity);
                var isStopDistanceEnter = (playerTransform.position - enemyTransform.position).sqrMagnitude < 1.5f * 1.5f;
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
