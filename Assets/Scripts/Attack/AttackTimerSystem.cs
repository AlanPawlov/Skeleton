using System;
using Leopotam.EcsLite;

public class AttackTimerSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var sharedData = systems.GetShared<ECSSharedData>();
        if (sharedData.IsPause||sharedData.IsPlayerDeath)
        {
            return;
        }

        var world = systems.GetWorld();
        var filter = world.Filter<AttackTimer>().Exc<AttackTimerReady>()
            .Exc<Death>().End();
        var timerPool = world.GetPool<AttackTimer>();
        var completeTimerPool = world.GetPool<AttackTimerReady>();

        foreach (var timerEntity in filter)
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
