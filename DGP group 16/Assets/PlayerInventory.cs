using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public GameObject currentItem;
    public Transform holdPoint;

    public void PickupItem(GameObject item)
    {
        currentItem = item;

        // Zet het item vast aan het holdPoint
        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        // Schakel physics uit zodat het object niet meer beweegt of botst
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // Zet collider naar trigger zodat het niet botst met speler
        Collider col = item.GetComponent<Collider>();
        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    public void DropCurrentItem()
    {
        if (currentItem != null)
        {
            // Ontkoppel van speler
            currentItem.transform.SetParent(null);

            // Zet physics weer aan zodat het object valt
            Rigidbody rb = currentItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Zet collider weer terug zodat het normaal botst
            Collider col = currentItem.GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = false;
            }

            // Verwijder uit inventory
            currentItem = null;
        }
    }
}
