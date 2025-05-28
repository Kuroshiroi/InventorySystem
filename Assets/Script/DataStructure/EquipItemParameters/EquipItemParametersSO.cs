using UnityEngine;
using System;
using System.Collections.Generic;

namespace Unity.Game.Inventory.Model
{
    [CreateAssetMenu(fileName = "EquipItemParametersSO", menuName = "Scriptable Objects/EquipItemParametersSO")]
    public class EquipItemParametersSO : ScriptableObject
    {
        [field: SerializeField]
        public string ParameterName { get; set; }
    }
}
