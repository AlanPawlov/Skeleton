using Leopotam.EcsLite;

public class DeathSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _healthFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _healthFilter = _world.Filter<Health>().Inc<TransformComponent>()
            .Exc<Death>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var healthPool = _world.GetPool<Health>();
        var deathPool = _world.GetPool<Death>();
        var transformPool = _world.GetPool<TransformComponent>();

        foreach (var e in _healthFilter)
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
