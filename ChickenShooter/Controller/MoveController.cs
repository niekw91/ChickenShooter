using ChickenShooter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChickenShooter.Controller
{
    public class MoveController
    {
        private ActionContainer actions;
        public int[] PlayerMoves { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public MoveController(ActionContainer actions)
        {
            this.actions = actions;
            Speed = 1;
            PlayerMoves = new int[2];
            Reset();
        }
        internal void GameView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Move(e.Key.ToString());
        }
        public void Move(string key)
        {
            switch (key.ToLower())
            {
                case "a":
                    X -= Speed;
                    break;
                case "w":
                    Y -= Speed;
                    break;
                case "s":
                    Y += Speed;
                    break;
                case "d":
                    X += Speed;
                    break;
                default:
                    break;
            }
            PlayerMoves[0] += X;
            PlayerMoves[1] += Y;
        }
        public void Reset()
        {
            X = 0;
            Y = 0;
        }
    }
}
