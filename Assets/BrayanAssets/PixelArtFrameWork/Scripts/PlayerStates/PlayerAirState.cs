using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        if (player.isGroundDetected())
            statemachine.ChangeState(player.idleState);

        
      
    }
}