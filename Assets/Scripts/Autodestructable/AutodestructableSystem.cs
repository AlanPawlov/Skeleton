using Leopotam.EcsLite;
using System;
using UnityEngine;

public class AutodestructableSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var autodestructableComponentFilter = world.Filter<Autodestructable>().End();
        var autodestructablePool = world.GetPool<Autodestructable>();
        foreach (var autodestructableEntity in autodestructableComponentFilter)
        {
            ref var autodestructable = ref autodestructablePool.Get(autodestructableEntity);
            var canDestroy = autodestructable.LifeTime < (DateTime.Now - autodestructable.SpawnTime).TotalSeconds;
            if (canDestroy)
            {
                UnityEngine.Object.Destroy(autodestructable.GameOject);
                world.DelEntity(autodestructableEntity);
            }
        }
    }
}
