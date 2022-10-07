using Leopotam.EcsLite;

public class ClearEmptyStackTagSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _filter = _world.Filter<ItemsStack>().Inc<EmptyStack>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var itemStackPool = _world.GetPool<ItemsStack>();
        var emptyStackPool = _world.GetPool<EmptyStack>();

        foreach (var itemStackEntity in _filter)
        {
            ref var itemStack = ref itemStackPool.Get(itemStackEntity);
            if (itemStack.Items.Count > 0)
            {
                emptyStackPool.Del(itemStackEntity);
            }
        }
    }
}
