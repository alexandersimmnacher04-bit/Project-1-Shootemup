using UnityEngine;

public class Hangar_Gate_Sound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void HangarGateSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Sound played");
        }
    }
}

