using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Model
{
    public class ActionContainer
    {
        public Stack<Bullet> ShotsFired { get; set; }
        public Player PlayerMoves { get; set; }
        public ActionContainer()
        {
            ShotsFired = new Stack<Bullet>();
            PlayerMoves = new Player();
        }
    }
}
