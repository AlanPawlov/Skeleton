using UnityEngine;
using Leopotam.EcsLite;

public class PickUpItemSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _itemFilter;
    private EcsFilter _itemStackFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _itemFilter = _world.Filter<Item>().Inc<TransformComponent>()
            .Exc<ItemInStack>().End();
        _itemStackFilter = _world.Filter<ItemsStack>().Inc<TransformComponent>()
            .Exc<FullStack>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var itemPool = _world.GetPool<Item>();
        var itemStackPool = _world.GetPool<ItemsStack>();
        var transformPool = _world.GetPool<TransformComponent>();
        var itemInStack = _world.GetPool<ItemInStack>();
        var fullStackPool = _world.GetPool<FullStack>();

        foreach (var itemStackEntity in _itemStackFilter)
        {
            var playerTransform = transformPool.Get(itemStackEntity);
            ref var itemStack = ref itemStackPool.Get(itemStackEntity);

            foreach (var itemEntity in _itemFilter)
            {
                ref var itemTransform = ref transformPool.Get(itemEntity);
                var item = itemPool.Get(itemEntity);
                var distance = (playerTransform.Transform.position - itemTransform.Transform.position).sqrMagnitude;

                if (distance < item.SqrPickUpDistance)
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
