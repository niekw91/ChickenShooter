using ChickenShooter.Controller;
using ChickenShooter.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ChickenShooter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Screen size
        public readonly int WIDTH = 800;
        public readonly int HEIGHT = 550;

        // Gameplay variables
        public readonly int NUMBER_OF_SHOTS = 10;
        public readonly int NUMBER_OF_ANIMALS = 10;
        public readonly double GAME_SPEED = 5;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create model
            Game game = new Game(NUMBER_OF_SHOTS,NUMBER_OF_ANIMALS);

            // Create view
            MainWindow gameWindow = new MainWindow(WIDTH, HEIGHT);
            GameView GameView = new GameView(game, WIDTH, HEIGHT); // view
            gameWindow.GameGrid.Children.Add(GameView);
            gameWindow.Show();

            // Add view to model
            game.AddView(GameView);

            // Start game
            game.Start();
        }
    }
}
