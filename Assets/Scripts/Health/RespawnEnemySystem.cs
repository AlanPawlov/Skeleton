using Leopotam.EcsLite;
using UnityEngine;

public class RespawnEnemySystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _deathFilter;
    private EcsFilter _spawnZoneFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _deathFilter = _world.Filter<Death>().Inc<TransformComponent>()
            .Inc<EnemyTag>().End();
        _spawnZoneFilter = _world.Filter<RespawnZone>().Inc<TransformComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var healthPool = _world.GetPool<Health>();
        var deathPool = _world.GetPool<Death>();
        var spawnZonePool = _world.GetPool<RespawnZone>();
        var transformPool = _world.GetPool<TransformComponent>();

        var spawnZonePosition = Vector3.zero;
        var spawnZoneRadius = 0f;
        foreach (var spawnZoneEntity in _spawnZoneFilter)
        {
            var spawnZone = spawnZonePool.Get(spawnZoneEntity);
            var spawnZoneTransform = transformPool.Get(spawnZoneEntity);
            spawnZonePosition = spawnZoneTransform.Transform.position;
            spawnZoneRadius = spawnZone.Radius;
        }

        foreach (var e in _deathFilter)
        {
            ref var health = ref healthPool.Get(e);
            ref var transform = ref transformPool.Get(e);
            var xPos = spawnZonePosition.x + (Random.Range(-1, 2) * Random.Range(0, spawnZoneRadius));
            var zPos = spawnZonePosition.z + (Random.Range(-1, 2) * Random.Range(0, spawnZoneRadius));
            var resultPosition = new Vector3(xPos, spawnZonePosition.y, zPos);
            transform.Transform.position = resultPosition;
            health.CurHealth = health.MaxHealth;
            transform.Transform.gameObject.SetActive(true);
            deathPool.Del(e);
        }
    }
}
