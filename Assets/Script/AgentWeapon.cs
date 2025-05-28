using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Game.Inventory.Model;
using System.Linq;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField]
    private List<EquipableItemSO> weapon = new List<EquipableItemSO>();

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private List<EquipParameter> parametersToModify;

    [SerializeField]
    private List<List<EquipParameter>> itemCurrentState = new List<List<EquipParameter>>();

    [SerializeField]
    public List<Transform> equipPoints = new List<Transform>();

    const string EQUIP_POINT_TAG = "EquipPoint";
    const int EQUIP_WEAPON = 0;
    const int EQUIP_SHIELD = 1;
    const int EQUIP_HAT = 2;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            weapon.Add(null);
            itemCurrentState.Add(new List<EquipParameter>());
        }
    }
    /*Let's Talk... 
     * This class is just a Proto...ProtoProtoType of an EquipmentSystem
     * I just limit myslef to spawn the correct prefabs in the corrects Transform
     * It doesn't Truck correctly the state of the Item once is Equiped having some major bug
     * The bigest problem was that I didn't want it to know about InventoryItem directly
     * (since out of scope of the system), so I tried to share a list of Parameters wich 
     * have been very very flexible... but so harsh to track with consistency in all the MVC model.
     * 
     */
    public void SetWeapon(EquipableItemSO weaponItem, List<EquipParameter> itemState)
    {
        int cat = 0;

        switch (weaponItem.category)
        {
            case EquipCategory.weapon:
                cat = 0;
                break;
            case EquipCategory.shield:
                cat = 1;
                break;
            case EquipCategory.helmet:
                cat = 2;
                break;
                //case EquipCategory.accessory: break;
        }
        if (weapon[cat]!=null)
        {
            itemCurrentState[cat][0].value = 0;
            inventoryData.UpdateItem(weapon[cat], 1, itemCurrentState[cat]);
        }
        
        Transform currentEquipPoint = equipPoints[cat];
        foreach (Transform child in currentEquipPoint)
        {
            Destroy(child.gameObject);
        }
        GameObject obj = Instantiate(weaponItem.itemPrefab, currentEquipPoint);
        obj.transform.parent = currentEquipPoint;

        weapon[cat] = weaponItem;
        itemCurrentState[cat] = new List<EquipParameter>(itemState);
        ModifyParameters(cat );
    }

    private void ModifyParameters(int cat)
    {
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrentState[cat].Contains(parameter))
            {
                int index = itemCurrentState[cat].IndexOf(parameter);
                
                // should be clamp
                float value = parameter.value;
                itemCurrentState[cat][index] = new EquipParameter
                {
                    parameter = parameter.parameter,
                    value = value,
                };
                inventoryData.UpdateItem(weapon[cat], 1, itemCurrentState[cat]);
                

            }
        }
    }
}
