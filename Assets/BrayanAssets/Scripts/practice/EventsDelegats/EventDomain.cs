using UnityEngine;
using UnityEngine.Events;

public class EventDomain : MonoBehaviour
{
    public UnityEvent MyUnityEvent;
    
    delegate void MyDelegate();
    MyDelegate attack;
    
    void Start()
    {
        attack += PrimaryAttack;
        attack += SecondaryAttack;
        /*MyUnityEvent.AddListener(PrintMessage);

        MyUnityEvent.Invoke();*/
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void PrintMessage()
    {
        Debug.Log("UnityEvent triggered!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (attack != null)
            {
                attack();
            }
        }
    }
    
    void PrimaryAttack()
    {
        // Primary attack
    }

    void SecondaryAttack()
    {
        // Secondary attack
    }
}
