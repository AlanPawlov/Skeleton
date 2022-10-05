using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovementSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<InputComponent>().Inc<StopMarker>().End();
        var inputPool = world.GetPool<InputComponent>();

        foreach (var enemyEntity in filter)
        {
            ref var input = ref inputPool.Get(enemyEntity);
            input.Move = Vector2.zero;
        }
    }
}
