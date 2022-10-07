using Leopotam.EcsLite;
using System.Collections;

public class InitEmptyStackSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _itemStackFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _itemStackFilter = _world.Filter<ItemsStack>().Exc<FullStack>()
            .Exc<EmptyStack>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var itemStackPool = _world.GetPool<ItemsStack>();
        var emptyStackPool = _world.GetPool<EmptyStack>();

        foreach (var itemStackEntity in _itemStackFilter)
        {
            ref var itemStack = ref itemStackPool.Get(itemStackEntity);
            if (itemStack.Items == null)
            {
                itemStack.Items = new Stack();
                emptyStackPool.Add(itemStackEntity);
            }
        }
    }
}
