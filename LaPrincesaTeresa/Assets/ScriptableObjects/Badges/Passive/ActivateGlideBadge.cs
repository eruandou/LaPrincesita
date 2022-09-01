using UnityEngine;

namespace ScriptableObjects.Badges.Passive
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PassiveBadges/GlideBadge")]
    public class ActivateGlideBadge : PassiveBadge
    {
        public override void OnEquip(PlayerModel model)
        {
            if (isEquipped)
                return;
            model.SetGlideAbility(true);
        }

        public override void OnUnequip(PlayerModel model)
        {
            if (!isEquipped)
                return;
            model.SetGlideAbility(false);
        }
    }
}