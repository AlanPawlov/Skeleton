using Leopotam.EcsLite;
using System.Collections;

public class InitEmptyStackSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var itemStackFilter = world.Filter<ItemsStack>().Exc<FullStack>()
            .Exc<EmptyStack>().End();
        var itemStackPool = world.GetPool<ItemsStack>();
        var emptyStackPool = world.GetPool<EmptyStack>();

        foreach (var itemStackEntity in itemStackFilter)
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
