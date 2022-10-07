using Leopotam.EcsLite;

public class UpdateHudSystem : IEcsRunSystem , IEcsInitSystem
{
    private ECSSharedData _sharedData;
    private EcsWorld _world;
    private EcsFilter _hudFilter;
    private EcsFilter _playerFilter;
    // TODO: нужно либо отказываться от обновления hud в ecs
    // либо обновлять только при наличии тега
    // но непонятно как вешать тег без костылей
    // поэтому пока обновление будет работать в каждом кадре
    public void Init(IEcsSystems systems)
    {
        _sharedData = systems.GetShared<ECSSharedData>();
        _world = systems.GetWorld();
        //var hudFilter = world.Filter<HudComponent>().Inc<UpdateHudComponent>().End();
        _hudFilter = _world.Filter<HudComponent>().End();                              
        _playerFilter = _world.Filter<Health>().Inc<PlayerTag>()
            .Inc<ItemsStack>().Inc<ScoreCounter>().End();            
    }

    public void Run(IEcsSystems systems)
    {
        if (_sharedData.IsPause || _sharedData.IsPlayerDeath)
        {
            return;
        }
        var hudPool= _world.GetPool<HudComponent>();
        var healthPool = _world.GetPool<Health>();
        var stackPool = _world.GetPool<ItemsStack>();
        var scoreCounterPool = _world.GetPool<ScoreCounter>();
        var healthBarFill = 0f;
        var maxStackSize = 0;
        var curStackSize = 0;
        var curScore = 0;

        foreach (var e in _playerFilter)
        {
            var health = healthPool.Get(e);
            healthBarFill = ((float)health.CurHealth / health.MaxHealth);
            var stack = stackPool.Get(e);
            curStackSize = stack.Items.Count;
            maxStackSize = stack.MaxSize;
            var scoreCounter = scoreCounterPool.Get(e);
            curScore = scoreCounter.CurrentScore;
        }

        foreach (var hudEntity in _hudFilter)
        {
            var hud = hudPool.Get(hudEntity);
            hud.HealthBar.fillAmount = healthBarFill;
            hud.ScoreText.text = curScore.ToString();
            hud.StackCounterText.text = $"{curStackSize}/{maxStackSize}";
        }
    }
}
