using UnityEngine;

public class Pronestate : MovementBase
{
    public override void EnterState(Movement movement)
    {
        // Enable prone animation
        movement.animator.SetBool("prone", true);
        movement.animator.SetBool("crouch", false);
        movement.animator.SetBool("moving", false);
        movement.CanJump = false;
        
    }

    public override void UpadteState(Movement movement)
    {
        // If prone canceled → FSM decides where to go
        if(!movement.isProne && movement.isCrouching)
        {
            movement.animator.SetBool("crouch", true);
            movement.SwitchState(movement.crouchstate);
          
        }
        if (!movement.isProne)
        {
            if (movement.inputMovement.magnitude > 0.1f)
                movement.SwitchState(movement.movingstate);
            else
                movement.SwitchState(movement.idel);
        }
       

    }

    public override void ExitState(Movement movement)
    {
        movement.animator.SetBool("prone", false);
        movement.CanJump = true;
       
    }
}
