using Leopotam.EcsLite;
using Zenject;

public class PauseSystem : IEcsRunSystem
{
    private PauseWindow _pauseWindow;
    
    public PauseSystem(PauseWindow pauseWindow)
    {
        _pauseWindow = pauseWindow;
    }

    public void Run(IEcsSystems systems)
    {
        var sharedData = systems.GetShared<ECSSharedData>();
        var world = systems.GetWorld();
        var filter = world.Filter<InputComponent>().Inc<PlayerTag>().End();
        var pool = world.GetPool<InputComponent>();
        foreach (var e in filter)
        {
            ref var input = ref pool.Get(e);
            if (input.Pause && !_pauseWindow.IsActive)
            {
                input.Pause = false;
                sharedData.IsPause = true;
                _pauseWindow.Show();
            }
        }
    }
}
