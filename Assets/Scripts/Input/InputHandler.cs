using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Leopotam.EcsLite;

public class InputHandler : MonoBehaviour
{
    public InputAction MoveInput;
    public InputAction LookInput;
    public InputAction JumpInput;
    public InputAction ShotInput;
    public InputAction PauseInput;

    private EcsFilter _filter;
    private EcsPool<InputComponent> _pool;
    private int _inputEntity;
    private EcsWorld _world;


    [Inject]
    public void Construct(EcsWorld world)
    {
        _world = world;
    }

    private void Start()
    {
        _filter = _world.Filter<InputComponent>().Inc<PlayerTag>().End();
        _pool = _world.GetPool<InputComponent>();

        foreach (var e in _filter)
        {
            _inputEntity = e;
        }

        MoveInput.Enable();
        LookInput.Enable();
        JumpInput.Enable();
        ShotInput.Enable();
        PauseInput.Enable();

        LookInput.started += LookActionOnStarted;
        LookInput.canceled += LookActionCanceled;
        JumpInput.started += JumpStarted;
        ShotInput.started += ShotStarted;
        PauseInput.started += PauseStarted;
        ShotInput.canceled += ShotCanceled;
    }

    private void Update()
    {
            ref var input = ref _pool.Get(_inputEntity);
            input.Move = MoveInput.ReadValue<Vector2>();
    }


    private void ShotStarted(InputAction.CallbackContext obj)
    {
        ref var input = ref _pool.Get(_inputEntity);
        input.Shot = ShotInput.ReadValue<float>() > 0 ? true : false;
        Debug.Log(input.Shot);
    }

    private void ShotCanceled(InputAction.CallbackContext obj)
    {
        ref var input = ref _pool.Get(_inputEntity);
        input.Shot = ShotInput.ReadValue<float>() > 0 ? true : false;
        Debug.Log(input.Shot);
    }
    private void JumpStarted(InputAction.CallbackContext obj)
    {
            ref var input = ref _pool.Get(_inputEntity);
            input.Jump = JumpInput.ReadValue<float>() > 0 ? true : false;
    }

    private void PauseStarted(InputAction.CallbackContext obj)
    {
        ref var input = ref _pool.Get(_inputEntity);
        input.Pause = PauseInput.ReadValue<float>() > 0 ? true : false;
    }

    private void LookActionOnStarted(InputAction.CallbackContext obj)
    {
        ref var input = ref _pool.Get(_inputEntity);
        input.Look = obj.ReadValue<Vector2>();
    }

    private void LookActionCanceled(InputAction.CallbackContext obj)
    {
            ref var input = ref _pool.Get(_inputEntity);
            input.Look = obj.ReadValue<Vector2>();
    }

    //private void OnApplicationFocus(bool hasFocus)
    //{
    //    SetCursorState(hasFocus);
    //}

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnDestroy()
    {
        MoveInput.Disable();
        LookInput.Disable();
        JumpInput.Disable();
        ShotInput.Disable();
        PauseInput.Disable();

        LookInput.started -= LookActionOnStarted;
        LookInput.canceled -= LookActionCanceled;
        JumpInput.started -= JumpStarted;
        ShotInput.started -= ShotStarted;
        ShotInput.canceled -= ShotStarted;
        PauseInput.started -= PauseStarted;
    }
}
