using Leopotam.EcsLite;
using UnityEngine;

public class FollowToPlayerSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var followersFilter = world.Filter<HumanoidMovementComponent>().Inc<EnemyTag>()
            .Inc<TransformComponent>().Inc<InputComponent>().End();
        var inputPool = world.GetPool<InputComponent>();

        foreach (var enemyEntity in followersFilter)
        {
            ref var input = ref inputPool.Get(enemyEntity);
            input.Move = Vector2.up;
        }
    }
}
