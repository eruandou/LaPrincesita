using System;
using System.Collections.Generic;

namespace Saves
{
    [Serializable]
    public struct SaveData
    {
        public bool DoubleJump;
        public bool Glide;
        public bool Dash;
        public List<string> unlockedLevels;

        public void UnlockDoubleJump()
        {
            DoubleJump = true;
        }

        public void UnlockGlide()
        {
            Glide = true;
        }

        public void UnlockDash()
        {
            Dash = true;
        }

        public void UnlockLevel(string levelID)
        {
            if (unlockedLevels.Contains(levelID))
                return;
            unlockedLevels.Add(levelID);
        }
    }
}