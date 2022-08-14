using UnityEngine;

namespace ScriptableObjects.Badges.Passive
{
    [CreateAssetMenu(fileName = "NewAddDashBadge", menuName = "ScriptableObjects/PassiveBadges/DashBadge", order = 0)]
    public class AddDashBadge : PassiveNumericBadge
    {
        public override void OnEquip(PlayerModel model)
        {
            if (IsEquipped)
                return;
            IsEquipped = true;
            model.AddDashForce(numericModifier);
        }
        public override void OnUnequip(PlayerModel model)
        {
            if (!IsEquipped)
                return;
            IsEquipped = false;
            model.AddDashForce(numericModifier * -1);
        }
    }
}