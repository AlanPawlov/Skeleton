using Leopotam.EcsLite;

public class ClearReadyToAttackSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _attackTimerReadyFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _attackTimerReadyFilter = _world.Filter<ReadyToAttack>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var inputPool = _world.GetPool<InputComponent>();
        var readyToAttackPool = _world.GetPool<ReadyToAttack>();
        foreach (var e in _attackTimerReadyFilter)
        {
            var input = inputPool.Get(e);
            if (!input.Shot)
            {
                readyToAttackPool.Del(e);
            }
        }
    }
}
