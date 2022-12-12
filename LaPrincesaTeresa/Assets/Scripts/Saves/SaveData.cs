using System;
using System.Collections.Generic;

namespace Saves
{
    [Serializable]
    public struct SaveData
    {
        public bool doubleJump;
        public bool glide;
        public bool dash;
        public List<string> unlockedLevels;

        public void UnlockDoubleJump()
        {
            doubleJump = true;
        }

        public void UnlockGlide()
        {
            glide = true;
        }

        public void UnlockDash()
        {
            dash = true;
        }

        public void UnlockLevel(string levelID)
        {
            if (unlockedLevels.Contains(levelID))
                return;
            unlockedLevels.Add(levelID);
        }
    }
}