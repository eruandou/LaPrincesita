using UnityEngine;

public abstract class PassiveBadge : Badge
{
    [HideInInspector] public bool isEquipped;
    public abstract void OnEquip(PlayerModel model);
    public abstract void OnUnequip(PlayerModel model);
}