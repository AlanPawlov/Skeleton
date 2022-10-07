using Leopotam.EcsLite;
using UnityEngine;

public class FollowToPlayerSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _followersFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _followersFilter = _world.Filter<HumanoidMovementComponent>().Inc<EnemyTag>()
            .Inc<TransformComponent>().Inc<InputComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {

        var inputPool = _world.GetPool<InputComponent>();

        foreach (var enemyEntity in _followersFilter)
        {
            ref var input = ref inputPool.Get(enemyEntity);
            input.Move = Vector2.up;
        }
    }
}
