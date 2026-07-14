using JetBrains.Annotations;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera robo_cam01;
    [SerializeField] private Camera robo_cam02;

    public bool IncamMode { get; private set; } = false;
    // Hier wird gestzt auf welcher Kamera der Spieler gerde zugreift.
    public void SwitchToPlayer()
    {
        IncamMode = false;
        playerCamera.gameObject.SetActive(true);
        robo_cam01.gameObject.SetActive(false);
        robo_cam02.gameObject.SetActive(false);
    }
    public void SwitchTo01()
    {
        IncamMode = true;

        playerCamera.gameObject.SetActive(false);
        robo_cam01.gameObject.SetActive(true);
        robo_cam02.gameObject.SetActive(false);
    }
    public void SwitchTo02()
    {
        IncamMode = true;
        playerCamera.gameObject.SetActive(false);
        robo_cam01.gameObject.SetActive(false);
        robo_cam02.gameObject.SetActive(true);
    }
}
