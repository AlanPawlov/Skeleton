using Leopotam.EcsLite;

public class ClearEmptyStackTagSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<ItemsStack>().Inc<EmptyStack>().End();
        var itemStackPool = world.GetPool<ItemsStack>();
        var emptyStackPool = world.GetPool<EmptyStack>();

        foreach (var itemStackEntity in filter)
        {
            ref var itemStack = ref itemStackPool.Get(itemStackEntity);
            if (itemStack.Items.Count > 0)
            {
                emptyStackPool.Del(itemStackEntity);
            }
        }
    }
}
