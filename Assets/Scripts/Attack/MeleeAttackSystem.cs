using System;
using Leopotam.EcsLite;
using UnityEngine;

public class MeleeAttackSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _meleeFilter;
    private EcsFilter _healthFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _meleeFilter = _world.Filter<AttackTimerReady>().Inc<ReadyToAttack>()
            .Inc<MeleeWeapon>().Inc<TransformComponent>().Exc<Death>().End();
        _healthFilter = _world.Filter<Health>().Inc<TransformComponent>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var timerPool = _world.GetPool<AttackTimer>();
        var healthPool = _world.GetPool<Health>();
        var transformPool = _world.GetPool<TransformComponent>();
        var completeTimerPool = _world.GetPool<AttackTimerReady>();
        var meleePool = _world.GetPool<MeleeWeapon>();

        foreach (var meleeEntity in _meleeFilter)
        {
            ref var timer = ref timerPool.Get(meleeEntity);
            var ownerTransform = transformPool.Get(meleeEntity);
            var melee = meleePool.Get(meleeEntity);
            var offset = ownerTransform.Transform.TransformDirection(melee.Offset);
            var direction = ownerTransform.Transform.TransformDirection(Vector3.forward);
            var hit = Physics.Raycast(new Ray(ownerTransform.Transform.position + offset, direction),
                out RaycastHit hitInfo, melee.Distance, melee.LayerMask);

            if (hit)
            {
                foreach (var e in _healthFilter)
                {
                    var transform = transformPool.Get(e);
                    if (hitInfo.transform == transform.Transform)
                    {
                        ref var health = ref healthPool.Get(e);
                        health.CurHealth -= melee.Damage;
                        timer.LastAttackTime = DateTime.Now;
                        completeTimerPool.Del(meleeEntity);
                    }
                }
            }
        }
    }
}
