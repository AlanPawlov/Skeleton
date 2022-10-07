using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovementSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<InputComponent>().Inc<StopMarker>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var inputPool = _world.GetPool<InputComponent>();

        foreach (var enemyEntity in _filter)
        {
            ref var input = ref inputPool.Get(enemyEntity);
            input.Move = Vector2.zero;
        }
    }
}
