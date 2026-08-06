using UnityEngine;

public class Play_Sound_Tank_Fill : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayFillFuelSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Fill fuel sound played");
        }
    }
}
