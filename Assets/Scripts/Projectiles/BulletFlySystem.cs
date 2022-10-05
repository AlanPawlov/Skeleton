using UnityEngine;

using Leopotam.EcsLite;

public class BulletFlySystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var bulletComponentFilter = world.Filter<Bullet>().Inc<TransformComponent>().End();
        var healthFilter = world.Filter<Health>().Inc<TransformComponent>()
            .Exc<Death>().End();
        var healthPool = world.GetPool<Health>();

        var bulletPool = world.GetPool<Bullet>();
        var transformPool = world.GetPool<TransformComponent>();
        foreach (var bulletEntity in bulletComponentFilter)
        {
            ref var transform = ref transformPool.Get(bulletEntity);
            var bullet = bulletPool.Get(bulletEntity);
            var hit = Physics.SphereCast(transform.Transform.position, bullet.Radius,
                transform.Transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo);

            if (hit)
            {
                foreach (var e in healthFilter)
                {
                    var targetTransform = transformPool.Get(e);
                    if (hitInfo.transform == targetTransform.Transform)
                    {
                        ref var health = ref healthPool.Get(e);
                        health.CurHealth -= bullet.Damage;
                        Debug.Log($"Bullet hit: {targetTransform.Transform.gameObject.name} {health.CurHealth}");
                        break;
                    }
                }
                Object.Destroy(transform.Transform.gameObject);
                world.DelEntity(bulletEntity);
                return;
            }
            var speed = Time.deltaTime * bullet.Speed;
            transform.Transform.Translate(bullet.Direction * speed, Space.World);
        }
    }
}
