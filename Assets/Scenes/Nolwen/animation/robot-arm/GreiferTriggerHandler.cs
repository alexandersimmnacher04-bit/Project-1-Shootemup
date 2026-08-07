using UnityEngine;

public class GreiferTriggerHandler : MonoBehaviour
{
    [Header("Referenzen")]
    [SerializeField] private Transform holdPoint;

    private GameObject gehaltenesObjekt = null;
    private float pickupSpeereTime = 0f;

    private void Update()
    {
        if (pickupSpeereTime > 0f)
        {
            pickupSpeereTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gehaltenesObjekt == null && pickupSpeereTime <= 0f)
        {
            if (other.CompareTag("Greifbar"))
            {
                GreifObjekt(other.gameObject);
                return;
            }
        }

        if (gehaltenesObjekt != null)
        {
            AblageZone zone = other.GetComponent<AblageZone>();
            if (zone != null)
            {
                LegeObjektAbInZone(zone);
            }
        }
    }

    private void GreifObjekt(GameObject obj)
    {
        gehaltenesObjekt = obj;

        gehaltenesObjekt.layer = LayerMask.NameToLayer("Default");

        Rigidbody rb = gehaltenesObjekt.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = gehaltenesObjekt.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (gehaltenesObjekt != null && holdPoint != null)
        {
            gehaltenesObjekt.transform.position = holdPoint.position;
            gehaltenesObjekt.transform.rotation = holdPoint.rotation * Quaternion.Euler(0f, 0f, 90f);
        }
    }

    private void LegeObjektAbInZone(AblageZone zone)
    {
        Transform zielPunkt = (zone.snapPunkt != null) ? zone.snapPunkt : zone.transform;

        gehaltenesObjekt.transform.SetParent(null);
        gehaltenesObjekt.transform.position = zielPunkt.position;
        gehaltenesObjekt.transform.rotation = zielPunkt.rotation * Quaternion.Euler(90f, 0f, 0f);

        Rigidbody rb = gehaltenesObjekt.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Collider col = gehaltenesObjekt.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        gehaltenesObjekt.tag = "Untagged";

        zone.RegistriereAblage();

        gehaltenesObjekt = null;

        pickupSpeereTime = 1.0f;
    }
}
