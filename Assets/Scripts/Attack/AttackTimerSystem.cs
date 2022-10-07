using System;
using Leopotam.EcsLite;

public class AttackTimerSystem : IEcsRunSystem, IEcsInitSystem
{
    private ECSSharedData _sharedData;
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _sharedData = systems.GetShared<ECSSharedData>();
        _world = systems.GetWorld();
        _filter = _world.Filter<AttackTimer>().Exc<AttackTimerReady>()
                .Exc<Death>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (_sharedData.IsPause || _sharedData.IsPlayerDeath)
        {
            return;
        }

        var timerPool = _world.GetPool<AttackTimer>();
        var completeTimerPool = _world.GetPool<AttackTimerReady>();
        foreach (var timerEntity in _filter)
        {
            ref var timer = ref timerPool.Get(timerEntity);
            var canAttack = timer.AttackInterval < (DateTime.Now - timer.LastAttackTime).TotalSeconds;
            if (canAttack)
            {
                completeTimerPool.Add(timerEntity);
            }
        }
    }
}
