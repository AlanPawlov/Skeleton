using Leopotam.EcsLite;
using UnityEngine;

public class SafeZoneSysem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var enemyFilter = world.Filter<HumanoidMovementComponent>().Inc<EnemyTag>()
            .Inc<TransformComponent>().Exc<StopMarker>().End();
        var playerFilter = world.Filter<PlayerTag>().Inc<TransformComponent>().End();
        var safeZoneFilter = world.Filter<SquareSafeZone>().End();
        var transformPool = world.GetPool<TransformComponent>();
        var stopMarkerPool = world.GetPool<StopMarker>();
        var safeZonePool = world.GetPool<SquareSafeZone>();

        var safeZonePosition = Vector3.zero;
        var edgeZoneLenght = 0f;
        foreach (var safeZoneEntity in safeZoneFilter)
        {
            var safeZone = safeZonePool.Get(safeZoneEntity);
            safeZonePosition = safeZone.Center;
            edgeZoneLenght = safeZone.EdgeLenght;
        }

        foreach (var enemyEntity in enemyFilter)
        {
            var enemyTransform = transformPool.Get(enemyEntity).Transform;

            foreach (var playerEntity in playerFilter)
            {
                var playerPosition = transformPool.Get(playerEntity).Transform.position;
                if (IsPlayerInSafeZone(playerPosition, safeZonePosition, edgeZoneLenght))
                {
                    stopMarkerPool.Add(enemyEntity);
                }
            }
        }
    }

    public bool IsPlayerInSafeZone(Vector3 playerPosition, Vector3 safeZoneposition, float edgeLenght)
    {
        var xPosInSafeZone = Mathf.Abs(playerPosition.x - safeZoneposition.x) < edgeLenght / 2;
        var zPosInSafeZone = Mathf.Abs(playerPosition.z - safeZoneposition.z) < edgeLenght / 2;
        var result = xPosInSafeZone && zPosInSafeZone;
        return result;
    }
}
