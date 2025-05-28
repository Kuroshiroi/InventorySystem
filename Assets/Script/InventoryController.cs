using UnityEngine;
using System.Collections.Generic;
using Unity.Game.Inventory.UI;
using Unity.Game.Inventory.Model;
using StarterAssets;
using System.Text;

namespace Unity.Game.Inventory.Controller
{
    /* Inventory Controlls works as bridge between InventorySO(Model) and 
     * UI_InventoryPageHandler(View), it also ensure their correct Initialization
     * InventorySO(Model) keep track of UI_InventoryPagHandler items movement in order to be in sync 
     * this way I can Identify withc item have been clicked only through its index
     */
    public class InventoryController : MonoBehaviour
    {
        private StarterAssetsInputs starterAssets;

        [SerializeField]
        private UI_InventoryPageHandler inventoryUI;

        [SerializeField]
        private InventorySO playerInventoryData;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        [SerializeField]
        private AudioClip dropAudio;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private PickUpItem pickupPrefab; 
        private void Start()
        {
            starterAssets = GetComponent<StarterAssetsInputs>();
            starterAssets.OnInventoryEvent += HandleInventoryEvent;
            PrepareUI();
            PrepareInventoryData();
        }

        // Initialize Inventory UI and get on Listen for is Events
        private void PrepareUI()
        {
            inventoryUI.InitilizeInventoryUI(playerInventoryData.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnItemActionsRequested += HandleItemActionsRequest;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnSwapItems += HandleSwapRequest;
        }

        // Initialize and populate InventorySO. It also get on Listen for is Events
        private void PrepareInventoryData()
        {
            playerInventoryData.Initialize();
            foreach (InventoryItem item in initialItems)
            {
                if (item.emptyState)
                    continue;
                playerInventoryData.AddItem(item);
            }

            playerInventoryData.OnChangedEvent += UpdateInventoryUI;
        }

        /* Function triggered from InventorySO followinf function: 
         * UpdateItem
         * AddItem
         * SwapItems
         * RemoveItems
         */
        private void UpdateInventoryUI(Dictionary<int,InventoryItem> inventoryData)
        {
            inventoryUI.ResetAllItems();
            foreach (KeyValuePair<int, InventoryItem> item in playerInventoryData.GetCurrentInventoryState())
            {
                if(item.Value.item.itemTypology == ItemTypology.Equipable) { 
                    inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity, item.Value.equipState[0].value>0);
                }
                inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
            }
        }

        //Handle the Inventory Event from InputSystem in order tho show or Hide Inventory
        public void HandleInventoryEvent()
        {
            if (inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Hide();
            }
            else
            {
                inventoryUI.Show();
                foreach (KeyValuePair<int, InventoryItem> item in playerInventoryData.GetCurrentInventoryState())
                {
                    if (item.Value.item.itemTypology == ItemTypology.Equipable)
                    {
                        inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity, item.Value.equipState[0].value > 0);
                    }
                    inventoryUI.UpdateData(item.Key, item.Value.item.ItemImage, item.Value.quantity);
                }
            }
        }


        private string PrepareDescription( InventoryItem inventoryItem)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(inventoryItem.item.Description);
            sb.AppendLine();
            /*for(int i=0; i< inventoryItem.equipState.Count; i++)
            {
                sb.Append($"{inventoryItem.equipState[i].parameter.ParameterName}" +
                   $": {inventoryItem.equipState[i].value}");
            }*/
            return sb.ToString();
        }


        public void HandleDescriptionRequest(int index)
        {
            inventoryUI.ResetSelection();
            InventoryItem inventoryItem = playerInventoryData.GetItemAt(index);
            if (inventoryItem.emptyState)
            {
                return;

            }
            ItemSO item = inventoryItem.item;
            inventoryUI.UpdateDescription(index, item.ItemImage, item.Name, PrepareDescription(inventoryItem));
        }

        /* Handle the Right Click Response for any Item in inventory 
         * It does request Any possible Action from InventorySO about the selected Item
         * Then it Add them on the correct panel through UI_InventoryPageHandler
         */
        public void HandleItemActionsRequest(int index)
        {
            InventoryItem item = playerInventoryData.GetItemAt(index);
            if (item.emptyState)
                return;
            IItemAction itemAction = (IItemAction)item.item;
            if (itemAction != null)
            {
                inventoryUI.AddAction(itemAction.ActionName, () => PerformAction(index));
            }
            IDestroyableItem itemDestroy = (IDestroyableItem)item.item;
            if (itemDestroy != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(index,item.quantity));
            }
            
            inventoryUI.ShowItemAction(index);

        }

        //Drop Method , ensure a pickup is left on the flor
        private void DropItem(int index, int quantity)
        {
            InventoryItem item = playerInventoryData.GetItemAt(index);
            pickupPrefab.SetPickup(item.item,quantity);
            Vector3 pickupPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
            Instantiate(pickupPrefab, pickupPos, Quaternion.identity);
            playerInventoryData.RemoveItem(index, quantity);
            inventoryUI.ResetSelection();
            audioSource.PlayOneShot(dropAudio);
        }

        /* Generic method to perform Action from Right Click menu
         * for Consumable Item only it ensure is removed the correct amount consumed
         */
        public void PerformAction(int index)
        {
            InventoryItem item = playerInventoryData.GetItemAt(index);
            if (item.emptyState)
                return;
            IItemAction itemAction = (IItemAction)item.item;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, item.equipState);
            }
            
            if (item.item.itemTypology == ItemTypology.Consumable)
            {
                playerInventoryData.RemoveItem(index, 1);
                audioSource.PlayOneShot(itemAction.actionSFX);
                if (playerInventoryData.GetItemAt(index).emptyState)
                {
                    inventoryUI.ResetSelection();
                }
            }
        }

        public void HandleDragging(int index)
        {
            InventoryItem item = playerInventoryData.GetItemAt(index);
            if (item.emptyState)
                return;
            inventoryUI.CreateDraggedItem(item.item.ItemImage, item.quantity);
        }

        public void HandleSwapRequest(int index1, int index2)
        {
            playerInventoryData.SwapItems(index1, index2);
        }
    }
}