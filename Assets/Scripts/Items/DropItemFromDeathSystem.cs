using UnityEngine;
using Leopotam.EcsLite;

public class DropItemFromDeathSystem : IEcsInitSystem, IEcsRunSystem
{
    private const string ResourcePath = "ChestTreasure01";
    private GameObject _prefab;
    public void Init(IEcsSystems systems)
    {
        _prefab = Resources.Load<GameObject>(ResourcePath);
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<DropItemFromDeath>().Inc<TransformComponent>().End();
        var transformPool = world.GetPool<TransformComponent>();
        var dropItemPool = world.GetPool<DropItemFromDeath>();

        foreach (var e in filter)
        {
            var deathTransform = transformPool.Get(e);
            Object.Instantiate(_prefab, deathTransform.Transform.position, deathTransform.Transform.rotation);
            dropItemPool.Del(e);
        }
    }
}
