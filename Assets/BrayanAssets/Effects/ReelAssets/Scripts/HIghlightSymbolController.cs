using UnityEngine;
using UnityEngine.VFX;

public class HIghlightSymbolController : MonoBehaviour
{
    public Animator anim;

    public VisualEffect pulseEffect;
    private VFXEventAttribute eventAttribute;

    private bool currentStateReset = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //!anim.GetBool("Attack")
        eventAttribute = pulseEffect.CreateVFXEventAttribute();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetBool("Reset", false);

            //img.material.SetTexture("_MainTex", Texture);
            ActivatePulse();
            

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetBool("Reset",true);
            //img.material.SetTexture("_MainTex", Texture);
           
            
        }
    }

    public void ActivatePulse()
    {
        anim.SetTrigger("Pulse");
        pulseEffect.SendEvent("ReplayParticle", eventAttribute);

    }




}
