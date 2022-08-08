using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAddDashBadge", menuName = "ScriptableObjects/Passive Badges/DashBadge", order = 0)]
public class AddDashBadge : PassiveBadge
{
    
    // [Header("Name")] public string name;
    // [Tooltip("This describes what the badge does")]
    // [Header("Description")] public string description;

    // [Header("Modifier")] public float modifier;

    private bool _equiped;
    
    void Equip(PlayerModel playerModel)
    {
        if (!_equiped)
        {
            playerModel.SetDashForce(modifier);
            _equiped = !_equiped;
        }
        
    }

    void UnEquip(PlayerModel playerModel)
    {
        playerModel.SetDashForce(modifier * -1);
    }
}