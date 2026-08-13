using UnityEngine;

public class Error_Sound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ErrorSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Sound played");
        }
    }
}
