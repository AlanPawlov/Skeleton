using UnityEngine;
using Leopotam.EcsLite;

public class DropItemFromDeathInitSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<Death>().Exc<DropItemFromDeath>()
            .Exc<NoDropItemFromDeath>().Exc<PlayerTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var dropItemPool = _world.GetPool<DropItemFromDeath>();
        var noDropItemPool = _world.GetPool<NoDropItemFromDeath>();

        foreach (var e in _filter)
        {
            if (RandomDrop())
            {
                dropItemPool.Add(e);
                continue;
            }
            noDropItemPool.Add(e);
        }
    }

    public bool RandomDrop() // TODO: можно добавить параметр для регулирования шанса дропа
    {
        var randomResult = Random.Range(0, 2);
        return randomResult > 0;
    }
}
