namespace ScriptableObjects.Badges.Passive
{
    public class ActivateDashBadge : PassiveBadge
    {
        public override void OnEquip(PlayerModel model)
        {
            if (isEquipped)
                return;
            model.SetDashAbility(true);
        }

        public override void OnUnequip(PlayerModel model)
        {
            if (!isEquipped)
                return;
            model.SetDashAbility(false);
        }
    }
}