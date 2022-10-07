using Leopotam.EcsLite;
using System;
using UnityEngine;

public class AutodestructableSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _autodestructableComponentFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _autodestructableComponentFilter = _world.Filter<Autodestructable>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var autodestructablePool = _world.GetPool<Autodestructable>();
        foreach (var autodestructableEntity in _autodestructableComponentFilter)
        {
            ref var autodestructable = ref autodestructablePool.Get(autodestructableEntity);
            var canDestroy = autodestructable.LifeTime < (DateTime.Now - autodestructable.SpawnTime).TotalSeconds;
            if (canDestroy)
            {
                UnityEngine.Object.Destroy(autodestructable.GameOject);
                _world.DelEntity(autodestructableEntity);
            }
        }
    }
}
