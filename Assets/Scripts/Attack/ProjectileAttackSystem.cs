using System;
using Leopotam.EcsLite;
using UnityEngine;

public class ProjectileAttackSystem : IEcsInitSystem, IEcsRunSystem
{
    private const string BulletPath = "Bullet";
    private GameObject _bulletPrefab;
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _bulletPrefab = Resources.Load<GameObject>(BulletPath);
        _world = systems.GetWorld();
        _filter = _world.Filter<AttackTimerReady>().Inc<ReadyToAttack>()
            .Inc<ProjectileWeapon>().Inc<TransformComponent>().Exc<Death>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var timerPool = _world.GetPool<AttackTimer>();
        var transformPool = _world.GetPool<TransformComponent>();
        var completeTimerPool = _world.GetPool<AttackTimerReady>();
        var attackPool = _world.GetPool<ProjectileWeapon>();

        foreach (var e in _filter)
        {
            ref var timer = ref timerPool.Get(e);
            var ownerTransform = transformPool.Get(e);
            var attackComponent = attackPool.Get(e);
            var offset = ownerTransform.Transform.TransformDirection(attackComponent.Offset);
            UnityEngine.Object.Instantiate(_bulletPrefab, ownerTransform.Transform.position + offset,
                ownerTransform.Transform.localRotation);
            timer.LastAttackTime = DateTime.Now;
            completeTimerPool.Del(e);
        }
    }
}
