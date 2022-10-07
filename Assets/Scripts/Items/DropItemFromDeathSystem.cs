using UnityEngine;
using Leopotam.EcsLite;

public class DropItemFromDeathSystem : IEcsInitSystem, IEcsRunSystem
{
    private const string ResourcePath = "ChestTreasure01";
    private GameObject _prefab;
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _prefab = Resources.Load<GameObject>(ResourcePath);
        _world = systems.GetWorld();
        _filter = _world.Filter<DropItemFromDeath>().Inc<TransformComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var transformPool = _world.GetPool<TransformComponent>();
        var dropItemPool = _world.GetPool<DropItemFromDeath>();

        foreach (var e in _filter)
        {
            var deathTransform = transformPool.Get(e);
            Object.Instantiate(_prefab, deathTransform.Transform.position, deathTransform.Transform.rotation);
            dropItemPool.Del(e);
        }
    }
}
