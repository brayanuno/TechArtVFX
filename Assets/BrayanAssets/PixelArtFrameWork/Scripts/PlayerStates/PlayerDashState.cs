using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration;
    }

    public override void ExitState()
    {
        base.ExitState();

        player.SetVelocity(0, rb.linearVelocity.y);
    }

    public override void update()
    {
        base.update();

        player.SetVelocity(player.dashSpeed * player.facingDirection, 0);

        if (stateTimer < 0)
           
            statemachine.ChangeState(player.idleState);

    }


}
