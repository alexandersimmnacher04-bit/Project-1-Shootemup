using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    [SerializeField] private MonoBehaviour primaryScript;
    [SerializeField] private string primaryMethode;

    [SerializeField] private MonoBehaviour secondaryScript;
    [SerializeField] private string secondaryMethode;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            var handle = other.GetComponentInChildren<PlayerInputHandler>();
            if (handle != null)
                handle.SetCurrentZone(this);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var handler = other.GetComponentInParent<PlayerInputHandler>();
            if (handler != null)
                handler.SetCurrentZone(null);
        }
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
