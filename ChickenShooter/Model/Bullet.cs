using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model
{
    public class Bullet
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Bullet(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
