using UnityEngine;

using Unity.Game.Inventory.UI;

public class UI_PickUpNotifier : MonoBehaviour
{
    [SerializeField]
    private PickUpSystem pickupSystem;

    [SerializeField]
    public GameObject uiPickupInterface;

    [SerializeField]
    public UI_InventoryItem uiItem;

    private void Start()
    {
        pickupSystem.OnPickUpAvaiable += HandlePickUpAvaiable;
    }

    private void HandlePickUpAvaiable(PickUpItem state)
    {
        if (state == null)
        {
            uiItem.ResetData();
            uiPickupInterface.SetActive(false);
            return;
        }
        uiPickupInterface.SetActive(state);
        uiItem.SetData(state.item.ItemImage, state.quantity);
        
    }
}
