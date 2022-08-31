using UnityEngine;

namespace ScriptableObjects.Badges.Passive
{
    [CreateAssetMenu(fileName = "NewAddJumpBadge", menuName = "ScriptableObjects/PassiveBadges/JumpBadge", order = 0)]
    public class AddJumpBadge : PassiveNumericBadge
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