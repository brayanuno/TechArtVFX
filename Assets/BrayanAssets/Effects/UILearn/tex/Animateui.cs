using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animateui : MonoBehaviour
{
    public AnimationCurve curve;
    private Image img;
    public float animationDuration; //seconds 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            StartCoroutine(AnimProperty());
        }


    }
    private IEnumerator AnimProperty()
    {
        float timer = 0;

        while (timer < animationDuration)
        {

            float someValueFrom0To1 = timer / animationDuration;

            timer += Time.deltaTime;

            float value = curve.Evaluate(someValueFrom0To1);

            img.material.SetFloat("_NoiseIntensity", value);
            Debug.Log(someValueFrom0To1);
            Debug.Log(value);

            yield return null;
        }
    }

}
