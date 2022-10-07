using Leopotam.EcsLite;
using UnityEngine;

public class InitDropZoneMarkerSystem : IEcsInitSystem
{
    private const string HealthMarkerPath = "HealthMarker";
    private const string ScoreMarkerPath = "ScoreMarker";
    private EcsWorld _world;
    private EcsFilter _dropZoneFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _dropZoneFilter = _world.Filter<DropItemFromStackZone>()
            .Inc<TransformComponent>().End();
        var transformPool = _world.GetPool<TransformComponent>();
        var dropZonePool = _world.GetPool<DropItemFromStackZone>();
        foreach (var e in _dropZoneFilter)
        {
            var transform = transformPool.Get(e);
            var dropZone = dropZonePool.Get(e);
            string path = string.Empty;
            switch (dropZone.RewardType)
            {
                case RewardType.Health:
                    path = HealthMarkerPath;
                    break;
                case RewardType.Score:
                    path = ScoreMarkerPath;
                    break;
            }
            var prefab = Resources.Load<Transform>(path);
            var marker =  Object.Instantiate(prefab, transform.Transform.position, transform.Transform.rotation);
            marker.localScale = Vector3.one * dropZone.Radius * dropZone.Radius;
        }
    }
}
