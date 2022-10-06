using System;
using Leopotam.EcsLite;
using UnityEngine;

public class ProjectileAttackSystem : IEcsInitSystem, IEcsRunSystem
{
    private GameObject _bulletPrefab;
    public void Init(IEcsSystems systems)
    {
        _bulletPrefab = Resources.Load<GameObject>("Bullet");
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<AttackTimerReady>().Inc<ReadyToAttack>()
            .Inc<ProjectileAttack>().Inc<TransformComponent>().Exc<Death>().End();
        var timerPool = world.GetPool<AttackTimer>();
        var transformPool = world.GetPool<TransformComponent>();
        var completeTimerPool = world.GetPool<AttackTimerReady>();
        var attackPool = world.GetPool<ProjectileAttack>();

        foreach (var e in filter)
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
