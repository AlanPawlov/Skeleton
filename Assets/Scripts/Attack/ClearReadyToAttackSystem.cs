using Leopotam.EcsLite;

public class ClearReadyToAttackSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var attackTimerReadyFilter = world.Filter<ReadyToAttack>().End();
        var inputPool = world.GetPool<InputComponent>();
        var readyToAttackPool = world.GetPool<ReadyToAttack>();
        foreach (var e in attackTimerReadyFilter)
        {
            var input = inputPool.Get(e);
            if (!input.Shot)
            {
                readyToAttackPool.Del(e);
            }
        }
    }
}
