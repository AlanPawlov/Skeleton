using UnityEngine;
using Leopotam.EcsLite;

public class BulletFlySystem : IEcsRunSystem, IEcsInitSystem
{
    private ECSSharedData _sharedData;
    private EcsWorld _world;
    private EcsFilter _bulletComponentFilter;
    private EcsFilter _healthFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _sharedData = systems.GetShared<ECSSharedData>();
        _bulletComponentFilter = _world.Filter<Bullet>().Inc<TransformComponent>().End();
        _healthFilter = _world.Filter<Health>().Inc<TransformComponent>()
            .Exc<Death>().End();
    }

    public void Run(IEcsSystems systems)
    {
        if (_sharedData.IsPause || _sharedData.IsPlayerDeath)
        {
            return;
        }
        var healthPool = _world.GetPool<Health>();

        var bulletPool = _world.GetPool<Bullet>();
        var transformPool = _world.GetPool<TransformComponent>();
        foreach (var bulletEntity in _bulletComponentFilter)
        {
            ref var transform = ref transformPool.Get(bulletEntity);
            var bullet = bulletPool.Get(bulletEntity);
            var hit = Physics.SphereCast(transform.Transform.position, bullet.Radius,
                transform.Transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, bullet.Radius);

            if (hit)
            {
                foreach (var e in _healthFilter)
                {
                    var targetTransform = transformPool.Get(e);
                    if (hitInfo.transform == targetTransform.Transform)
                    {
                        ref var health = ref healthPool.Get(e);
                        health.CurHealth -= bullet.Damage;
                        break;
                    }
                }
                Object.Destroy(transform.Transform.gameObject);
                _world.DelEntity(bulletEntity);
                return;
            }
            var speed = Time.deltaTime * bullet.Speed;
            transform.Transform.Translate(bullet.Direction * speed, Space.World);
        }
    }
}
