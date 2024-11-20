using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventsPractice : MonoBehaviour
{

    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        onPlayerDeath = null;
    }

    private void Update()
    {
        if (onPlayerDeath != null)
        {
            onPlayerDeath?.Invoke();
            EventsPractice.onPlayerDeath = null;
        }
    }
    void PlayerDeathFunction ()

    {
        //kill the player
    }
}
