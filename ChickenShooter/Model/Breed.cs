using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model
{
    public class Breed
    {
        public int Size { get; set; }
        public int Speed { get; set; }
        public string ImagePath { get; set; }
        public Breed(int size, int speed, string imagePath)
        {
            Size = size;
            Speed = speed;
            ImagePath = imagePath;
        }

        public Animal CreateAnimal()
        {
            return new Animal(this);
        }
    }
}
