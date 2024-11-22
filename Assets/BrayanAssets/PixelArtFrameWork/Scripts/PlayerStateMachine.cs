using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class PlayerStateMachine 
{
    public PlayerState currentSate { get; private set;}


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Initialize(PlayerState startSate)
    {
        currentSate = startSate;
        currentSate.Enter();

    }

    public void ChangeState(PlayerState newState)
    {
        currentSate.ExitState();
        currentSate = newState;
        currentSate.Enter();
    }

}
