using UnityEngine;

public class Movingstate : MovementBase
{
    public override void EnterState(Movement movement)
    {
        movement.animator.SetBool("moving", true);
    }

    public override void UpadteState(Movement movement)
    {
        // If crouch toggle ON → crouch state
        if (movement.isCrouching)
        {
            movement.SwitchState(movement.crouchstate);
            return;
        }

        // Stop moving → back to Idle
        if (movement.inputMovement.magnitude <= 0.1f)
        {
            movement.SwitchState(movement.idel);
        }
    }

    public override void ExitState(Movement movement)
    {
        movement.animator.SetBool("moving", false);
    }
}
