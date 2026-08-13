using UnityEngine;

public class Gate_Close_Sound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GateCloseSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Sound played");
        }
    }
}
