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
using System.Windows.Threading;

namespace ChickenShooter.Model
{
    public class Game : INotifyPropertyChanged
    {
        public GameView GameView;

        private readonly string ANIMAL_FILE = "animals.json";

        public Player Player { get; set; }

        public BaseLevelState Level { get; set; }

        public List<Animal> Animals { get; set; }
        public Stack<Animal> Hitlist { get; set; }
        public List<Breed> Breeds { get; set; }

        public ActionContainer Actions { get; set; }

        private Thread animator;

        public volatile bool running = false;
        public volatile bool gameOver = false;

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

        private int stage;
        public int Stage
        {
            get { return stage; }
            set
            {
                stage = value;
                OnPropertyChanged("Stage");
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

        public Game(int shotCount = 10, int animalCount = 10, double gameSpeed = 5)
        {
            // Create levels
            new LevelOne(this, @"../../Images/background.jpg");
            new LevelTwo(this, @"../../Images/background-stage-2.jpg");
            new LevelFinished(this, null);

            // Set game level
            SetLevel(LevelFactory.GetFirstLevel());

            Actions = new ActionContainer();

            ResetGameStats();
        }

        public void SetLevel(BaseLevelState level)
        {
            if (level != null)
                this.Level = level;
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
                // Load animal JSON file
                LoadAnimals();
                // initialize game objects
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

        public void Resume()
        {
            Run();
        }

        public static long NanoTime
        {
            get { return (long)(Stopwatch.GetTimestamp() / (Stopwatch.Frequency / 1000000000.0)); }
        }

        private void ResetGameStats()
        {
            ShotsLeft = 10;
            NumberOfAnimals = 10;
            GameSpeed = 5;
            Stage = Level.ID;
        }

        public void Run()
        {
            running = true;
            Level.LoadGraphics();
            ResetGameStats();

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
                    Level.Update(dt);
                    dt--;
                }
                Level.Render(dt);
                Thread.Sleep(10);
            }
        }

        public void SwitchLevel(BaseLevelState level)
        {
            Stop();
            GameView.ClearElements();

            SetLevel(level);
            Resume();
        }

        public void EndGame()
        {
            Stop();
            GameView.EndGame(Score);
        }

        public void InitializeGameObjects(int animalCount)
        {
            Animals = new List<Animal>();
            Hitlist = new Stack<Animal>();
            // Create player
            Player = new Player();

            Random rnd = new Random();
            for (int i = 0; i < animalCount; i++)
            {
                // Generate random index
                int ind = rnd.Next(0, Breeds.Count);
                // Create animal using factory
                Animal animal = Breeds[ind].CreateAnimal();
                // Set x and y
                animal.XPosition = rnd.Next(animal.Size * 2, Width - animal.Size * 2);
                animal.YPosition = rnd.Next(animal.Size * 2, Height - animal.Size * 2);

                Animals.Add(animal);
            }
        }

        /// <summary>
        /// Load animals from ANIMAL_FILE
        /// </summary>
        public void LoadAnimals()
        {
            // Create streamreader and read animal JSON file
            using (StreamReader r = new StreamReader(ANIMAL_FILE))
            {
                string json = r.ReadToEnd();
                Breeds = JsonConvert.DeserializeObject<List<Breed>>(json);
            }
        }
    }
}
