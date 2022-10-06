using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

public class MobileInput : MonoBehaviour
{
    [SerializeField]
    private MobileJoystick _mobileJoystick;
    [SerializeField]
    private ExtendetButton _pauseButton;
    [SerializeField]
    private ExtendetButton _shotButton;
    [SerializeField]
    private ExtendetButton _jumpButton;

    private EcsFilter _filter;
    private EcsPool<InputComponent> _pool;
    private EcsWorld _world;
    private int _inputEntity;

    [Inject]
    public void Construct(EcsWorld world)
    {
        _world = world;
    }

    public void Start()
    {
        _filter = _world.Filter<InputComponent>().Inc<PlayerTag>().End();
        _pool = _world.GetPool<InputComponent>();

        foreach (var e in _filter)
        {
            _inputEntity = e;
        }
        _mobileJoystick.OnJoysctickValueChanged += OnMove;
        _jumpButton.onClick.AddListener(OnJump);
        _pauseButton.onClick.AddListener(OnPause);
    }

    private void OnPause()
    {
        ref var input = ref _pool.Get(_inputEntity);
        input.Pause = true;
    }

    private void OnJump()
    {
        ref var input = ref _pool.Get(_inputEntity);
        input.Jump = true;
    }

    private void OnMove(Vector2 value)
    {
        ref var input = ref _pool.Get(_inputEntity);
        input.Move = value;
    }

    private void Update()
    {
        ref var input = ref _pool.Get(_inputEntity);
        input.Shot = _shotButton.ButtonIsPressed;
    }

    private void OnDestroy()
    {
        _mobileJoystick.OnJoysctickValueChanged -= OnMove;
    }
}
