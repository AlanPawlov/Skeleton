using Leopotam.EcsLite;
using UnityEngine;

public class SafeZoneSysem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _enemyFilter;
    private EcsFilter _playerFilter;
    private EcsFilter _safeZoneFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _enemyFilter = _world.Filter<HumanoidMovementComponent>().Inc<EnemyTag>()
            .Inc<TransformComponent>().Exc<StopMarker>().End();
        _playerFilter = _world.Filter<PlayerTag>().Inc<TransformComponent>().End();
        _safeZoneFilter = _world.Filter<SquareSafeZone>().Inc<TransformComponent>().End();
    }
 
    public void Run(IEcsSystems systems)
    {
        var transformPool = _world.GetPool<TransformComponent>();
        var stopMarkerPool = _world.GetPool<StopMarker>();
        var safeZonePool = _world.GetPool<SquareSafeZone>();

        var safeZonePosition = Vector3.zero;
        var edgeZoneLenght = 0f;
        foreach (var safeZoneEntity in _safeZoneFilter)
        {
            var safeZone = safeZonePool.Get(safeZoneEntity);
            var safeZoneTransform = transformPool.Get(safeZoneEntity);
            safeZonePosition = safeZoneTransform.Transform.position;
            edgeZoneLenght = safeZone.EdgeLenght;
        }

        foreach (var enemyEntity in _enemyFilter)
        {
            var enemyTransform = transformPool.Get(enemyEntity).Transform;

            foreach (var playerEntity in _playerFilter)
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
