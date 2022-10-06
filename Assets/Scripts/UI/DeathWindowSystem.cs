using Leopotam.EcsLite;
using Zenject;

public class DeathWindowSystem : IEcsRunSystem
{
    private DeathWindow _deathWindow;    //       но я не знаю сколько буду думать над более адекватной реализацией данного момента,
                                         //       поэтому пока будет так, но что-то с этим надо делать
    
    public DeathWindowSystem(DeathWindow deathWindow)
    {
        _deathWindow = deathWindow;
    }

    public void Run(IEcsSystems systems)
    {
        var sharedData = systems.GetShared<ECSSharedData>();
        var world = systems.GetWorld();
        var filter = world.Filter<Death>().Inc<PlayerTag>().End();
        foreach (var e in filter)
        {
            if (!_deathWindow.IsActive)
            {
                sharedData.IsPlayerDeath = true;
                _deathWindow.Show();
            }
        }
    }
}
