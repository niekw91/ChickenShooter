using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChickenShooter.Model
{
    public abstract class BaseLevelState
    {
        protected Game game;
        protected string bg;

        public int ID { get; set; }

        protected List<Animal> animals { get { return game.Animals; } }
        protected Stack<Animal> hitlist { get { return game.Hitlist; } }
        protected List<Breed> breeds { get { return game.Breeds; } }
        protected Player player { get { return game.Player; } }
        protected ActionContainer Actions { get { return game.Actions; } }

        public BaseLevelState() { }

        public BaseLevelState(Game game, int id, string bg)
        {
            this.game = game;
            ID = id;
            this.bg = bg;
            LevelFactory.Assign(this);
        }

        public abstract void Update(double dt);
        public abstract void Render(double dt);


        public void LoadGraphics()
        {
            // Set level background
            game.GameView.SetBackground(bg);
            // Draw objects
            if (animals != null)
            {
                game.GameView.Initialize(animals, player);
            }
        }

        protected void NextLevel()
        {
            BaseLevelState next = LevelFactory.NextLevel(this);

            if (next == null)
                next = LevelFactory.Finished();

            game.SwitchLevel(next);
        }
    }
}
