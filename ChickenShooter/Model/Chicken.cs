using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ChickenShooter.Model
{
    public class Chicken
    {
        public Image Image { get; set; }
        public BitmapImage Bitmap { get; set; }
        public double PositionLeft { get; set; }
        public double PositionTop { get; set; }
        public bool Remove { get; set; }

        private Random random = new Random();

        public Chicken()
        {
            Bitmap = new BitmapImage(new Uri(@"C:\Users\Niek Willems\Documents\Visual Studio 2013\Projects\ChickenShooter\ChickenShooter\Images\chicken.png"));

            Image = new Image();
            Image.Name = "imgChicken";
            Image.Width = 40;
            Image.Height = 40;
            Image.Source = Bitmap;

            PositionLeft = 0;
        }

        public void Move(double left)
        {
            PositionLeft += left;
        }
    }
}
