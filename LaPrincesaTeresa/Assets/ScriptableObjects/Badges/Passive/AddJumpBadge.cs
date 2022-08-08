using UnityEngine;

[CreateAssetMenu(fileName = "NewAddJumpBadge", menuName = "ScriptableObjects/Passive Badges/JumpBadge", order = 0)]
public class AddJumpBadge : PassiveBadge
{
   // [Header("Modifier")] public int modifier;
    void Equip(PlayerModel playerModel)
    {
        playerModel.SetJumpAmount((int)modifier);
    }

    void UnEquip(PlayerModel playerModel)
    {
        playerModel.SetJumpAmount((int)modifier * -1);
    }
}