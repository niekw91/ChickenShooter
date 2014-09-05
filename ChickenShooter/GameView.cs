using ChickenShooter.Controller;
using ChickenShooter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ChickenShooter
{
    public class GameView : Canvas
    {
        private Game game;
        private GameController controller;
        public GameView(Game game, int height, int width)
        {
            this.game = game;

            this.Height = height;
            this.Width = width;
        }

        public void CreateController()
        {
            controller = new GameController(game);
        }

        public void Move(Chicken chicken, double newX, double newY)
        {
            var top = chicken.PositionLeft;
            var left = chicken.PositionTop;
            TranslateTransform trans = new TranslateTransform();
            chicken.Image.RenderTransform = trans;
            DoubleAnimation anim1 = new DoubleAnimation(top, newY - top, TimeSpan.FromSeconds(10));
            DoubleAnimation anim2 = new DoubleAnimation(left, newX - left, TimeSpan.FromSeconds(10));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);
        }

        public void Initialize(List<Chicken> chickens)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                for(int i = 0; i < chickens.Count; i++)
                {
                    this.Children.Add(chickens[i].Image);
                    Canvas.SetTop(chickens[i].Image, chickens[i].PositionTop);
                    Canvas.SetTop(chickens[i].Image, chickens[i].PositionLeft);
                }
            }));
        }

        public void Render(List<Chicken> chickens)
        {
            foreach(Chicken chicken in chickens)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
                {
                    Move(chicken, 10.0, 20.0);
                }));
            }
        }

        public void Remove(Image img)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(() =>
            {
                this.Children.Remove(img);
            }));
        }

        public void EndGame()
        {
            int score = game.NUMBER_OF_CHICKENS - this.Children.Count; 

            // Create label
            Label lblEnd = new Label();
            lblEnd.Content = String.Format("Game Over, you scored {0}!", score);
            lblEnd.FontSize = 26;
            this.Children.Add(lblEnd);

            Canvas.SetTop(lblEnd, (game.HEIGHT / 2.5));
            Canvas.SetLeft(lblEnd, (game.WIDTH / 3.5));
        }
    }
}
