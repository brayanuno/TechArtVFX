using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventsPractice : MonoBehaviour
{
    

    public static event Action<Color> onUpdateColor;

    [ContextMenu("Get Cubes")]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void ChangeCubes()
    {
        if (onUpdateColor != null)
        {
            onUpdateColor.Invoke(UnityEngine.Random.ColorHSV());
        }


    }

    IEnumerator ChangeCubesRoutine(Action onComplete = null)
    {
        yield return new WaitForSeconds(5.0f);

        onComplete?.Invoke();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ChangeCubes();
            StartCoroutine(ChangeCubesRoutine(() => printMessage() ));
        }
    }

    public void printMessage ()
    {
        Debug.Log("FiveSeconds passed");
    }

    public void RemoveHealth(float amount)
    {

    }
}


