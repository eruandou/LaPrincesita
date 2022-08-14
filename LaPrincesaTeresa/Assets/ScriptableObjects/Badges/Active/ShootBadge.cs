using UnityEngine;

namespace ScriptableObjects.Badges.Active
{
    [CreateAssetMenu(fileName = "NewShootBadge", menuName = "ScriptableObjects/Badges/ActiveBadge/ShootBadge",
        order = 0)]
    public class ShootBadge : ActiveBadge
    {
        public override void Execute()
        {
            if (CheckCanExecute())
            {
#if UNITY_EDITOR
                Debug.Log("Shoot");
#endif
            }
        }
    }
}