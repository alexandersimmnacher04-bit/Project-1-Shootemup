using UnityEngine;

public class türgehtauf_script : MonoBehaviour
{
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void tuersound()
        {
            if (audioSource != null)
            {
                audioSource.Play();
                Debug.Log("Sound played");
            }
        }
    }


