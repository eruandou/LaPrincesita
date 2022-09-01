using UnityEngine;

namespace ScriptableObjects.Badges.Passive
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PassiveBadges/DashBadge")]
    public class AddDashTimeBadge : PassiveNumericBadge
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