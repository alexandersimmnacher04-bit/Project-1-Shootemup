using UnityEngine;

public class Sound_Robot_Arm : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundRobotArm()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            Debug.Log("Robot arm sound played");
        }
    }
}
