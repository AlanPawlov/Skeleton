using System;

using Leopotam.EcsLite;

public class AttackTimerSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var playerFilter = world.Filter<AttackTimer>().Exc<ReadyToAttack>().End();
        var timerPool = world.GetPool<AttackTimer>();
        var completeTimerPool = world.GetPool<ReadyToAttack>();

        foreach (var timerEntity in playerFilter)
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
