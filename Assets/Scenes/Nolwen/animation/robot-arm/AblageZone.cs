using UnityEngine;

public class AblageZone : MonoBehaviour
{
    public Transform snapPunkt;

    private void OnDrawGizmos()
    {
        Vector3 pos = (snapPunkt != null) ? snapPunkt.position : transform.position;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(pos, new Vector3(0.3f, 0.3f, 0.3f));
    }
}
