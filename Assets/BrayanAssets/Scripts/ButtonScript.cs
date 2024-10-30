using System.Collections;
using UnityEngine;
using UnityEngine.UI;



public class ButtonScript : MonoBehaviour
{


    public AnimationCurve curve;
    public float animationDuration;
    private Image img;
    public string PropertyName;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        PropertyName = "_" + PropertyName;

        StarAnimation();

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void StarAnimation()
    {

        StartCoroutine(AnimProperty());
        //StartCoroutine(ExecuteAfterTime(animationDuration));
    }

    private IEnumerator AnimProperty()
    {

        float timer = 0;

        while (timer < animationDuration)
        {

            float someValueFrom0To1 = timer / animationDuration;

            timer += Time.deltaTime;

            float value = curve.Evaluate(someValueFrom0To1);

            Material mat = new Material(GetComponent<Image>().material);


            this.GetComponent<Image>().material = mat;
            this.GetComponent<Image>().material.SetFloat(PropertyName, value);



            yield return null;
        }

        if (gameObject)
        {
            if (timer >= animationDuration)
            {
                Destroy(gameObject);
            }
        }

    }
    //IEnumerator ExecuteAfterTime(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    Debug.Log(time);
    //    //Destroy(gameObject);
    //}

}
