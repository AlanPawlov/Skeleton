using Leopotam.EcsLite;

public class DeathSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var healthFilter = world.Filter<Health>().Inc<TransformComponent>()
            .Exc<Death>().End();
        var healthPool = world.GetPool<Health>();
        var deathPool = world.GetPool<Death>();
        var transformPool = world.GetPool<TransformComponent>();

        foreach (var e in healthFilter)
        {
            var health = healthPool.Get(e);
            if (health.CurHealth <= 0)
            {
                ref var transform = ref transformPool.Get(e);
                transform.Transform.gameObject.SetActive(false);
                deathPool.Add(e);
            }
        }
    }
}
