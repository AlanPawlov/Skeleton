using UnityEngine;
using Leopotam.EcsLite;

public class PickUpItemSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var itemFilter = world.Filter<Item>().Inc<TransformComponent>()
            .Exc<ItemInStack>().End();
        var itemStackFilter = world.Filter<ItemsStack>().Inc<TransformComponent>()
            .Exc<FullStack>().End();
        var itemPool = world.GetPool<Item>();
        var itemStackPool = world.GetPool<ItemsStack>();
        var transformPool = world.GetPool<TransformComponent>();
        var itemInStack = world.GetPool<ItemInStack>();
        var fullStackPool = world.GetPool<FullStack>();

        foreach (var itemStackEntity in itemStackFilter)
        {
            var playerTransform = transformPool.Get(itemStackEntity);
            ref var itemStack = ref itemStackPool.Get(itemStackEntity);

            foreach (var itemEntity in itemFilter)
            {
                ref var itemTransform = ref transformPool.Get(itemEntity);
                var item = itemPool.Get(itemEntity);
                var distance = Vector3.Distance(playerTransform.Transform.position, itemTransform.Transform.position);

                if (distance < 1)
                {
                    itemTransform.Transform.parent = itemStack.Parent;
                    itemTransform.Transform.position = itemStack.Parent.position + itemStack.Items.Count * item.Offset;
                    itemTransform.Transform.rotation = itemStack.Parent.rotation;
                    itemStack.Items.Push(itemEntity);
                    itemInStack.Add(itemEntity);

                    if (itemStack.Items.Count >= itemStack.MaxSize)
                    {
                        fullStackPool.Add(itemStackEntity);
                        break;
                    }
                }
            }
        }
    }
}
