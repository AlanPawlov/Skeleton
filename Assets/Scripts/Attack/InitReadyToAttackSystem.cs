using Leopotam.EcsLite;

public class InitReadyToAttackSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var attackTimerReadyFilter = world.Filter<AttackTimerReady>().Inc<InputComponent>()
            .Exc<ReadyToAttack>().End();
        var inputPool = world.GetPool<InputComponent>();
        var readyToAttackPool = world.GetPool<ReadyToAttack>();
        foreach (var e in attackTimerReadyFilter)
        {
            var input = inputPool.Get(e);
            if (input.Shot)
            {
                readyToAttackPool.Add(e);
            }
        }
    }
}
