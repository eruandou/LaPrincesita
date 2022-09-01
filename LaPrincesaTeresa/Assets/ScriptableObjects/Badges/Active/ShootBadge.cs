using UnityEngine;

namespace ScriptableObjects.Badges.Active
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Badges/ActiveBadge/ShootBadge")]
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