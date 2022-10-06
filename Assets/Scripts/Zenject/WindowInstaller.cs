using UnityEngine;
using Zenject;

public class WindowInstaller : MonoInstaller
{
    [SerializeField]
    private PauseWindow _pauseWindow;
    [SerializeField]
    private DeathWindow _deathWindow;

    public override void InstallBindings()
    {
        InstallPauseWindow();
        InstallDeathWindow();
    }

    private void InstallDeathWindow()
    {
        Container
            .Bind<PauseWindow>()
            .FromInstance(_pauseWindow)
            .AsSingle();
    }

    private void InstallPauseWindow()
    {
        Container
            .Bind<DeathWindow>()
            .FromInstance(_deathWindow)
            .AsSingle();
    }
}
