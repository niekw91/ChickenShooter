using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model
{
    public class LevelFactory
    {
        private static Dictionary<int, BaseLevelState> levelMap = new Dictionary<int, BaseLevelState>();

        public static void Assign(BaseLevelState level)
        {
            if (level != null)
                levelMap.Add(level.ID, level);
        }

        public static BaseLevelState GetFirstLevel()
        {
            if (levelMap.Count != 0)
                return levelMap.Values.First();
            return null;
        }

        public static BaseLevelState NextLevel(BaseLevelState current)
        {
            if (current == null) return null;

            int nextLevelId = current.ID + 1;

            if (levelMap.ContainsKey(nextLevelId))
                return levelMap[nextLevelId];
            else
                return null;
        }

        public static BaseLevelState Finished()
        {
            if (levelMap.ContainsKey(-1))
                return levelMap[-1];
            else
                return null;
        }
    }
}
