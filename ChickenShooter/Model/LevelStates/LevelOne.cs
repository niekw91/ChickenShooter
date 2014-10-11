using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model
{
    public class LevelOne : BaseLevelState
    {
        public LevelOne(Game game, string bg)
            : base(game, 1, bg)
        {
        }

        public override void Update(double dt)
        {
            // Remove animals from list that are on the hitlist
            DetermineTargets(Actions.ShotsFired);

            // Move player
            PlayerMovement();
            
            // Move animals
            CalculateMovement();
        }

        public override void Render(double dt)
        {
            if (!game.gameOver)
            {
                // Remove animals in hitlist from screen
                while (hitlist.Count != 0)
                    game.GameView.Remove(hitlist.Pop().Image);

                game.GameView.Render(animals, player);
            }
        }


    }
}
