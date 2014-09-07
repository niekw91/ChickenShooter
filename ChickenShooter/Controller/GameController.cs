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
        public GameController(Game game)
        {
            this.game = game;

            game.GameView.MouseLeftButtonDown += GameView_MouseLeftButtonDown;
        }

        private void GameView_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double x = e.GetPosition(this.game.GameView).X;
            double y = e.GetPosition(this.game.GameView).Y;
            game.Shoot(x, y);
            Image obj = ((e.Source) as Image);
            if (obj != null)
            {
                game.Hit(obj);
            }
        }


    }
}
