using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
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

        //always check to jump
        if(Input.GetKeyDown(KeyCode.Space) && player.isGroundDetected())
        {
            statemachine.ChangeState(player.jumpState);
        }
    }
}
