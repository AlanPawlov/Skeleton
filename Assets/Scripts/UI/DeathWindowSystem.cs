using Leopotam.EcsLite;

public class DeathWindowSystem : IEcsRunSystem, IEcsInitSystem
{
    private DeathWindow _deathWindow;
    private ECSSharedData _sharedData;
    private EcsWorld _world;
    private EcsFilter _filter;

    public DeathWindowSystem(DeathWindow deathWindow)
    {
        _deathWindow = deathWindow;
    }

    public void Init(IEcsSystems systems)
    {
        _sharedData = systems.GetShared<ECSSharedData>();
        _world = systems.GetWorld();
        _filter = _world.Filter<Death>().Inc<PlayerTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var e in _filter)
        {
            if (!_deathWindow.IsActive)
            {
                _sharedData.IsPlayerDeath = true;
                _deathWindow.Show();
            }
        }
    }
}
