using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Unity.Game.Inventory.Model;
using StarterAssets;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField]
    private InventorySO invetoryData;

    private StarterAssetsInputs starterAssets;

    private List<PickUpItem> pickableItems = new List<PickUpItem>();

    public event Action<PickUpItem> OnPickUpAvaiable;

    private void Start()
    {
        starterAssets = GetComponent<StarterAssetsInputs>();
        starterAssets.OnInteractEvent += pickupItem;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PickUpSystem::OnTriggerEnter -> " +other.name);
        if (other.gameObject.CompareTag("PickUp"))
        {
            PickUpItem pickupItem = other.GetComponent<PickUpItem>();
            if (pickableItems.Count == 0)
                OnPickUpAvaiable?.Invoke(pickupItem);
            pickableItems.Add(pickupItem);
            
        }

    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("PickUpSystem::OnTriggerExit -> " + other.name);

        if (other.gameObject.CompareTag("PickUp"))
        {
            PickUpItem pickupItem = other.GetComponent<PickUpItem>();
            pickableItems.Remove(pickupItem);
            if (pickableItems.Count == 0)
                OnPickUpAvaiable?.Invoke(null);
            else
            {
                OnPickUpAvaiable?.Invoke(pickableItems[0]);
            }

        }

    }

    private void pickupItem()
    {
        /*Collider[] others = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (Collider other in others)
        {
            if (other.gameObject.CompareTag("PickUp"))
            {       PickUpItem pickupItem = other.GetComponent<PickUpItem>();
                
            }
        }
        */
        Debug.Log("PickUpSystem::pickupItem -> " + pickableItems.Count);

        if (pickableItems.Count > 0)
        {
            PickUpItem pickupItem = pickableItems[0];
            pickableItems.RemoveAt(0);
            int reminder = invetoryData.AddItem(pickupItem.item, pickupItem.quantity);
            if (reminder == 0)
            {
                pickupItem.DestroyItem();
                if (pickableItems.Count == 0)
                {
                    OnPickUpAvaiable?.Invoke(null);
                }
                else {
                    OnPickUpAvaiable?.Invoke(pickableItems[0]);
                }
            }
            else
            {
                pickableItems.Add(pickupItem);
                pickupItem.quantity = reminder;
            }
        }
    }
}
