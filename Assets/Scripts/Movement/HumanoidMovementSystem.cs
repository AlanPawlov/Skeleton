using UnityEngine;
using Leopotam.EcsLite;

public class HumanoidMovementSystem : IEcsInitSystem, IEcsRunSystem
{
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var movementFilter = world.Filter<HumanoidMovementComponent>().End();
        var movementPool = world.GetPool<HumanoidMovementComponent>();
        foreach (var movementEntity in movementFilter)
        {
            ref var movement = ref movementPool.Get(movementEntity);
            movement.AnimIDSpeed = Animator.StringToHash("Speed");
            movement.AnimIDGrounded = Animator.StringToHash("Grounded");
            movement.AnimIDJump = Animator.StringToHash("Jump");
            movement.AnimIDFreeFall = Animator.StringToHash("FreeFall");
            movement.AnimIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }
    }

    public void Run(IEcsSystems systems)
    {
        var sharedData = systems.GetShared<ECSSharedData>();
        if (sharedData.IsPause || sharedData.IsPlayerDeath)
        {
            return;
        }
        var world = systems.GetWorld();
        var movementFilter = world.Filter<HumanoidMovementComponent>().Inc<TransformComponent>()
            .Inc<InputComponent>().End();
        var inputPool = world.GetPool<InputComponent>();
        var movementPool = world.GetPool<HumanoidMovementComponent>();
        var transformPool = world.GetPool<TransformComponent>();
        foreach (var movementEntity in movementFilter)
        {
            ref var input = ref inputPool.Get(movementEntity);
            ref var movement = ref movementPool.Get(movementEntity);
            var transform = transformPool.Get(movementEntity);

            JumpAndGravity(ref input, ref movement);
            GroundedCheck(ref movement, transform.Transform.position);
            Move(input, ref movement);
        }
    }

    private void GroundedCheck(ref HumanoidMovementComponent movement, Vector3 position)
    {
        Vector3 spherePosition = new Vector3(position.x, position.y - movement.GroundedOffset, position.z);

        movement.Grounded = Physics.CheckSphere(spherePosition, movement.GroundedRadius,
            movement.GroundLayers, QueryTriggerInteraction.Ignore);

        if (movement.HasAnimator)
        {
            movement.Animator.SetBool(movement.AnimIDGrounded, movement.Grounded);
        }
    }

    private void JumpAndGravity(ref InputComponent input, ref HumanoidMovementComponent movement)
    {
        if (movement.Grounded)
        {
            movement.FallTimeoutDelta = movement.FallTimeout;

            if (movement.HasAnimator)
            {
                movement.Animator.SetBool(movement.AnimIDJump, false);
                movement.Animator.SetBool(movement.AnimIDFreeFall, false);
            }

            if (movement.VerticalVelocity < 0.0f)
            {
                movement.VerticalVelocity = -2f;
            }

            if (input.Jump && movement.JumpTimeoutDelta <= 0.0f)
            {
                movement.VerticalVelocity = Mathf.Sqrt(movement.JumpHeight * -2f * movement.Gravity);

                if (movement.HasAnimator)
                {
                    movement.Animator.SetBool(movement.AnimIDJump, true);
                }
            }


            if (movement.JumpTimeoutDelta >= 0.0f)
            {
                movement.JumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            movement.JumpTimeoutDelta = movement.JumpTimeout;

            if (movement.FallTimeoutDelta >= 0.0f)
            {
                movement.FallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (movement.HasAnimator)
                {
                    movement.Animator.SetBool(movement.AnimIDFreeFall, true);
                }
            }

            input.Jump = false;
        }

        if (movement.VerticalVelocity < movement.TerminalVelocity)
        {
            movement.VerticalVelocity += movement.Gravity * Time.deltaTime;
        }
    }

    private void Move(InputComponent input, ref HumanoidMovementComponent movement)
    {
        float targetSpeed = movement.MoveSpeed;
        
        if (input.Move == Vector2.zero)
        {
            targetSpeed = 0.0f;
        } 

        float currentHorizontalSpeed = new Vector3(movement.Controller.velocity.x, 0.0f, movement.Controller.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMagnitude = 1;
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            movement.Speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * movement.SpeedChangeRate);
            movement.Speed = Mathf.Round(movement.Speed * 1000f) / 1000f;
        }
        else
        {
            movement.Speed = targetSpeed;
        }

        movement.AnimationBlend = Mathf.Lerp(movement.AnimationBlend,
            targetSpeed, Time.deltaTime * movement.SpeedChangeRate);
        if (movement.AnimationBlend < 0.01f)
        {
            movement.AnimationBlend = 0f;
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, movement.TargetRotation, 0.0f) * Vector3.forward;

        movement.Controller.Move(targetDirection.normalized * (movement.Speed * Time.deltaTime) +
            new Vector3(0.0f, movement.VerticalVelocity, 0.0f) * Time.deltaTime);

        if (movement.HasAnimator)
        {
            movement.Animator.SetFloat(movement.AnimIDSpeed, movement.AnimationBlend);
            movement.Animator.SetFloat(movement.AnimIDMotionSpeed, inputMagnitude);
        }
    }
}
