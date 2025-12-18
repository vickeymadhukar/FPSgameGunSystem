using UnityEngine;

public class Idelstate : MovementBase
{
    public override void EnterState(Movement movement)
    {
        // Reset animator parameters for idle
        movement.animator.SetBool("moving", false);
        movement.animator.SetBool("crouch", false);
        movement.animator.SetBool("prone", false);
    }

    public override void UpadteState(Movement movement)
    {
        // If crouching toggle is ON → go to crouch state
        if (movement.isCrouching)
        {
            movement.SwitchState(movement.crouchstate);
            return;
        }

        if (movement.isProne)
        {
               movement.SwitchState(movement.pronestate);
        }

        // If joystick moves → go to Moving state
        if (movement.inputMovement.magnitude > 0.1f)
        {
            movement.SwitchState(movement.movingstate);
        }
    }


    public override void ExitState(Movement movement)
    {
        // nothing special needed, but keep for consistency
    }

}
