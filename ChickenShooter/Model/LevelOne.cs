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

        private void PlayerMovement()
        {
            player.XTrajectory = player.X + Actions.Moves.PlayerMoves[0];
            player.YTrajectory = player.Y + Actions.Moves.PlayerMoves[1];
            player.XTrajectory = player.XTrajectory > game.Width - player.Size ? game.Width - player.Size : player.XTrajectory;
            player.XTrajectory = player.XTrajectory < 0 ? 0 : player.XTrajectory;
            player.YTrajectory = player.YTrajectory > game.Height - player.Size * 2 ? game.Height - player.Size : player.YTrajectory;
            player.YTrajectory = player.YTrajectory < 0 ? 0 : player.YTrajectory;
            Actions.Moves.Reset();
        }

        private void DetermineTargets(Stack<Bullet> bullets)
        {
            foreach (Bullet bullet in bullets)
            {
                foreach (Animal animal in animals)
                {
                    if (animal.IsShot(bullet.X, bullet.Y))
                    {
                        game.Score += 10;
                        hitlist.Push(animal);
                    }
                }
            }
            Actions.ShotsFired.Clear();
            // End game if no more shots left
            if (game.ShotsLeft == 0)
                NextLevel();
        }

        private void CalculateMovement()
        {
            Random rnd = new Random();
            foreach (Animal animal in animals)
            {
                double compareX = Math.Abs(animal.XPosition - player.X);
                double compareY = Math.Abs(animal.YPosition - player.Y);
                if (compareX < 10 && compareY < 10)
                    game.EndGame();

                // Horizontal movement
                if (animal.XTrajectory < game.Width - (animal.Size + 15) && animal.XTrajectory > 0)
                {
                    animal.HorizontalMovement();
                }
                else
                {
                    animal.ChangeHorizontalDirection();
                    animal.HorizontalMovement();
                }
                // Vertical movement
                if (animal.YTrajectory < game.Height - (animal.Size * 2) && animal.YTrajectory > 0)
                {
                    animal.VerticalMovement();
                }
                else
                {
                    animal.ChangeVerticalDirection();
                    animal.VerticalMovement();
                }
            }
        }
    }
}
