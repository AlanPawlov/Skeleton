using Leopotam.EcsLite;
using Zenject;

public class PauseSystem : IEcsRunSystem, IEcsInitSystem
{
    private PauseWindow _pauseWindow;
    private ECSSharedData _sharedData;
    private EcsWorld _world;
    private EcsFilter _filter;

    public PauseSystem(PauseWindow pauseWindow)
    {
        _pauseWindow = pauseWindow;
    }

    public void Init(IEcsSystems systems)
    {
        _sharedData = systems.GetShared<ECSSharedData>();
        _world = systems.GetWorld();
        _filter = _world.Filter<InputComponent>().Inc<PlayerTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        var pool = _world.GetPool<InputComponent>();
        foreach (var e in _filter)
        {
            ref var input = ref pool.Get(e);
            if (input.Pause && !_pauseWindow.IsActive)
            {
                input.Pause = false;
                _sharedData.IsPause = true;
                _pauseWindow.Show();
            }
        }
    }
}
