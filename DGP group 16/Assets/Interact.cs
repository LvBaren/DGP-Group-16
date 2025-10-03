using UnityEngine;

public class Interact : MonoBehaviour
{
    public PlayerInventory inventory;
    public float pickupRange = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TryPickupOrSwap();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DropItem();
        }
    }

    void TryPickupOrSwap()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, pickupRange);
        foreach (Collider hit in hits)
        {
            PickupItem item = hit.GetComponent<PickupItem>();
            if (item != null)
            {
                if (inventory.currentItem != null)
                {
                    inventory.DropCurrentItem();
                }

                inventory.PickupItem(hit.gameObject);
                return;
            }
        }
    }

    void DropItem()
    {
        if (inventory.currentItem != null)
        {
            inventory.DropCurrentItem();
        }
    }
}
