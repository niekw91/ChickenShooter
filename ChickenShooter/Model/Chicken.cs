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
    public class Chicken
    {
        public Image Image { get; set; }
        public BitmapImage Bitmap { get; set; }
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public double XTrajectory { get; set; }
        public double YTrajectory { get; set; }
        public int Size { get; set; }
        public bool IsMovingDown { get; set; }
        public bool IsMovingRight { get; set; }
        public double Speed { get; set; }
        public Chicken()
        {
            // Initialize properties
            Size = 40;
            XPosition = 100;
            YPosition = 100;
            IsMovingRight = new Random().Next(0, 1) == 1 ? true : false;
            IsMovingDown = new Random().Next(0, 1) == 1 ? true : false;
            Speed = new Random().NextDouble();
            // Set image
            Bitmap = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\..\..\Images\chicken.png"));
            Image = new Image();
            Image.Name = "imgChicken";
            Image.Width = Size;
            Image.Height = Size;
            Image.Source = Bitmap;
        }
        public Chicken(double xposition, double yposition)
        {
            // Initialize properties
            Size = 40;
            XPosition = xposition;
            YPosition = yposition;
            Random rnd = new Random();
            IsMovingRight = rnd.Next(0, 1) == 1 ? true : false;
            IsMovingDown = rnd.Next(0, 1) == 1 ? true : false;
            Speed = rnd.NextDouble();

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
            Bitmap = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\..\..\Images\chicken.png"));
            Image = new Image();
            Image.Name = "imgChicken";
            Image.Width = Size;
            Image.Height = Size;
            Image.Source = Bitmap;
        }

        public void HorizontalMovement(double gamespeed)
        {
            if (IsMovingRight)
            {
                XPosition += Speed * gamespeed;
                XTrajectory = XPosition + Speed * gamespeed;
            }
            else
            {
                XPosition -= Speed * gamespeed;
                XTrajectory = XPosition - Speed * gamespeed;
            }
        }
        public void VerticalMovement(double gamespeed)
        {

            if (IsMovingDown)
            {
                YPosition += Speed * gamespeed;
                YTrajectory = YPosition + Speed * gamespeed;
            }
            else
            {
                YPosition -= Speed * gamespeed;
                YTrajectory = YPosition - Speed * gamespeed;
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
