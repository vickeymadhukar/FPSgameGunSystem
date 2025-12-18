using UnityEngine;

public class Crouchstate : MovementBase
{
    public override void EnterState(Movement movement)
    {
        // Enable crouch animation
        movement.animator.SetBool("crouch", true);
        movement.animator.SetBool("moving", false);
        movement.CanJump = false;
        
    }

    public override void UpadteState(Movement movement)
    {
        // If crouch toggle OFF → go back
        if (!movement.isCrouching)
        {
            if (movement.inputMovement.magnitude > 0.1f)
            {
                movement.SwitchState(movement.movingstate);
            }
            else if (movement.isProne)
            {
                movement.SwitchState(movement.pronestate);
            }
            else
            {
                movement.SwitchState(movement.idel);
            }
            return;
        }

        // Still crouching but moving → crouch + moving animation handle yahi rahega
        if (movement.inputMovement.magnitude > 0.1f)
        {
            movement.animator.SetBool("moving", true);
        }
        else
        {
            movement.animator.SetBool("moving", false);
        }
    }

    public override void ExitState(Movement movement)
    {
        movement.animator.SetBool("crouch", false);
        movement.CanJump = true;
       
    }
}
