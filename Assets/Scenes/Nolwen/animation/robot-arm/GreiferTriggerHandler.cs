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
        // TEST 1: Reagiert der Trigger überhaupt auf Irgendwas?
        Debug.Log("Trigger berührt: " + other.gameObject.name + " mit Tag: " + other.tag);

        if (gehaltenesObjekt == null && pickupSpeereTime <= 0f)
        {
            if (other.CompareTag("Greifbar"))
            {
                Debug.Log("Greifbares Objekt erkannt -> Aufnehmen gestartet!");
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

        gehaltenesObjekt = null;

        pickupSpeereTime = 1.0f;
    }
}
