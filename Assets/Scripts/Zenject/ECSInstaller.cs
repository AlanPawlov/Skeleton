using Zenject;
using Leopotam.EcsLite;
using Voody.UniLeo.Lite;

public class ECSInstaller : MonoInstaller
{
    private EcsWorld _world;
    private EcsSystems _systems;
    private EcsSystems _lateUpdateSystems;
    private EcsSystems _fixedUpdateSystems;

    public override void InstallBindings()
    {
        Init();
        InstallWorld();
        _systems
            .Add(new EnemyFollowToPlayerSystem())
            .Add(new EnemyStopMarkerSwitcherSystem())
            .Add(new SafeZoneSysem())
            .Add(new StopMovementSystem())
            .Add(new HumanoidMovementSystem())
            .Add(new PlayerRotationSystem())
            .Add(new EnemyRotationSystem());

        _lateUpdateSystems
            .Add(new ThirdPersonCameraSystem());

        _systems.ConvertScene();
        _systems.Init();

        _lateUpdateSystems
            .Init();

        _fixedUpdateSystems
            .Init();
    }

    private void InstallWorld()
    {
        Container
            .Bind<EcsWorld>()
            .FromInstance(_world)
            .AsSingle();
    }

    private void Init()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);
        _lateUpdateSystems = new EcsSystems(_world);
        _fixedUpdateSystems = new EcsSystems(_world);
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void LateUpdate()
    {
        _lateUpdateSystems?.Run();
    }

    private void FixedUpdate()
    {
        _fixedUpdateSystems?.Run();
    }

    private void OnDestroy()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }

        if (_lateUpdateSystems != null)
        {
            _lateUpdateSystems.Destroy();
            _lateUpdateSystems = null;
        }

        if (_fixedUpdateSystems != null)
        {
            _fixedUpdateSystems.Destroy();
            _fixedUpdateSystems = null;
        }

        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}
