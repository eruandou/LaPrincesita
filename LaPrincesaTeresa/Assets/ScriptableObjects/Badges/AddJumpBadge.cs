using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAddJumpBadge", menuName = "ScriptableObjects/Player/Passive Badge", order = 0)]
public class AddJumpBadge : ScriptableObject
{
    
    [Header("Name")] public string name;
    [Tooltip("This describes what the badge does")]
    [Header("Description")] public string description;

    [Header("Modifier")] public int modifier;
    
    void Equip(PlayerModel playerModel)
    {
        playerModel.SetJumpAmount(modifier);
    }

    void UnEquip(PlayerModel playerModel)
    {
        playerModel.SetJumpAmount(modifier * -1);
    }
}