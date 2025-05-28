using UnityEngine;


namespace Unity.Game.Inventory.Model
{
    [CreateAssetMenu(fileName = "CharacterStatHealthModifierSO", menuName = "Scriptable Objects/CharacterStatHealthModifierSO")]
    public class CharacterStatHealthModifierSO : CharacterStatModifierSO
    {
        public override void AffectCharacter(GameObject character, float val)
        {
            Debug.Log("CharacterStatHealthModifierSO::AffectCharacter -> VIVI! banana");
            PlayerStatController health = character.GetComponent<PlayerStatController>();
            if (health != null)
                health.UpdateHealth((int)val);
        }
    }
}
