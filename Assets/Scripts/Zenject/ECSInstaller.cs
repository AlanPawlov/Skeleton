using Zenject;
using Leopotam.EcsLite;
using Voody.UniLeo.Lite;
using UnityEngine;

public class ECSInstaller : MonoInstaller
{
    [SerializeField]
    private DeathWindow _deathWindow;
    [SerializeField]
    private PauseWindow _pauseWindow;
    private ECSSharedData _sharedData;
    private EcsWorld _world;
    private EcsSystems _systems;
    private EcsSystems _lateUpdateSystems;


    private void Awake()
    {
        _systems
           .Add(new InitDropZoneMarkerSystem())
           .Add(new InitSafeZoneMarkerSystem())
           .Add(new AutodestructableSystem())
           .Add(new RespawnEnemySystem())
           .Add(new FollowToPlayerSystem())
           .Add(new EnemyStopMarkerSwitcherSystem())
           .Add(new SafeZoneSysem())
           .Add(new StopMovementSystem())
           .Add(new HumanoidMovementSystem())
           .Add(new PlayerRotationSystem())
           .Add(new AttackTimerSystem())
           .Add(new InitReadyToAttackSystem())
           .Add(new MeleeAttackSystem())
           .Add(new ProjectileAttackSystem())
           .Add(new ClearReadyToAttackSystem())
           .Add(new DeathSystem())
           .Add(new DropItemFromDeathInitSystem())
           .Add(new DropItemFromDeathSystem())
           .Add(new ClearDropItemFromDeathTagSystem())
           .Add(new InitEmptyStackSystem())
           .Add(new PickUpItemSystem())
           .Add(new ClearEmptyStackTagSystem())
           .Add(new DropItemFromStackSystem())
           .Add(new BulletFlySystem())
           .Add(new EnemyRotationSystem())
           .Add(new UpdateHudSystem())
           .Add(new PauseSystem(_pauseWindow))
           .Add(new DeathWindowSystem(_deathWindow));

        _lateUpdateSystems
            .Add(new ThirdPersonCameraSystem());

        _systems.ConvertScene();
        _systems.Init();

        _lateUpdateSystems.Init();
    }

    public override void InstallBindings()
    {
        Init();
        InstallWorld();
        InstallSystems();
        InstallSharedData();
    }

    private void InstallWorld()
    {
        Container
            .Bind<EcsWorld>()
            .FromInstance(_world)
            .AsSingle();
    }
    private void InstallSystems()
    {
        Container
            .Bind<EcsSystems>()
            .FromInstance(_systems)
            .AsSingle();
    }

    private void InstallSharedData()
    {
        Container
            .Bind<ECSSharedData>()
            .FromInstance(_sharedData)
            .AsSingle();
    }

    private void Init()
    {
        _sharedData = new ECSSharedData();
        _world = new EcsWorld();
        _systems = new EcsSystems(_world, _sharedData);
        _lateUpdateSystems = new EcsSystems(_world);
    }

    private void Update()
    {
        _systems?.Run();
    }

    private void LateUpdate()
    {
        _lateUpdateSystems?.Run();
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

        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }
}
