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
        if(rb != null)
        {
            rb.isKinematic = true;
        }

        gehaltenesObjekt.transform.SetParent(holdPoint);
        gehaltenesObjekt.transform.localPosition = Vector3.zero;
        gehaltenesObjekt.transform.localRotation = Quaternion.identity;
    }

    private void LegeObjektAbInZone(AblageZone zone)
    {
        Transform zielPunkt = (zone.snapPunkt != null) ? zone.snapPunkt : zone.transform;

        gehaltenesObjekt.transform.SetParent(null);
        gehaltenesObjekt.transform.position = zielPunkt.position;
        gehaltenesObjekt.transform.rotation = zielPunkt.rotation;

        Rigidbody rb = gehaltenesObjekt.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }



        gehaltenesObjekt.tag = "Untagged";

        gehaltenesObjekt = null;

        pickupSpeereTime = 1.0f;
    }
}
