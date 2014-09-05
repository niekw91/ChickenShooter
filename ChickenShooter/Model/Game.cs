using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ChickenShooter.Model
{
    public class Game
    {
        private MainWindow gameWindow;
        public GameView GameView;

        //private Chicken chicken;
        private List<Chicken> chickens;
        private Thread animator;

        private volatile bool running = false;
        private volatile bool gameOver = false;



        // Screen size
        public readonly int WIDTH = 800;
        public readonly int HEIGHT = 550;

        public readonly int NUMBER_OF_CHICKENS = 10;
        public readonly double SPEED = 1;

        private int numberOfShots = 10;

        public Game()
        {
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
            Random rnd = new Random();
            foreach(Chicken chicken in chickens)
            {
                chicken.Move(rnd.NextDouble() * SPEED);
            }
            //if (chicken.PositionLeft > WIDTH || chicken.PositionLeft < 0)
            //{
            //    //chicken.Remove = true;
            //    //chicken = null;
            //}
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
            Random rnd = new Random();
            for (int i = 0; i < chickenCount; i++)
            {
                chickens.Add(new Chicken() { PositionTop = rnd.Next(40, HEIGHT - 40) } );
            }
        }

        public void Hit(Image img)
        {
            GameView.Remove(img);
        }

        public void Shoot()
        {
            numberOfShots--;
            if (numberOfShots == 0)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            Stop();
            GameView.EndGame();
        }
    }
}
