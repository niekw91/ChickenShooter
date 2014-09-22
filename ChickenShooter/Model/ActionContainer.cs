using ChickenShooter.Controller;
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
        public ActionContainer()
        {
            ShotsFired = new Stack<Bullet>();
        }

        public void AddMovesController(MoveController moveController)
        {
            this.Moves = moveController;
        }

        public MoveController Moves { get; set; }
    }
}
