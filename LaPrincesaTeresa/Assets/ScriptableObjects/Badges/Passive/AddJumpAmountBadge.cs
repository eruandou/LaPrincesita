using UnityEngine;

namespace ScriptableObjects.Badges.Passive
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PassiveBadges/JumpBadge")]
    public class AddJumpAmountBadge : PassiveNumericBadge
    {
        public override void OnEquip(PlayerModel playerModel)
        {
            playerModel.AddMaxJumps((int)numericModifier);
        }

        public override void OnUnequip(PlayerModel playerModel)
        {
            playerModel.AddMaxJumps((int)numericModifier * -1);
        }
    }
}