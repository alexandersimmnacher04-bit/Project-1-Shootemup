using UnityEngine;

public class Crawler_Play_Sound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundCrawler()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Sound played");
        }
    }
}
