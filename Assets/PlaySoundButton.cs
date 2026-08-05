using UnityEngine;

public class PlaySoundButton : MonoBehaviour
{
  private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Sound played");
        }
    }
}
