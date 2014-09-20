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

        private MainWindow gameWindow;
        public GameView GameView;

        private List<Animal> animals;
        private Stack<Animal> hitlist;
        private List<Breed> breeds;
        
        private Thread animator;

        private volatile bool running = false;
        private volatile bool gameOver = false;

        // Screen size
        public readonly int WIDTH = 800;
        public readonly int HEIGHT = 550;

        // Gameplay variables
        public readonly int NUMBER_OF_SHOTS = 10;
        public readonly int NUMBER_OF_CHICKENS = 10;
        public readonly double GAME_SPEED = 5;

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

        public Game()
        {
            // Load animal JSON file
            LoadAnimals();

            // Initialize score and shotsleft
            Score = 0;
            ShotsLeft = NUMBER_OF_SHOTS;

            // Initialize view
            gameWindow = new MainWindow(WIDTH, HEIGHT);
            GameView = new GameView(this, HEIGHT, WIDTH); // view
            GameView.CreateController();

            gameWindow.GameGrid.Children.Add(GameView);

            gameWindow.Show();

            Start();
        }

        private void Start()
        {
            if (animator == null || !running)
            {
                InitializeGameObjects(NUMBER_OF_CHICKENS);

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
            double delta = 0;

            while (running)
            {
                long now = NanoTime;
                delta += (now - lastTime) / ns;
                lastTime = now;

                if (delta >= 1)
                {
                    Update();
                    delta--;
                }
                Render();
            }
        }
        private void Update()
        {
            // Remove chicken from chickens list that are on the hitlist
            HitmanTheChickenSlayer();

            // Move chickens
            TheChickenMovement();
        }

        private void TheChickenMovement()
        {
            Random rnd = new Random();
            foreach (Animal animal in animals)
            {
                // Horizontal movement
                if (animal.XTrajectory < WIDTH - (animal.Size + 15) && animal.XTrajectory > 0)
                {
                    animal.HorizontalMovement();
                }
                else
                {
                    animal.ChangeHorizontalDirection();
                    animal.HorizontalMovement();
                }
                // Vertical movement
                if (animal.YTrajectory < HEIGHT - (animal.Size * 2) && animal.YTrajectory > 0)
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

        private void HitmanTheChickenSlayer()
        {
            foreach (Animal animal in hitlist)
            {
                animals.Remove(animal);
            }
        }

        private void Render()
        {
            if (!gameOver)
            {
                // Remove animals in hitlist from screen
                while (hitlist.Count != 0)
                    GameView.Remove(hitlist.Pop().Image);

                GameView.Render(animals);
            }
        }

        private void LoadGraphics()
        {
            if (animals != null)
            {
                GameView.Initialize(animals);
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
                animal.XPosition = rnd.Next(animal.Size * 2, WIDTH - animal.Size * 2);
                animal.YPosition = rnd.Next(animal.Size * 2, HEIGHT - animal.Size * 2);

                animals.Add(animal);
            }
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

        public void Shoot(double x, double y)
        {
            --ShotsLeft;
            if (ShotsLeft > 0 || animals.Count == 0)
            {
                foreach (Animal animal in animals)
                {
                    if (animal.IsShot(x, y))
                    {
                        Score += 10;
                        hitlist.Push(animal);
                    }
                }
            }
            else
            {
                EndGame();
            }
        }

        public void Hit(Image img)
        {
            GameView.Remove(img);
        }

        private void EndGame()
        {
            Stop();
            GameView.EndGame(Score);
        }
    }
}
