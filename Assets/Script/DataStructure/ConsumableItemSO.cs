using UnityEngine;
using System;
using System.Collections.Generic;

namespace Unity.Game.Inventory.Model
{
    [CreateAssetMenu(fileName = "ConsumableItemSO", menuName = "Scriptable Objects/ConsumableItemSO")]
    public class ConsumableItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [field: SerializeField]
        List<ModifierData> modifiersData = new List<ModifierData>();

        
        public string ActionName => "Use";

        [field: SerializeField]
        public AudioClip actionSFX {get; private set;}

        public bool PerformAction(GameObject character, List<EquipParameter> itemState = null)
        {
            foreach(ModifierData modifier in modifiersData)
            {
                modifier.statModifer.AffectCharacter(character, modifier.value);
            }
            return false;
        }
    }


}