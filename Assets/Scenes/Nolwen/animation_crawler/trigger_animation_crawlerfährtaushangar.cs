using UnityEngine;

public class trigger_animation_crawlerfährtaushangar : MonoBehaviour
{
    Animator animator;

    public GameObject industrielamep_redlight;
    public GameObject industrielampe_greenlight;
    public AnimationClip hangar_tor_fährthoch;

    private void Start()
    {
       

        
    }

    private void Update()
    {
        animator.SetBool("opendoor", true);
    }



    void crawler_animation()
    {
        
    }
    void hangartor_animation()
    {
       
        animator.SetBool("opendoor", true);
    }
    
    void turnredlighton()
    {
        industrielamep_redlight.SetActive(true);

    }

    void turnredlightoff()
    {
        industrielamep_redlight.SetActive(false);
    }

    void turngreenlighton()
    {
        industrielampe_greenlight.SetActive(true);
    }
    void turngreenlightoff()
    {
        industrielampe_greenlight.SetActive(false);

    }
    
}
