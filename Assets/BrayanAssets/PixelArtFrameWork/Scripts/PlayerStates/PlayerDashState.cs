using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 1.5f;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void update()
    {
        base.update();

        if (stateTimer < 0)
            statemachine.ChangeState(player.idleState);



    }


}
