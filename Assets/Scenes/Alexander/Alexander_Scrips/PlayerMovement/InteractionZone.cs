using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    [Header("Interact E")]
    [SerializeField] private MonoBehaviour primaryScript;
    [SerializeField] private string primaryMethode;
    [Header("Interact F")]
    [SerializeField] private MonoBehaviour secondaryScript;
    [SerializeField] private string secondaryMethode;

    [Header("Highlight")]
    public HighlightObject highlightObject;
    private Material Outline;
    private float intensity = 10.0f;
    public bool inTriggerzone { get; private set; } = false;
  
    
   
    private void OnTriggerEnter(Collider other)
    {  
            if (other.CompareTag("Player"))
            {
                Debug.Log("Test");
                var handle = other.GetComponentInChildren<PlayerInputHandler>();
                if (handle != null)
                    handle.SetCurrentZone(this);

            }
         
            Material Outline = GetComponent<Renderer>().material;
            Outline.SetColor("_EmissionColor", Color.red * intensity);
        
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        
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
        if (primaryScript != null && !string.IsNullOrEmpty(primaryMethode))
            primaryScript.Invoke(primaryMethode, 0f);
    }

    public void TriggerSecondary()
    {
        if (secondaryScript != null && !string.IsNullOrEmpty(secondaryMethode))
            secondaryScript.Invoke(secondaryMethode, 0f);
    }

    
}
