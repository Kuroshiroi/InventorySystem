using UnityEngine;


namespace Unity.Game.Inventory.Model
{
    public abstract class CharacterStatModifierSO : ScriptableObject
    {
        public abstract void AffectCharacter(GameObject character, float val);
    }
}
