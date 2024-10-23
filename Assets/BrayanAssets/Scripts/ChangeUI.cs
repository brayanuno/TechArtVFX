using UnityEngine;
using UnityEngine.UI;

public class ChangeUI : MonoBehaviour
{

    public Texture2D Texture;
    public Material NewMat;

    private Image img;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //img.material.SetTexture("_MainTex", Texture);
            img.SetMaterialDirty();

            
        }
    }
}
