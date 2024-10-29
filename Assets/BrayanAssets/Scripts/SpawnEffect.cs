using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SpawnEffect : MonoBehaviour
{
    public GameObject effectShining;
    public GameObject effectButtonPressed;

    public GameObject childLooping;


    [Range(.5f, 10f)]
    public float elapsedTime;

    private float timePassed = 0f ;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameObject childLooping = transform.Find("LoopingEffect").gameObject;

    }

    // Update is called once per frame
    void Update()
    {   
        //Looping
        //timePassed += Time.deltaTime;
        //if (timePassed > elapsedTime)
        //{
        //    //do something
        //    LoopingEffect(effectShining);
          
        //    timePassed = 0f;
        //}
        
    }

    private void LoopingEffect(GameObject effect)
    {
        GameObject copyEffect = Instantiate(effectShining, this.transform);
    }

    public void buttonPressed ()
    {
        Debug.Log("clicked");

        
        Instantiate(effectButtonPressed, this.transform);
        childLooping.transform.SetAsLastSibling();

        effectButtonPressed.transform.SetAsFirstSibling();

        //GameObject copyEffect = Instantiate(effectShining, this.transform);
    }


}
