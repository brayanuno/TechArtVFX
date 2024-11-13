using UnityEngine;
using UnityEngine.VFX;

public class Reel : MonoBehaviour
{
    public GameObject[] SlotSymbol;

    public GameObject[] VisibleSymbolParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnParticle();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnParticle()
    {
        for (int i = 0; i < VisibleSymbolParent.Length; i++)
        {
            Instantiate(SlotSymbol[i], VisibleSymbolParent[i].transform);
        }
    }
}
