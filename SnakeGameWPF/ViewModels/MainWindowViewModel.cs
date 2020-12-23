using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGameWPF.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private GameEngine gameEngine;
        
        private int score;
        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }

        private int life;
        public int Life
        {
            get => life;
            set
            {
                life = value;
                OnPropertyChanged("Life");
            }
        }

        public MainWindowViewModel()
        {
            gameEngine = new GameEngine();
        }
    }
}
