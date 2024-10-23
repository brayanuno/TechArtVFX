using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animateui : MonoBehaviour
{
    public AnimationCurve curve;
    private Image img;
    public float animDuration;
    
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

            
            //img.material.SetTexture("_NoiseIntensity", Curve);
            //img.SetMaterialDirty();
            //Debug.Log("wORKING");
        }

        
    }
    private IEnumerator AnimProperty ()
    {
        float time = 0;

        while (time < animDuration) {
            float rate = animDuration / 60;
            time = time + Time.deltaTime * rate ;

         
            float value = curve.Evaluate(time );

            img.material.SetFloat("_NoiseIntensity", value);
            Debug.Log(value);
            Debug.Log(time);
            yield return null;
        }
    }

}
