using UnityEngine;

namespace ScriptableObjects.Badges.Passive
{
    [CreateAssetMenu(fileName = "NewAddDashBadge", menuName = "ScriptableObjects/PassiveBadges/DashBadge", order = 0)]
    public class AddDashBadge : PassiveNumericBadge
    {
        public override void OnEquip(PlayerModel model)
        {
            if (isEquipped)
                return;
            isEquipped = true;
            model.AddDashTime(numericModifier);
        }
        public override void OnUnequip(PlayerModel model)
        {
            if (!isEquipped)
                return;
            isEquipped = false;
            model.AddDashTime(numericModifier * -1);
        }
    }
}