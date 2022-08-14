using UnityEngine;

namespace ScriptableObjects.Badges.Active
{
    public abstract class ActiveBadge : Badge
    {
        [Tooltip("Ability cooldown time")] public float cooldownTime;
        private float _currentCooldownTime;

        protected bool CheckCanExecute()
        {
            return _currentCooldownTime <= Time.time;
        }

        public abstract void Execute();
    }
}