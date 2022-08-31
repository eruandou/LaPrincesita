public abstract class PassiveBadge : Badge
{
    public bool isEquipped;
    public abstract void OnEquip(PlayerModel model);
    public abstract void OnUnequip(PlayerModel model);
}