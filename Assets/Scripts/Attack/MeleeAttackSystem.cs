using System;
using Leopotam.EcsLite;
using UnityEngine;

public class MeleeAttackSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var meleeFilter = world.Filter<AttackTimerReady>().Inc<ReadyToAttack>()
            .Inc<MeleeAttack>().Inc<TransformComponent>().Exc<Death>().End();
        var healthFilter = world.Filter<Health>().Inc<TransformComponent>().End();

        var timerPool = world.GetPool<AttackTimer>();
        var healthPool = world.GetPool<Health>();
        var transformPool = world.GetPool<TransformComponent>();
        var completeTimerPool = world.GetPool<AttackTimerReady>();
        var meleePool = world.GetPool<MeleeAttack>();

        foreach (var meleeEntity in meleeFilter)
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
                foreach (var e in healthFilter)
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
