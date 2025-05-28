using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Unity.Game.Inventory.Model
{
    public enum ItemTypology
    {
        Consumable, 
        Equipable
    }

    public abstract class ItemSO : ScriptableObject
    {
        public int MyProperty { get; set; }

        [field: SerializeField]
        public ItemTypology itemTypology;

        [field: SerializeField]
        public bool IsStackable { get; set; }

        public int ID => GetInstanceID();

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }

        [field: SerializeField]
        public List<EquipParameter> DefaultParametersList { get; set; }
    }
    public interface IDestroyableItem
    {

    }
    // Interface that define the possibiliti to Right Click One Action specific
    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<EquipParameter> itemState = null);
    }

    /* Type used mostly for ConsumableItemSO, 
     * it packages a Possible StatModifier with its value of effect 
     */
    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifierSO statModifer;
        public float value;
    }

    /*Type used mostly for EquipableItemSO, 
     * it packages a Possible EquipParameter with its value of effect 
     * this may be damage, defense , durability , etc and can be changed run time
     */
    [Serializable]
    public class EquipParameter : IEquatable<EquipParameter>
    {
        public EquipItemParametersSO parameter;
        public float value;

        public bool Equals(EquipParameter other)
        {
            return other.parameter == parameter;
        }
    }

}
