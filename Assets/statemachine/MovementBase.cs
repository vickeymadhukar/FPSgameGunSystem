

public abstract class MovementBase 
{
    public abstract void EnterState(Movement movement);

    public abstract void UpadteState(Movement movement);

    public virtual void ExitState(Movement movement) { }
}
