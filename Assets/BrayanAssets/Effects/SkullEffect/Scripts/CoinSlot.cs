using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CoinSlot : MonoBehaviour
{
    public TextMeshProUGUI textmesh;
    public GameObject EffectHit;

    private Animator anim;

    private int currentScore = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textmesh.text = 0.ToString();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit ()
    {
        GameObject effectHit = Instantiate(EffectHit, transform);
        effectHit.transform.SetParent(this.transform);

        if (anim){
            anim.Play("hit");
        }
        
    }
    

    public void IncreaseScore ()
    {
        Debug.Log("CoinScoreCalled");
        currentScore += 20;
        textmesh.text = currentScore.ToString();
        
    }
}
