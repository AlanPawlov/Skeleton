using Leopotam.EcsLite;

public class ClearDropItemFromDeathTagSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<NoDropItemFromDeath>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var noDropItemPool = _world.GetPool<NoDropItemFromDeath>();
        foreach (var e in _filter)
        {
            noDropItemPool.Del(e);
        }
    }
}
