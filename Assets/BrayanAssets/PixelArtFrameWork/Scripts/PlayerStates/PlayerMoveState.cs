using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void update()
    {
        base.update();

        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);
        
        if (xInput == 0)
        {
            statemachine.ChangeState(player.idleState);
        }
    }
}
