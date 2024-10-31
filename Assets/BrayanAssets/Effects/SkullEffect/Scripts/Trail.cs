using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Trail : MonoBehaviour
{
    public float waitToDestroy;
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

    void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<CoinSlot>().IncreaseScore();
        other.gameObject.GetComponent<CoinSlot>().Hit();

        float entered =  waitToDestroy;
        StartCoroutine(WaitForSeconds(entered));
    }

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
