using UnityEngine;

namespace Unity.Game.Inventory.Model
{

    [CreateAssetMenu(fileName = "CharacterStatEnergyModifierSO", menuName = "Scriptable Objects/CharacterStatEnergyModifierSO")]
    public class CharacterStatEnergyModifierSO : CharacterStatModifierSO
    {
        public override void AffectCharacter(GameObject character, float val)
        {
            Debug.Log("CharacterStatHealthModifierSO::AffectCharacter -> VIVI! banana");
            PlayerStatController energy = character.GetComponent<PlayerStatController>();
            if (energy != null)
                energy.UpdateEnergy((int)val);
        }
    }
}
