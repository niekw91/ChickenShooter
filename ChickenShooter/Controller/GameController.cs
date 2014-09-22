using ChickenShooter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChickenShooter.Controller
{
    public class GameController
    {
        private Game game;
        public MoveController Move { get; set; }

        public GameController(Game game)
        {
            this.game = game;
            Move = new MoveController(game.Actions);
            game.Actions.AddMovesController(Move);
        }

        public void Shoot(double x, double y)
        {
            if (game.ShotsLeft != 0)
            {
                --game.ShotsLeft;
                game.Actions.ShotsFired.Push(new Bullet(x, y));
            }
        }


    }
}
