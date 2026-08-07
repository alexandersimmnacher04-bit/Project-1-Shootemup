using UnityEngine;
using UnityEngine.Video;

public class trigger_animation_crawlerfährtaushangar : MonoBehaviour
{
    
    // Ich lade erst alle Gameobjects rein. Ist eigentlich egal ob public oder private. Hab einfach private genommen, das klingt cool und geheim.
   

    [SerializeField] private GameObject industrielamep_redlight;
    [SerializeField] private GameObject industrielampe_greenlight;
    [SerializeField] private Transform torinput;
    [SerializeField] private GameObject crawlerinput;
    [SerializeField] private GameObject door01triggerzone;
    [SerializeField] private GameObject door02triggerzone;
    [SerializeField] private GameObject screenshape;

    private float animationtor = 0f;
    private float animationcrawler = 0f;
   

    // Die Start Methode ist der Trigger durch den Button an der Console.
    private void Start()
    {

        
        Debug.Log("Der Code wird gestartet");

    // Die Türen werden geschlossen
        closedoor01();
        closedoor02();


   // Das Licht ändert sich von grün in rot. 
        turngreenlightoff();
        turnredlighton();

   // Das Tor öffnet sich.            
        hangartor_animationopen();

    // Nach einer Zeit fährt der Crawler aus dem Hangar. 
        crawler_animation();

    // Wenn der Crawler aus dem Hangar gefahren ist. Geht das Tor wieder zu. 
        hangartor_animationclose();

    // Das Licht wechselt von rot auf grün, wenn das Hangar Tor geschlossen ist.
        turnredlightoff();
        turngreenlighton();

    // Und dann wenn das Licht grün ist, schaltet der Fernseher sich ein und spielt die jeweilige Animation ab. 
        //entweder:
        videosuccessanimationstart();

        //oder:
        videofailanimationstart();

    }

    // Die Update Funktion ist nur zum testen.
    private void Update()
    {
        crawler_animation();
    }


    #region Methoden Animation
    // Diese Methode startet und führt die Animation für den Crawler aus
    void crawler_animation()
    {
        animationcrawler = animationcrawler + 0.00001f;

        crawlerinput.transform.position += new Vector3(0f, 0f, animationcrawler);

    

    }

    // Diese Methode startet und führt die Animation für das Hangar Tor aus.
    void hangartor_animationopen()
    {
        while (false) //11.72
        {
            animationtor = animationtor + 0.00001f;

            torinput.transform.position += new Vector3(0f, animationtor, 0f);

            Debug.Log("Ja geht");

        }
    }

    void hangartor_animationclose()
    {
        animationtor = 11.72f;

        while (false) //11.72
        {
            animationtor = animationtor - 0.00001f;

            torinput.transform.position += new Vector3(0f, animationtor, 0f);

            Debug.Log("Ja geht");

        }




    }

    #endregion
    #region lights
    // Diese Methode schaltet das rote Industrielicht an. 
    void turnredlighton()
    {
        industrielamep_redlight.SetActive(true);

    }

    // Diese Methode schaltet das rote Industrielicht aus.
    void turnredlightoff()
    {
        industrielamep_redlight.SetActive(false);
    }

    // Diese Methode schaltet das grüne Industrielicht an.
    void turngreenlighton()
    {
        industrielampe_greenlight.SetActive(true);
    }

    //´Diese Methode schaltet das grüne Industrielicht aus. 
    void turngreenlightoff()
    {
        industrielampe_greenlight.SetActive(false);

    }
    #endregion
    #region closedoors
    void closedoor01()
    {
        door01triggerzone.SetActive(false);
    }

    void closedoor02()
    {
        door02triggerzone.SetActive(false);
    }
    #endregion
    #region videoplayer
    void videosuccessanimationstart() 
    { 
    // Es muss irgendwie erkannt werden, ob die Rätsel erfolgreich abgespielt wurden oder nicht
    }

    void videofailanimationstart()
    {

        

          

    }
    #endregion 
}
