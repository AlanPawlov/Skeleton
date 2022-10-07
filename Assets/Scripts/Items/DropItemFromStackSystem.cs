using Leopotam.EcsLite;
using System;

public class DropItemFromStackSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _dropZoneFilter;
    private EcsFilter _itemStackFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _dropZoneFilter = _world.Filter<DropItemFromStackZone>()
            .Inc<TransformComponent>().End();
        _itemStackFilter = _world.Filter<ItemsStack>().Inc<TransformComponent>()
            .Exc<EmptyStack>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var dropZonePool = _world.GetPool<DropItemFromStackZone>();
        var itemStackPool = _world.GetPool<ItemsStack>();
        var transformPool = _world.GetPool<TransformComponent>();
        var emptyStackPool = _world.GetPool<EmptyStack>();
        var fullStackPool = _world.GetPool<FullStack>();
        var scoreCounterPool = _world.GetPool<ScoreCounter>();
        var healthPool = _world.GetPool<Health>();

        foreach (var dropZoneEntity in _dropZoneFilter)
        {
            ref var dropZone = ref dropZonePool.Get(dropZoneEntity);
            var dropZoneTransform = transformPool.Get(dropZoneEntity);

            foreach (var itemStackEntity in _itemStackFilter)
            {
                var itemTransform = transformPool.Get(itemStackEntity);
                var distance = (itemTransform.Transform.position - dropZoneTransform.Transform.position).sqrMagnitude;
                var stack = itemStackPool.Get(itemStackEntity);
                var timeCondition = (DateTime.Now - dropZone.LastDropTime).TotalSeconds > dropZone.DropInterval;
                if (distance <= dropZone.Radius * dropZone.Radius && timeCondition)
                {
                    var itemEntity = (int)stack.Items.Pop();
                    UnityEngine.Object.Destroy(transformPool.Get(itemEntity).Transform.gameObject);
                    _world.DelEntity(itemEntity);
                    dropZone.LastDropTime = DateTime.Now;
                    AddReward(scoreCounterPool, healthPool, dropZone, itemStackEntity);

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

    private void AddReward(EcsPool<ScoreCounter> scoreCounterPool, EcsPool<Health> healthPool, DropItemFromStackZone dropZone, int itemStackEntity)
    {
        switch (dropZone.RewardType)
        {
            case RewardType.Health:
                AddHealth(healthPool, dropZone, itemStackEntity);
                break;
            case RewardType.Score:
                AddScore(scoreCounterPool, dropZone, itemStackEntity);
                break;
        }
    }

    private void AddScore(EcsPool<ScoreCounter> scoreCounterPool, DropItemFromStackZone dropZone, int itemStackEntity)
    {
        if (scoreCounterPool.Has(itemStackEntity))
        {
            ref var scoreCounter = ref scoreCounterPool.Get(itemStackEntity);
            scoreCounter.CurrentScore += dropZone.Reward;
        }
    }

    private void AddHealth(EcsPool<Health> healthPool, DropItemFromStackZone dropZone, int itemStackEntity)
    {
        if (healthPool.Has(itemStackEntity))
        {
            ref var health = ref healthPool.Get(itemStackEntity);
            health.CurHealth += dropZone.Reward;
        }
    }
}
