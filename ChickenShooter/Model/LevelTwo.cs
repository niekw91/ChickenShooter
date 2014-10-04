using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model
{
    public class LevelTwo : BaseLevelState
    {
        // private readonly string ANIMAL_FILE = "animals.json";

        //private List<Animal> animals;
        //private Stack<Animal> hitlist;
        //private List<Breed> breeds;

        //public Player player { get; set; }

        public LevelTwo(Game game, string bg)
            : base(game, 2, bg)
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
                game.EndGame();
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

        //private void KillTargets()
        //{
        //    foreach (Animal animal in hitlist)
        //    {
        //        animals.Remove(animal);
        //    }
        //    // End game if all animals are dead
        //    if (animals.Count == 0)
        //        game.EndGame();
        //}

        //public override void LoadGraphics()
        //{
        //    if (animals != null)
        //    {
        //        game.GameView.Initialize(animals, player);
        //    }
        //}

        //public override void InitializeGameObjects(int animalCount)
        //{
        //    animals = new List<Animal>();
        //    hitlist = new Stack<Animal>();
        //    // Create player
        //    player = new Player();

        //    Random rnd = new Random();
        //    for (int i = 0; i < animalCount; i++)
        //    {
        //        // Generate random index
        //        int ind = rnd.Next(0, breeds.Count);
        //        // Create animal using factory
        //        Animal animal = breeds[ind].CreateAnimal();
        //        // Set x and y
        //        animal.XPosition = rnd.Next(animal.Size * 2, game.Width - animal.Size * 2);
        //        animal.YPosition = rnd.Next(animal.Size * 2, game.Height - animal.Size * 2);

        //        animals.Add(animal);
        //    }
        //}

        ///// <summary>
        ///// Load animals from ANIMAL_FILE
        ///// </summary>
        //public override void LoadAnimals()
        //{
        //    // Create streamreader and read animal JSON file
        //    using (StreamReader r = new StreamReader(ANIMAL_FILE))
        //    {
        //        string json = r.ReadToEnd();
        //        breeds = JsonConvert.DeserializeObject<List<Breed>>(json);
        //    }
        //}
    }
}
