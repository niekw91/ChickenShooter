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
    public class Animal
    {
        private Breed breed;
        public int Size { get { return breed.Size; } }
        public int Speed { get { return breed.Speed; } }


        public Image Image { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public double XTrajectory { get; set; }
        public double YTrajectory { get; set; }
        public bool IsMovingDown { get; set; }
        public bool IsMovingRight { get; set; }


        private Random rnd = new Random();
        public Animal(Breed breed)
        {
            this.breed = breed;

            IsMovingRight = rnd.Next(0, 1) == 1 ? true : false;
            IsMovingDown = rnd.Next(0, 1) == 1 ? true : false;

            if (IsMovingRight)
            {
                XTrajectory = XPosition + Speed;
            }
            else
            {
                XTrajectory = XPosition - Speed;
            }
            if (IsMovingDown)
            {
                YTrajectory = YPosition + Speed;
            }
            else
            {
                YTrajectory = YPosition - Speed;
            }
            // Set image
            BitmapImage bitmap = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\..\..\Images\" + breed.ImagePath));
            Image = new Image();
            Image.Name = "imgChicken";
            Image.Width = Size;
            Image.Height = Size;
            Image.Source = bitmap;
        }

        public void HorizontalMovement()
        {
            if (IsMovingRight)
            {
                XPosition += Speed;
                XTrajectory = XPosition + Speed;
            }
            else
            {
                XPosition -= Speed;
                XTrajectory = XPosition - Speed;
            }
        }
        public void VerticalMovement()
        {

            if (IsMovingDown)
            {
                YPosition += Speed;
                YTrajectory = YPosition + Speed;
            }
            else
            {
                YPosition -= Speed;
                YTrajectory = YPosition - Speed;
            }
        }

        public bool IsShot(double x, double y)
        {
            if ((x > XPosition && x < XPosition + Size) && (y > YPosition && y < YPosition + Size))
            {
                return true;
            }
            return false;
        
        }
        public void ChangeHorizontalDirection()
        {
            IsMovingRight = !IsMovingRight;
        }
        public void ChangeVerticalDirection()
        {
            IsMovingDown = !IsMovingDown;
        }
    }
}
