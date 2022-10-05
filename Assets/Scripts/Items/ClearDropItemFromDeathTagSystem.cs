using Leopotam.EcsLite;

public class ClearDropItemFromDeathTagSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<NoDropItemFromDeath>().End();
        var noDropItemPool = world.GetPool<NoDropItemFromDeath>();
        foreach (var e in filter)
        {
            noDropItemPool.Del(e);
        }
    }
}
