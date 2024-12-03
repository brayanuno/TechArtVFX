
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine statemachine;
    protected Player player;

    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    protected string animBoolName;

    protected float stateTimer;

    public PlayerState(Player player, PlayerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;
        this.statemachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        
    }
    public virtual void update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("YVelocity", rb.linearVelocity.y);

        Debug.Log(yInput);
    }

    public virtual void ExitState()
    {
        player.anim.SetBool(animBoolName, false);
    }

}
