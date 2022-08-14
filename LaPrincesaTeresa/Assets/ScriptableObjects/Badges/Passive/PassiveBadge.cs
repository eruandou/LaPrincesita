public abstract class PassiveBadge : Badge
{
    public bool IsEquipped;
    public abstract void OnEquip(PlayerModel model);
    public abstract void OnUnequip(PlayerModel model);
}