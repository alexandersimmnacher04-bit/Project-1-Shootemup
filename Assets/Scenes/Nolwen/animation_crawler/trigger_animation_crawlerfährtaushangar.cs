using System.Diagnostics;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class trigger_animation_crawlerfährtaushangar : MonoBehaviour
{
    public Crawler_Play_Sound playSoundcrawler;
    public Hangar_Gate_Sound playSoundtor;
    //-----------------------------------------------------------------------

    #region Deklaration  und SerializeField
    // Ich lade erst alle Gameobjects rein. Ist eigentlich egal ob public oder private. Hab einfach private genommen, das klingt cool und geheim.


    [SerializeField] private GameObject redlight;
    [SerializeField] private GameObject greenlight;
    [SerializeField] private GameObject redlight01;
    [SerializeField] private GameObject greenlight01;
    [SerializeField] private Transform torinput;
    [SerializeField] private Transform crawlerinput;
    [SerializeField] private GameObject abdeckungtraeger;
    [SerializeField] private GameObject door01triggerzone;
    [SerializeField] private GameObject door02triggerzone;
    [SerializeField] private Transform door01pivotvorne;
    [SerializeField] private Transform door02pivothinten;
    [SerializeField] private GameObject screenshape;
    [SerializeField] private GameManager gamemanager;
    [SerializeField] private Schalter schalter;
    [SerializeField] private GameObject Satellit;

    // Variablen deklaration für die Animation
    private float animationtor = 0f;
    private float animationcrawler = 0f;
    public bool doorisopen = false;
    private bool crawlerisoutofhangar= false;
    private bool playvideoscreen = false;
    private float doorclosepos01 = -90f;
    private float doorclosepos02 = 90f;
    private bool playsoundcrawler = false;

    #endregion

    //-----------------------------------------------------------------------

    // Die Start Methode ist der Trigger durch den Button an der Console.
    private void Start()
    {

        turnredlightoff();
        turngreenlighton();



    }

// Die Update Funktion ruft die Methoden nach timing auf. 
    private void Update()
    {
        if (schalter.animationOn)
        {
            if (schalter.playsoundtor == true)
            {
                playSoundtor.HangarGateSound();
                print("Hangar Tor sound played.");
                schalter.playsoundtor = false;
            }
            abdeckungtraegervisible(); 
            if (animationtor <= 0.015f)
            {
                hangartor_animationopen();


                if (animationtor >= 0.015f)
                {
                    doorisopen = true;
                    playsoundcrawler = true;
                    satellitoff();
                }
            }
           
        }
        

        if (animationcrawler <= 0.05f && doorisopen == true)
        {
            if (playsoundcrawler == true)
            {
                playSoundcrawler.PlaySoundCrawler();
                print("Crawler sound played.");
                playsoundcrawler = false;
            }
            crawler_animation();
            
           
            if (animationcrawler >= 0.038f)
            {
                crawlerisoutofhangar = true;
                
                
            }
        }

        if (animationtor <= 0.0213f && crawlerisoutofhangar == true)
            {
                hangartor_animationclose();
            

                if (animationtor >= 0.0213f)
                {
                    playvideoscreen = true;
                
                    
                }
            }

            if (playvideoscreen == true)
            {
               gamemanager.Endgame();
               playvideoscreen = false;
            }   

            // Methoden aufruf für fail animation fehlt


            
        


    }
//-----------------------------------------------------------------------
// Die Methoden

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
            animationtor = animationtor + 0.00001f;

            torinput.transform.position += new Vector3(0f, animationtor, 0f);

    }

    void hangartor_animationclose()
    {
            animationtor = animationtor + 0.00001f;

            torinput.transform.position -= new Vector3(0f, animationtor, 0f);

    }

    
    #endregion
    #region lights
    // Diese Methode schaltet das rote Industrielicht an. 
   public void turnredlighton()
    {
        redlight.SetActive(true);
        redlight01.SetActive(true);
    }

    // Diese Methode schaltet das rote Industrielicht aus.
   public void turnredlightoff()
    {
        redlight.SetActive(false);
        redlight01.SetActive(false);
    }

    // Diese Methode schaltet das grüne Industrielicht an.
   public void turngreenlighton()
    {
        greenlight.SetActive(true);
        greenlight01.SetActive(true);
    }

    //´Diese Methode schaltet das grüne Industrielicht aus. 
  public void turngreenlightoff()
  {
        greenlight.SetActive(false);
        greenlight01.SetActive(false);

  }
    #endregion
    #region closedoors
     public void closedoor01()
    {
        
        door01triggerzone.GetComponent<BoxCollider>().enabled = false;
        door01pivotvorne.transform.eulerAngles = new Vector3(0f, doorclosepos01, 0f);

    }

   public void closedoor02()
    {
        door02triggerzone.GetComponent<BoxCollider>().enabled = false;
        door02pivothinten.transform.eulerAngles = new Vector3(0f, doorclosepos02, 0f);
    }

    
    #endregion
    #region videoplayer

   public void videosuccessanimationstart() 
    {
        //Index 0 ist der erste Video Player, das ist die succes animation deshalb true 
        // Index 1 ist der zweite Video Player, das ist die fail aniation deshalb false
        // Beide Index müssen aufgelistet sein, damit es funktioniert. 
        VideoPlayer[] animations = screenshape.GetComponents<VideoPlayer>();
        animations[0].Play();



    }

    public void videofailanimationstart()
    {
        //Index 0 ist der erste Video Player, das ist die succes animation deshalb false
        // Index 1 ist der zweite Video Player, das ist die fail aniation deshalb true
        // Beide Index müssen aufgelistet sein, damit es funktioniert. 
        VideoPlayer[] animations = screenshape.GetComponents<VideoPlayer>();
        animations[1].Play();


    }
    #endregion
    #region weitere...
    void abdeckungtraegervisible()
    {
        abdeckungtraeger.SetActive(true);
    }

   void satellitoff()
    {
        Satellit.SetActive(false);
    }
    #endregion
}
