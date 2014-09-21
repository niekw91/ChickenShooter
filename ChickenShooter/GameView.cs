using ChickenShooter.Controller;
using ChickenShooter.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ChickenShooter
{
    public class GameView : Canvas
    {
        private Game game;
        private GameController controller;
        public GameView(Game game, int width, int height)
        {
            this.game = game;
            // Set background transparent for mouse event detection
            Color c = Colors.Black;
            c.A = 0;
            this.Background = new SolidColorBrush(c);
            DataContext = this;

            this.Height = height;
            this.Width = width;

            this.MouseLeftButtonDown += Shoot;
        }

        public void AddController(GameController gameController)
        {
            this.controller = gameController;
        }

        private void Shoot(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (game.ShotsLeft != 0)
            {
                double x = e.GetPosition(this.game.GameView).X;
                double y = e.GetPosition(this.game.GameView).Y;
                controller.Shoot(x, y);
            }
        }

        public void Initialize(List<Animal> animals)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                for (int i = 0; i < animals.Count; i++)
                {
                    this.Children.Add(animals[i].Image);
                }
                CreateLabels();
            }));
        }

        public void Render(List<Animal> animals)
        {
            foreach (Animal animal in animals)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                {
                    Animate(animal);
                }));
            }
        }

        private void CreateLabels()
        {
            // Create shots left label
            Label lblShots = new Label();
            lblShots.FontSize = 20;
            lblShots.Foreground = Brushes.White;
            lblShots.FontWeight = FontWeights.Bold;
            Label lblShotsLeft = new Label();
            lblShotsLeft.Content = String.Format("Shots left:");
            lblShotsLeft.FontSize = 20;
            lblShotsLeft.Foreground = Brushes.White;
            lblShotsLeft.FontWeight = FontWeights.Bold;
            // Create binding    
            Binding binding = new Binding("ShotsLeft");
            binding.Source = game;
            lblShots.SetBinding(Label.ContentProperty, binding);
            // Add elements to canvas
            this.Children.Add(lblShotsLeft);
            this.Children.Add(lblShots);
            Canvas.SetRight(lblShotsLeft, 80);
            Canvas.SetTop(lblShotsLeft, 10);
            Canvas.SetRight(lblShots, 50);
            Canvas.SetTop(lblShots, 10);
        }

        private void Animate(Animal animal)
        {
            TranslateTransform trans = new TranslateTransform();
            animal.Image.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(animal.XPosition, animal.XTrajectory, TimeSpan.FromSeconds(10));
            DoubleAnimation anim2 = new DoubleAnimation(animal.YPosition, animal.YTrajectory, TimeSpan.FromSeconds(10));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);
        }

        public void Remove(Image img)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                this.Children.Remove(img);
            }));
        }

        public void EndGame(int score)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                // Create label
                Label lblEnd = new Label();
                lblEnd.Content = String.Format("Game Over, you scored {0}!", game.Score);
                lblEnd.FontSize = 26;

                this.Children.Add(lblEnd);

                Canvas.SetTop(lblEnd, (game.Height / 2.5));
                Canvas.SetLeft(lblEnd, (game.Width / 3.5));
            }));
        }
    }
}
