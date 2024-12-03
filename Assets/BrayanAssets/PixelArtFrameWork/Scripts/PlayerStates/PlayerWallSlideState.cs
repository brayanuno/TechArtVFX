using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("enteredWallSlideMode");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void update()
    {
        base.update();

        if(xInput != 0 && player.facingDirection != xInput)
        {
            statemachine.ChangeState(player.idleState);
        }
        //if pressing down move normally 
        if (yInput < 0)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        } else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y * .7f);
        }

        if (player.isGroundDetected())
        {
            statemachine.ChangeState(player.idleState);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
}
