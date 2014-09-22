using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ChickenShooter.Model
{
    public class Player
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double XTrajectory { get; set; }
        public double YTrajectory { get; set; }

        public double Speed { get; set; }
        public int Size { get ; set; }


        public Image Image { get; set; }

        public Player()
        {
            Random rnd = new Random();
            X = rnd.Next(100, 200);
            Y = rnd.Next(100, 200);
            Speed = 1;
            Size = 40;
            // Set image
            BitmapImage bitmap = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\..\..\Images\player.png" ));
            Image = new Image();
            Image.Name = "imgPlayer";
            Image.Width = Size;
            Image.Height = Size;
            Image.Source = bitmap;
        }

    }
}
