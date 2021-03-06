﻿using ChickenShooter.Controller;
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
using System.Windows.Media.Imaging;
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

        public void SetBackground(string path)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                ImageBrush ib = new ImageBrush();
                ib.ImageSource = new BitmapImage(new Uri(path, UriKind.Relative));
                this.Background = ib;
            }));
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

        public void Initialize(List<Animal> animals, Player player)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                for (int i = 0; i < animals.Count; i++)
                {
                    this.Children.Add(animals[i].Image);
                }
                this.Children.Add(player.Image);
                CreateLabels();
            }));
        }

        public void Render(List<Animal> animals, Player player)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                foreach (Animal animal in animals)
                {
                    Animate(animal);
                }
                AnimatePlayer(player);
            }));
        }

        private void AnimatePlayer(Player player)
        {
            TranslateTransform trans = new TranslateTransform();
            player.Image.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(player.X, player.XTrajectory, TimeSpan.FromSeconds(0));
            DoubleAnimation anim2 = new DoubleAnimation(player.Y, player.YTrajectory, TimeSpan.FromSeconds(0));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);
            player.X = player.XTrajectory;
            player.Y = player.YTrajectory;
        }

        private void CreateLabels()
        {
            // Create stage level label
            Label lblStage = new Label();
            lblStage.FontSize = 20;
            lblStage.Foreground = Brushes.White;
            lblStage.FontWeight = FontWeights.Bold;
            Label lblCurrStage = new Label();
            lblCurrStage.Content = String.Format("Stage ");
            lblCurrStage.FontSize = 20;
            lblCurrStage.Foreground = Brushes.White;
            lblCurrStage.FontWeight = FontWeights.Bold;
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
            // Create binding stage
            Binding stage = new Binding("Stage");
            stage.Source = game;
            lblStage.SetBinding(Label.ContentProperty, stage);
            // Add elements to canvas
            this.Children.Add(lblCurrStage);
            this.Children.Add(lblStage);
            this.Children.Add(lblShotsLeft);
            this.Children.Add(lblShots);
            Canvas.SetLeft(lblCurrStage, 10);
            Canvas.SetTop(lblCurrStage, 10);
            Canvas.SetLeft(lblStage, 70);
            Canvas.SetTop(lblStage, 10);
            Canvas.SetRight(lblShotsLeft, 80);
            Canvas.SetTop(lblShotsLeft, 10);
            Canvas.SetRight(lblShots, 50);
            Canvas.SetTop(lblShots, 10);
        }

        private void Animate(Animal animal)
        {
            TranslateTransform trans = new TranslateTransform();
            animal.Image.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(animal.XPosition, animal.XTrajectory, TimeSpan.FromSeconds(0));
            DoubleAnimation anim2 = new DoubleAnimation(animal.YPosition, animal.YTrajectory, TimeSpan.FromSeconds(0));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);
        }

        public void DrawLoadingNext(int score)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                // Create stage level label
                Label lblScore = new Label();
                lblScore.Content = String.Format("Loading Next Level\nCurrent Score is {0}", score);
                lblScore.FontSize = 20;
                lblScore.Foreground = Brushes.White;
                lblScore.FontWeight = FontWeights.Bold;

                // Add elements to canvas
                this.Children.Add(lblScore);

                Canvas.SetLeft(lblScore, (game.Width / 2.7));
                Canvas.SetTop(lblScore, (game.Height / 2.5));
            }));
        }

        public void Remove(Image img)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                this.Children.Remove(img);
            }));
        }

        public void ClearElements()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                this.Children.Clear();
            }));
        }

        public void PrepareEndGame()
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                this.Children.Clear();
            }));
        }

        public void EndGame(int score)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                // Create label
                Label lblEnd = new Label();
                lblEnd.Content = String.Format("You scored {0}!", game.Score);
                lblEnd.FontSize = 28;
                lblEnd.Foreground = Brushes.White;

                this.Children.Add(lblEnd);

                Canvas.SetTop(lblEnd, (game.Height / 2.5));
                Canvas.SetLeft(lblEnd, (game.Width / 2.7));
            }));
        }
    }
}
