using UnityEngine;
using UnityEngine.UI;

public class Animateui : MonoBehaviour
{
    public AnimationCurve Curve;
    private Image img;
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
            Curve.Evaluate()

            img.material.SetTexture("_NoiseIntensity", Curve);
            img.SetMaterialDirty();

            Debug.Log("wORKING");
        }
    }
}
