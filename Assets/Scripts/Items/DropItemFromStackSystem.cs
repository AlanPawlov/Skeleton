using UnityEngine;
using Leopotam.EcsLite;
using System;

public class DropItemFromStackSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var dropZoneFilter = world.Filter<DropItemFromStackZone>()
            .Inc<TransformComponent>().End();
        var itemStackFilter = world.Filter<ItemsStack>().Inc<TransformComponent>()
            .Exc<EmptyStack>().End();

        var dropZonePool = world.GetPool<DropItemFromStackZone>();
        var itemStackPool = world.GetPool<ItemsStack>();
        var transformPool = world.GetPool<TransformComponent>();
        var emptyStackPool = world.GetPool<EmptyStack>();
        var fullStackPool = world.GetPool<FullStack>();

        foreach (var dropZoneEntity in dropZoneFilter)
        {
            ref var dropZone = ref dropZonePool.Get(dropZoneEntity);
            var dropZoneTransform = transformPool.Get(dropZoneEntity);

            foreach (var itemStackEntity in itemStackFilter)
            {
                var itemTransform = transformPool.Get(itemStackEntity);
                var distance = Vector3.Distance(itemTransform.Transform.position, dropZoneTransform.Transform.position);
                var stack = itemStackPool.Get(itemStackEntity);
                var timeCondition = (DateTime.Now - dropZone.LastDropTime).TotalSeconds > dropZone.DropInterval;
                if (distance <= dropZone.Radius && timeCondition)
                {
                    var itemEntity = (int)stack.Items.Pop();
                    UnityEngine.Object.Destroy(transformPool.Get(itemEntity).Transform.gameObject);
                    world.DelEntity(itemEntity);
                    dropZone.LastDropTime = DateTime.Now;

                    if (fullStackPool.Has(itemStackEntity))
                    {
                        fullStackPool.Del(itemStackEntity);
                    }
                    if (stack.Items.Count == 0)
                    {
                        emptyStackPool.Add(itemStackEntity);
                    }
                }
            }
        }
    }
}
