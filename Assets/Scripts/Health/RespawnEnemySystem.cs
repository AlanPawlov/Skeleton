using Leopotam.EcsLite;
using UnityEngine;

public class RespawnEnemySystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var deathFilter = world.Filter<Death>().Inc<TransformComponent>()
            .Inc<EnemyTag>().End();
        var spawnZoneFilter = world.Filter<RespawnZone>().End();
        var healthPool = world.GetPool<Health>();
        var deathPool = world.GetPool<Death>();
        var spawnZonePool = world.GetPool<RespawnZone>();
        var transformPool = world.GetPool<TransformComponent>();

        var spawnZoneCenterPosition = Vector3.zero;
        var spawnZoneRadius = 0f;
        foreach (var spawnZoneEntity in spawnZoneFilter)
        {
            var spawnZone = spawnZonePool.Get(spawnZoneEntity);
            spawnZoneCenterPosition = spawnZone.Center;
            spawnZoneRadius = spawnZone.Radius;
        }

        foreach (var e in deathFilter)
        {
            ref var health = ref healthPool.Get(e);
            ref var transform = ref transformPool.Get(e);
            var xPos = spawnZoneCenterPosition.x + (Random.Range(-1, 2) * Random.Range(0, spawnZoneRadius));
            var zPos = spawnZoneCenterPosition.z + (Random.Range(-1, 2) * Random.Range(0, spawnZoneRadius));
            var resultPosition = new Vector3(xPos, spawnZoneCenterPosition.y, zPos);
            transform.Transform.position = resultPosition;
            health.CurHealth = health.MaxHealth;
            transform.Transform.gameObject.SetActive(true);
            deathPool.Del(e);
        }
    }
}
