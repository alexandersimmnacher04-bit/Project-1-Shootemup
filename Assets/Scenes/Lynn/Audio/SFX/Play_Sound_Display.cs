using UnityEngine;

public class Play_Sound_Display : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayDisplaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Display Sound played");
        }
    }
}
