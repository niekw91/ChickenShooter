using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChickenShooter.Model
{
    public abstract class BaseLevelState
    {
        protected Game game;
        protected string bg;

        public int ID { get; set; }

        protected List<Animal> animals { get { return game.Animals; } }
        protected Stack<Animal> hitlist { get { return game.Hitlist; } }
        protected List<Breed> breeds { get { return game.Breeds; } }
        protected Player player { get { return game.Player; } }
        protected ActionContainer Actions { get { return game.Actions; } }

        public BaseLevelState() { }

        public BaseLevelState(Game game, int id, string bg)
        {
            this.game = game;
            ID = id;
            this.bg = bg;
            LevelFactory.Assign(this);
        }

        public abstract void Update(double dt);
        public abstract void Render(double dt);


        public void LoadGraphics()
        {
            // Set level background
            game.GameView.SetBackground(bg);
            // Draw objects
            if (animals != null)
            {
                game.GameView.Initialize(animals, player);
            }
        }

        protected void NextLevel()
        {
            BaseLevelState next = LevelFactory.NextLevel(this);

            if (next == null)
                next = LevelFactory.Finished();

            game.SwitchLevel(next);
        }

        protected void PlayerMovement()
        {
            player.XTrajectory = player.X + Actions.Moves.PlayerMoves[0];
            player.YTrajectory = player.Y + Actions.Moves.PlayerMoves[1];
            player.XTrajectory = player.XTrajectory > game.Width - player.Size ? game.Width - player.Size : player.XTrajectory;
            player.XTrajectory = player.XTrajectory < 0 ? 0 : player.XTrajectory;
            player.YTrajectory = player.YTrajectory > game.Height - player.Size * 2 ? game.Height - player.Size : player.YTrajectory;
            player.YTrajectory = player.YTrajectory < 0 ? 0 : player.YTrajectory;
            Actions.Moves.Reset();
        }

        protected void DetermineTargets(Stack<Bullet> bullets)
        {
            foreach (Bullet bullet in bullets)
            {
                foreach (Animal animal in animals)
                {
                    if (animal.IsShot(bullet.X, bullet.Y))
                    {
                        game.Score += 10;
                        animal.Shot = true;
                        hitlist.Push(animal);
                    }
                }
            }
            Actions.ShotsFired.Clear();
            // End game if no more shots left
            if (game.ShotsLeft == 0)
                NextLevel();
        }

        protected void CalculateMovement()
        {
            Random rnd = new Random();
            foreach (Animal animal in animals)
            {
                double compareX = Math.Abs(animal.XPosition - player.X);
                double compareY = Math.Abs(animal.YPosition - player.Y);
                if (compareX < 10 && compareY < 10 && !animal.Shot)
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
