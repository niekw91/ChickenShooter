using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChickenShooter.Model
{
    public class Game : INotifyPropertyChanged
    {
        private MainWindow gameWindow;
        public GameView GameView;

        private List<Chicken> chickens;
        private List<Chicken> hitlist;
        private Thread animator;

        private volatile bool running = false;
        private volatile bool gameOver = false;

        // Screen size
        public readonly int WIDTH = 800;
        public readonly int HEIGHT = 550;

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
            // Initialize score and shotsleft
            Score = 0;
            ShotsLeft = 10;

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
                ShotsLeft = 10;

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
            foreach (Chicken chicken in chickens)
            {
                // Horizontal movement
                if (chicken.XTrajectory < WIDTH - (chicken.Size) && chicken.XTrajectory > -100)
                {
                    chicken.HorizontalMovement(GAME_SPEED);
                }
                else
                {
                    chicken.ChangeHorizontalDirection();
                    chicken.HorizontalMovement(GAME_SPEED);
                }
                // Vertical movement
                if (chicken.YTrajectory < HEIGHT - (chicken.Size + 100) && chicken.YTrajectory > -100)
                {
                    chicken.VerticalMovement(GAME_SPEED);
                }
                else
                {
                    chicken.ChangeVerticalDirection();
                    chicken.VerticalMovement(GAME_SPEED);
                }
            }
        }

        private void HitmanTheChickenSlayer()
        {
            foreach (Chicken chicken in hitlist)
            {
                chickens.Remove(chicken);
            }
        }

        private void Render()
        {
            if (!gameOver)
            {
                GameView.Render(chickens);
            }
        }

        private void LoadGraphics()
        {
            if (chickens != null)
            {
                GameView.Initialize(chickens);
            }
        }

        public void InitializeGameObjects(int chickenCount)
        {
            chickens = new List<Chicken>();
            hitlist = new List<Chicken>();
            Random rnd = new Random();
            for (int i = 0; i < chickenCount; i++)
            {
                chickens.Add(new Chicken(rnd.Next(1, WIDTH), rnd.Next(1, HEIGHT)));
            }
        }

        public void Shoot(double x, double y)
        {
            ShotsLeft--;
            if (ShotsLeft > 0)
            {
                foreach (Chicken chicken in chickens)
                {
                    if(chicken.IsShot(x, y))
                    {
                        Score += 10;
                        hitlist.Add(chicken);
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
            GameView.EndGame();
        }
    }
}
