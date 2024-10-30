using System.Collections;
using UnityEngine;

public class Trail : MonoBehaviour
{


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Setup(GameObject target, float duration)
    {
        StartCoroutine(Move(this.transform.position, target.transform.position, duration));
    }

    IEnumerator Move(Vector3 beginPos, Vector3 endPos, float time)

    {

        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {

            transform.position = Vector3.Lerp(beginPos, endPos, t);
            yield return null;
        }
    }
}
