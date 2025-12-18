using UnityEngine;

public class Aimstate : MovementBase
{
    public override void EnterState(Movement movement)
    {
        movement.animator.SetBool("shoot", true);
    }


    public override void UpadteState(Movement movement)
    {
        if (!movement.isAim)
        {

            if (movement.isCrouching)
            {
                movement.SwitchState(movement.crouchstate);
            }
            else if (movement.isProne)
            {
                movement.SwitchState(movement.pronestate);
            }
            else
            {
                movement.SwitchState(movement.idel);
            }


        }
    }
}
