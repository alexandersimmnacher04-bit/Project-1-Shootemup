using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    [Header("Interact E")]
    [SerializeField] private MonoBehaviour primaryScript;
    [SerializeField] private string primaryMethode;
    [Header("Interact F")]
    [SerializeField] private MonoBehaviour secondaryScript;
    [SerializeField] private string secondaryMethode;

    [Header("Objects")]
    private Material Outline;
    private float intensity = 15.0f;
    public OpenTank openTank;
    public Camera Playercamera;
    public FirstPersonController firstPersonController;
    public bool inTriggerzone { get; private set; } = false;

  
    
   
    private void OnTriggerEnter(Collider other)
    {  
         inTriggerzone = true;
        
            if (other.CompareTag("Player"))
            {
               

                    Debug.Log("Test");
                var handle = other.GetComponentInChildren<PlayerInputHandler>();
                if (handle != null)
                    handle.SetCurrentZone(this);


            }
        
         
            //Material Outline = GetComponent<Renderer>().material;
            //Outline.SetColor("_EmissionColor", Color.red * intensity);
        
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        inTriggerzone = false;
        if (other.CompareTag("Player"))
        { Debug.Log("Test");
            var handler = other.GetComponentInChildren<PlayerInputHandler>();
            if (handler != null)
                handler.SetCurrentZone(null);

        }
        Material Outline = GetComponent<Renderer>().material;
        Outline.SetColor("_EmissionColor", Color.black);
    }

    public void TriggerPrimary()
    {
        if (!firstPersonController.windowOn)
        {
            if (primaryScript != null && !string.IsNullOrEmpty(primaryMethode))
                primaryScript.Invoke(primaryMethode, 0f);
        }
    }

    public void TriggerSecondary()
    {
        if (!firstPersonController.windowOn)
        {


            if (secondaryScript != null && !string.IsNullOrEmpty(secondaryMethode))
                secondaryScript.Invoke(secondaryMethode, 0f);
        }
    }

    private void Update()
    {
        if (inTriggerzone)
        {
            Ray ray = new Ray(Playercamera.transform.position, Playercamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, openTank.interactDistance))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    Material Outline = GetComponent<Renderer>().material;
                    Outline.SetColor("_EmissionColor", Color.red * intensity);
                }
                else
                {
                    Material Outline = GetComponent<Renderer>().material;
                    Outline.SetColor("_EmissionColor", Color.black);
                }
            }
           
        }
    }
}
