using UnityEngine;

public class Cube : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        EventsPractice.onUpdateColor += UpdateColor;
    }

    // Update is called once per frame
    void UpdateColor(Color randomGeneratedColor)
    {
        GetComponent<MeshRenderer>().material.color = randomGeneratedColor;
    }

    
    void OnDisable()
    {
        EventsPractice.onUpdateColor -= UpdateColor;
    }
}
