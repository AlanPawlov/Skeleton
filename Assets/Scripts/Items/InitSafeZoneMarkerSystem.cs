using Leopotam.EcsLite;
using UnityEngine;

public class InitSafeZoneMarkerSystem : IEcsInitSystem
{
    private const string HealthMarkerPath = "SafeMarker";
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<SquareSafeZone>()
            .Inc<TransformComponent>().End();
        
        var transformPool = _world.GetPool<TransformComponent>();
        var safeZonePool = _world.GetPool<SquareSafeZone>();
        var prefab = Resources.Load<Transform>(HealthMarkerPath);
        
        foreach (var e in _filter)
        {
            var transform = transformPool.Get(e);
            var dropZone = safeZonePool.Get(e);
            var marker = Object.Instantiate(prefab, transform.Transform.position, transform.Transform.rotation);
            marker.localScale = Vector3.one * dropZone.EdgeLenght;
        }
    }
}
