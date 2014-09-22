using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChickenShooter.Model
{
    public class Game : INotifyPropertyChanged
    {
        private readonly string ANIMAL_FILE = "animals.json";

        public GameView GameView;

        private List<Animal> animals;
        private Stack<Animal> hitlist;
        private List<Breed> breeds;
        
        private Thread animator;

        private volatile bool running = false;
        private volatile bool gameOver = false;

        // Gameplay variables
        public int NumberOfAnimals { get; set; }
        public double GameSpeed { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Score { get; set; }
        private int shotsLeft;
        public int ShotsLeft
        {
            get { return shotsLeft; }
            set
            {
                shotsLeft = value;
                OnPropertyChanged("ShotsLeft");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        public ActionContainer Actions { get; set; }

        public Game(int shotCount = 10, int animalCount = 10, double gameSpeed = 5)
        {
            // Load animal JSON file
            LoadAnimals();

            // Initialize score and shotsleft
            Score = 0;
            ShotsLeft = shotCount;
            NumberOfAnimals = animalCount;
            GameSpeed = gameSpeed;

            Actions = new ActionContainer();
        }

        public void AddView(GameView gameView)
        {
            this.GameView = gameView;
            Width = (int)gameView.Width;
            Height = (int)gameView.Height;
        }

        public void Start()
        {
            if (animator == null || !running)
            {
                InitializeGameObjects(this.NumberOfAnimals);

                animator = new Thread(Run);
                animator.SetApartmentState(ApartmentState.STA);
                animator.Start();
            }
        }

        public void Stop()
        {
            running = false;
        }

        public static long NanoTime
        {
            get { return (long)(Stopwatch.GetTimestamp() / (Stopwatch.Frequency / 1000000000.0)); }
        } 

        public void Run()
        {
            running = true;
            LoadGraphics();

            long lastTime = NanoTime;
            double fps = 60.0;
            double ns = 1000000000 / fps;
            double dt = 0.0;

            while (running)
            {
                long now = NanoTime;
                dt += (now - lastTime) / ns;
                lastTime = now;

                if (dt >= 1)
                {
                    Update(dt);
                    dt--;
                }
                Render(dt);
                Thread.Sleep(10);
            }
        }
        private void Update(double dt)
        {
            // Remove animals from list that are on the hitlist
            DetermineTargets(Actions.ShotsFired);
            KillTargets();
            // Move player
            player.XTrajectory = player.X + Actions.Moves.PlayerMoves[0];
            player.YTrajectory = player.Y + Actions.Moves.PlayerMoves[1];
            player.XTrajectory = player.XTrajectory > Width - player.Size ? Width - player.Size : player.XTrajectory;
            player.XTrajectory = player.XTrajectory < 0 ? 0 : player.XTrajectory;
            player.YTrajectory = player.YTrajectory > Height - player.Size*2 ? Height - player.Size : player.YTrajectory;
            player.YTrajectory = player.YTrajectory < 0 ? 0 : player.YTrajectory;
            
            //Actions.Moves.Reset();
            // Move animals
            CalculateMovement();
        }

        private void DetermineTargets(Stack<Bullet> bullets)
        {
            foreach (Bullet bullet in bullets)
            {
                foreach (Animal animal in animals)
                {
                    if (animal.IsShot(bullet.X, bullet.Y))
                    {
                        Score += 10;
                        hitlist.Push(animal);
                    }
                }
            }
            Actions.ShotsFired.Clear();
            // End game if no more shots left
            if (ShotsLeft == 0)
                EndGame();
        }

        private void CalculateMovement()
        {
            Random rnd = new Random();
            foreach (Animal animal in animals)
            {
                // Horizontal movement
                if (animal.XTrajectory < Width - (animal.Size + 15) && animal.XTrajectory > 0)
                {
                    animal.HorizontalMovement();
                }
                else
                {
                    animal.ChangeHorizontalDirection();
                    animal.HorizontalMovement();
                }
                // Vertical movement
                if (animal.YTrajectory < Height - (animal.Size * 2) && animal.YTrajectory > 0)
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

        private void KillTargets()
        {
            foreach (Animal animal in hitlist)
            {
                animals.Remove(animal);
            }
            // End game if all animals are dead
            if (animals.Count == 0)
                EndGame();
        }

        private void Render(double dt)
        {
            if (!gameOver)
            {
                // Remove animals in hitlist from screen
                while (hitlist.Count != 0)
                    GameView.Remove(hitlist.Pop().Image);

                GameView.Render(animals, player);
            }
        }

        private void LoadGraphics()
        {
            if (animals != null)
            {
                GameView.Initialize(animals, player);
            }
        }

        public void InitializeGameObjects(int animalCount)
        {
            animals = new List<Animal>();
            hitlist = new Stack<Animal>();
            Random rnd = new Random();
            for (int i = 0; i < animalCount; i++)
            {
                // Generate random index
                int ind = rnd.Next(0, breeds.Count);
                // Create animal using factory
                Animal animal = breeds[ind].CreateAnimal();
                // Set x and y
                animal.XPosition = rnd.Next(animal.Size * 2, Width - animal.Size * 2);
                animal.YPosition = rnd.Next(animal.Size * 2, Height - animal.Size * 2);

                animals.Add(animal);
            }
            player = new Player(); 
        }

        /// <summary>
        /// Load animals from ANIMAL_FILE
        /// </summary>
        private void LoadAnimals()
        {
            // Create streamreader and read animal JSON file
            using (StreamReader r = new StreamReader(ANIMAL_FILE))
            {
                string json = r.ReadToEnd();
                breeds = JsonConvert.DeserializeObject<List<Breed>>(json);
            }
        }

        private void EndGame()
        {
            Stop();
            GameView.EndGame(Score);
        }

        public Player player { get; set; }
    }
}
