using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Leopotam.EcsLite;

public class InputHandler : MonoBehaviour
{
    public InputAction MoveInput;
    public InputAction LookInput;
    public InputAction JumpInput;

    private EcsFilter _filter;
    private EcsPool<InputComponent> _pool;
    private int _inputEntity;

    private void Awake()
    {
        MoveInput.Enable();
        LookInput.Enable();
        JumpInput.Enable();

        LookInput.started += LookActionOnStarted;
        LookInput.canceled += LookActionCanceled;
        JumpInput.started += JumpStarted;
    }

    [Inject]
    public void Construct(EcsWorld world)
    {
        _filter = world.Filter<InputComponent>().Inc<PlayerTag>().End();
        _pool = world.GetPool<InputComponent>();

        foreach (var e in _filter)
        {
            _inputEntity = e;
        }
    }

    private void Update()
    {
            ref var input = ref _pool.Get(_inputEntity);
            input.Move = MoveInput.ReadValue<Vector2>();
    }

    private void JumpStarted(InputAction.CallbackContext obj)
    {
            ref var input = ref _pool.Get(_inputEntity);
            input.Jump = JumpInput.ReadValue<float>() > 0 ? true : false;
    }

    private void LookActionOnStarted(InputAction.CallbackContext obj)
    {
        foreach (var e in _filter)
        {
            _inputEntity = e;
        }
        ref var input = ref _pool.Get(_inputEntity);
        input.Look = obj.ReadValue<Vector2>();
    }

    private void LookActionCanceled(InputAction.CallbackContext obj)
    {
            ref var input = ref _pool.Get(_inputEntity);
            input.Look = obj.ReadValue<Vector2>();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        //SetCursorState(hasFocus);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    private void OnDestroy()
    {
        MoveInput.Disable();
        LookInput.Disable();
        JumpInput.Disable();

        LookInput.started -= LookActionOnStarted;
        LookInput.canceled -= LookActionCanceled;
        JumpInput.started -= JumpStarted;

    }
}
