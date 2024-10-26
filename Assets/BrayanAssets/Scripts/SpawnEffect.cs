using UnityEngine;

public class SpawnEffect : MonoBehaviour
{
    public GameObject effectShining;
    public GameObject effectButtonPressed;

    [Range(.5f, 10f)]
    public float elapsedTime;

    private float timePassed = 0f ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > elapsedTime)
        {
            //do something
            LoopingEffect(effectShining);
          
            timePassed = 0f;
        }
        
    }

    private void LoopingEffect(GameObject effect)
    {
        GameObject copyEffect = Instantiate(effectShining, this.transform);
    }

    public void buttonPressed ()
    {
        Debug.Log("clicked");
        Instantiate(effectButtonPressed, this.transform);
    }


}
