using UnityEngine;
using Leopotam.EcsLite;

public class DropItemFromDeathInitSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<Death>().Exc<DropItemFromDeath>()
            .Exc<NoDropItemFromDeath>().Exc<PlayerTag>().End();
        var dropItemPool = world.GetPool<DropItemFromDeath>();
        var noDropItemPool = world.GetPool<NoDropItemFromDeath>();

        foreach (var e in filter)
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
