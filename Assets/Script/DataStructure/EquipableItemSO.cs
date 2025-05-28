using UnityEngine;
using System.Collections.Generic;

namespace Unity.Game.Inventory.Model
{

    public enum EquipCategory
    {
        weapon,
        shield,
        helmet,
        accessory
    }

    [CreateAssetMenu(fileName = "EquipableItemSO", menuName = "Scriptable Objects/EquipableItemSO")]
    public class EquipableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "Equip";

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        [field: SerializeField]
        public EquipCategory category { get; private set; }
        
        [field: SerializeField]
        public GameObject itemPrefab { get; private set; }

        public bool PerformAction(GameObject character, List<EquipParameter> itemState = null)
        {
            AgentWeapon weaponSystem = character.GetComponent<AgentWeapon>();
            if(weaponSystem != null)
            {
                weaponSystem.SetWeapon(this, itemState == null ? null : itemState);
                return true;
            }
            return false;
        }


    }
}
