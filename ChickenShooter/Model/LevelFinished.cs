using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model
{
    public class LevelFinished : BaseLevelState
    {
        public LevelFinished(Game game, string bg)
            : base(game, -1, bg)
        {
        }

        public override void Update(double dt)
        {
            game.EndGame();
        }

        public override void Render(double dt)
        {

        }
    }
}
